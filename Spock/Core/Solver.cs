﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spock.Core
{
	internal static class Solver
	{
		internal static Regex Re_Operator = new(@"[\!\&\|\^]");
		internal static Regex Re_Identifier = new(@"([A-Z])");
		internal static Regex Re_Constant = new(@"[01]");

		public static (string, int) Simplify(string expr) => Simplify(expr, '\0');

		public static (string, int) Simplify(string expr, char context)
		{
			string ret;
			int length; // Length of string to simplify

			string op1;
			string op2;
			int l1;
			int l2;
			bool op1n;
			bool op2n;

			if (expr.Length == 0)
			{
				throw new Exception("Malformed Boolean Expression!");
			}

			switch(expr[0])
			{
				case '!':
					(op1, length) = Simplify(expr[1..], '!');
					if (Re_Constant.IsMatch(op1))
					{
						ret = op1 == "1" ? "0" : "1";
						length = 2;
					}
					else
					{
						ret = $"!{op1}";
						length++;
					}
					break;

				case '&':
				case '|':
					length = 0;
					(op1, l1) = Simplify(expr[1..]);
					(op2, l2) = Simplify(expr[(1 + l1)..]);
					op1n = op1[0] == '!';
					op2n = op2[0] == '!';

					//Self negatives
					if (op1 == $"!{op2}" || op2 == $"!{op1}")
						ret = expr[0] == '&' ? "0" : "1";

					//With constant 1
					else if (op1 == "1" || op2 == "1")
						ret = expr[0] == '&' ? (op1 == "1" ? op2 : op1) : "1";

					//With constant 0
					else if (op1 == "0" || op2 == "0")
						ret = expr[0] == '|' ? (op1 == "1" ? op2 : op1) : "0";

					else if (op1 == (expr[0] == '&' ? "!" : "0"))
						ret = op2;

					else if (op2 == (expr[0] == '&' ? "!" : "0"))
						ret = op1;

					//Self repetition
					else if (op1 == op2)
						ret = op1;

					//DeMorgans reduction
					else if (op1n || op2n)
					{
						ret = $"!{(expr[0] == '&' ? "|" : "&")}{(op1n ? "" : "!")}{op1[(op1n ? 1 : 0)..]}{(op2n ? "" : "!")}{op2[(op2n ? 1 : 0)..]}";
						if (expr.Length < ret.Length - (context == '!' ? 1 : 0))
						{
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

					if (op1 == "1" || op2 == "1")
						ret = "0";

					else if (op1 == "0")
						ret = op2;
					else if (op2 == "0")
						ret = op1;

					else if (op1 == op2)
						ret = "0";

					else ret = $"|{op1}{op2}";
					length = 1 + l1 + l2;
					break;

				default:
					if (Re_Constant.IsMatch(expr[0].ToString()) || Re_Identifier.IsMatch(expr[0].ToString())) return (expr[0].ToString(), 1);
					else throw new Exception("Malformed Boolean Expression");
			}

			return (ret.Replace("!!", ""), length);
		}
	}
}
