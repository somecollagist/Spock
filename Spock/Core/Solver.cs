using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spock.Core
{
	internal static class Solver
	{

		private static Regex Re_Operator = new(@"[\!\&\|\^]");
		private static Regex Re_Identifier = new(@"([A-Z])");
		private static Regex Re_Constant = new(@"[01]");

		public static string Simplify(string expr)
		{
			string ret = "";
			string op1;
			string op2;
			switch(expr[0])
			{
				case '!':
					op1 = Simplify(expr[1..]);
					if(Re_Constant.IsMatch(op1))
					{
						ret = op1 == "1" ? "0" : "1";
					}
					break;

				case '&':
					op1 = Simplify(expr[1..]);
					op2 = Simplify(expr[(1 + op1.Length)..]);

					if (op1 == "0" || op2 == "0")
						ret = "0";

					else if (op1 == "1")
						ret = op2;
					else if (op2 == "1")
						ret = op1;

					else if (op1 == op2)
						ret = op1;

					else ret = $"&{op1}{op2}";
					break;

				case '|':
					op1 = Simplify(expr[1..]);
					op2 = Simplify(expr[(1 + op1.Length)..]);

					if (op1 == "1" || op2 == "1")
						ret = "1";

					else if (op1 == "0")
						ret = op2;
					else if (op2 == "0")
						ret = op1;

					else if (op1 == op2)
						ret = op1;

					else ret = $"|{op1}{op2}";
					break;

				case '^':
					op1 = Simplify(expr[1..]);
					op2 = Simplify(expr[(1 + op1.Length)..]);

					if (op1 == "1" || op2 == "1")
						ret = "0";

					else if (op1 == "0")
						ret = op2;
					else if (op2 == "0")
						ret = op1;

					else if (op1 == op2)
						ret = "0";

					else ret = $"|{op1}{op2}";
					break;

				default:
					if (Re_Constant.IsMatch(expr[0].ToString()) || Re_Identifier.IsMatch(expr[0].ToString())) return expr[0].ToString();
					else throw new Exception("What have you done my don");
			}

			return ret;
		}
	}
}
