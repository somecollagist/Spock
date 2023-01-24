using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spock.Core
{
	internal static class Solver
	{
		/// <summary>
		/// Regex to filter for boolean operators (unary and dyadic).
		/// </summary>
		internal static Regex Re_Operator = new(@"[\!\&\|\^]");
		/// <summary>
		/// Regex to filter for any valid identifiers (upper case letters).
		/// </summary>
		internal static Regex Re_Identifier = new(@"([A-Z])");
		/// <summary>
		/// Regex to filter for constants (0 and 1).
		/// </summary>
		internal static Regex Re_Constant = new(@"[01]");

		/// <summary>
		/// The circuit being currently used.
		/// </summary>
		public static Lamp Circuit = new();
		/// <summary>
		/// A binding of input identifiers to their respective gates in the loaded circuit.
		/// </summary>
		public static Dictionary<char, List<Input>> InputBinds = new();

		/// <summary>
		/// Generates a component tree representing the given circuit.
		/// </summary>
		/// <param name="expr">The circuit in Polish notation.</param>
		/// <returns>A component tree representing the given circuit.</returns>
		public static Component GenerateCircuit(string expr)
		{
			int _ = 0;
			return GenerateCircuit(expr, ref _);
		}

		/// <summary>
		/// Generates a component tree representing the given circuit.
		/// </summary>
		/// <param name="expr">The circuit in Polish notation.</param>
		/// <param name="offset">The character to be parsed.</param>
		/// <returns>A component tree representing the given circuit.</returns>
		public static Component GenerateCircuit(string expr, ref int offset)
		{
			char c = expr[offset++];
			if (Re_Identifier.IsMatch(c.ToString()))
			{
				// Create an input component
				Input i = new() { ComponentContent = c.ToString() };
				// Add to registry if needed
				if (!InputBinds.ContainsKey(c))
					InputBinds.Add(c, new());
				InputBinds[c].Add(i);
				return i;
			}

			// Constants
			if (c == '0')
				return new ConstantZero();
			if (c == '1')
				return new ConstantOne();

			// Operator component, store in temporary
			Component gate;
			switch(c)
			{
				case '!':
					gate = new NOT();
					goto unaryoperator;
				case '&':
					gate = new AND();
					goto diadicoperator;
				case '|':
					gate = new OR();
					goto diadicoperator;
				case '^':
					gate = new XOR();
					goto diadicoperator;
				default:
					throw new Exception("Malformed Boolean Expression");
			}

		// For gates with only one input.
		unaryoperator:
			gate.Inputs.Add(GenerateCircuit(expr, ref offset));
			goto end;

		// For gates with two inputs.
		diadicoperator:
			gate.Inputs.Add(GenerateCircuit(expr, ref offset));
			gate.Inputs.Add(GenerateCircuit(expr, ref offset));	// Take advantage of side effects
			goto end;

		end:
			return gate;
		}

		/// <summary>
		/// Represents a circuit in infix notation.
		/// </summary>
		/// <param name="c">The circuit to be represented.</param>
		/// <returns>An infix notation representation of the circuit.</returns>
		public static string GenerateInfixExpr(this Component c)
		{
			// In-order tree traversal!
			
			string ret = "";
			// There should be no more than two inputs
			if (c.Inputs.Count > 0) ret += c.Inputs[0].GenerateInfixExpr();
			ret += c.ComponentContent;
			if (c.Inputs.Count > 1) ret += c.Inputs[1].GenerateInfixExpr();

			// NOT gates are on the wrong side, so flip them round
			if (ret[^1] == '!') ret = $"!{ret[..^1]}";
			// If there's two inputs, it might be more readable to wrap them in brackets
			if (c.Inputs.Count > 1) ret = $"({ret})";

			return ret;
		}

		/// <summary>
		/// Simplifies the given expression in accordance to the laws of boolean algebra.
		/// </summary>
		/// <param name="expr">The expression to simplify.</param>
		/// <returns>The simplified expression.</returns>
		public static (string, int) Simplify(string expr) => Simplify(expr, '\0');

		/// <summary>
		/// Simplifies the given expression in accordance to the laws of boolean algebra.
		/// </summary>
		/// <param name="expr">The expression to simplify.</param>
		/// <param name="context">The preceding character that has been parsed.</param>
		/// <returns>The simplified expression.</returns>
		public static (string, int) Simplify(string expr, char context)
		{
			string ret;	// Temp
			int length; // Length of string to simplify

			string op1;	// Expression of first operand
			string op2;	// Expression of second operand
			int l1;		// Length of first operand
			int l2;		// Length of second operand
			bool op1n;	// Is the first operand negated?
			bool op2n;	// Is the second operand negated?

			// Zero length expressions are automatically bad
			if (expr.Length == 0)
			{
				throw new Exception("Malformed Boolean Expression!");
			}

			switch(expr[0])
			{
				case '!':
					(op1, length) = Simplify(expr[1..], '!');	// Pass in context here for simplifying via DeMorgans
					if (Re_Constant.IsMatch(op1))
					{
						// Convert negated constants
						ret = op1 == "1" ? "0" : "1";
						length = 2;
					}
					else
					{
						// Otherwise there's not really a lot we can do :(
						ret = $"!{op1}";
						length++;
					}
					break;

				case '&':
				case '|':
					length = 0;
					(op1, l1) = Simplify(expr[1..]);
					(op2, l2) = Simplify(expr[(1 + l1)..]);	// Include the offset from the previous operand
					op1n = op1[0] == '!';
					op2n = op2[0] == '!';

					// Self negatives
					if (op1 == $"!{op2}" || op2 == $"!{op1}")
						ret = expr[0] == '&' ? "0" : "1";

					// With constant 1
					else if (op1 == "1" || op2 == "1")
						ret = expr[0] == '&' ? (op1 == "1" ? op2 : op1) : "1";

					// With constant 0
					else if (op1 == "0" || op2 == "0")
						ret = expr[0] == '|' ? (op1 == "1" ? op2 : op1) : "0";

					// I have no clue what I was doing here but it seems unnecessary
					//else if (op1 == (expr[0] == '&' ? "!" : "0"))
					//	ret = op2;

					//else if (op2 == (expr[0] == '&' ? "!" : "0"))
					//	ret = op1;

					// Self repetition
					else if (op1 == op2)
						ret = op1;

					// DeMorgans reduction
					else if (op1n || op2n)
					{
						// If either is negative then generate a logically equivalent expression
						ret = $"!{(expr[0] == '&' ? "|" : "&")}{(op1n ? "" : "!")}{op1[(op1n ? 1 : 0)..]}{(op2n ? "" : "!")}{op2[(op2n ? 1 : 0)..]}";	//FEAR ME
						if (expr.Length < ret.Length - (context == '!' ? 1 : 0))
						{
							// Simplified string may be longer (i.e. more complex), so check if context makes it shorter, and use the shorter one
							ret = $"{(expr[0] == '&' ? "&" : "|")}{op1}{op2}";
							length = 0;
						}
						else length = 0;
					}

					else ret = $"{(expr[0] == '&' ? "&" : "|")}{op1}{op2}";
					length += 1 + l1 + l2;
					break;

				case '^':
					(op1, l1) = Simplify(expr[1..]);
					(op2, l2) = Simplify(expr[(1 + l1)..]);

					// Identical operands results in 0
					if (op1 == op2)
						ret = "0";

					// Redundant operators
					else if (op1 == "0")
						ret = op2;
					else if (op2 == "0")
						ret = op1;

					else ret = $"^{op1}{op2}";
					length = 1 + l1 + l2;
					break;

				default:
					if (Re_Constant.IsMatch(expr[0].ToString()) || Re_Identifier.IsMatch(expr[0].ToString())) return (expr[0].ToString(), 1);
					else throw new Exception("Malformed Boolean Expression"); // Throws on anything other than an operator, identifier, or constant
			}

			return (ret.Replace("!!", ""), length); // Remove double negatives
		}
	}
}
