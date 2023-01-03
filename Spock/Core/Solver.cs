using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spock.Core
{
	internal class Solver
	{
		public static Token Solve(string expr)
		{
			Token Root = new(expr);
			return Root.Parse();
		}
	}

	internal class Token
	{
		public List<Token> Tokens;
		public string Content;

		public Token(string content)
		{
			Content = content;
			Tokens = new();
		}

		internal Token Parse() => Parse(Content);

		internal Token Parse(string expr)
		{
			List<Token> tokens = new List<Token>();
			for(int tracker = 0; tracker < expr.Length; tracker++)
			{
				char c = expr[tracker];
				if(c == '(')
				{
					string subexpr = expr.Substring(tracker+1, expr.LastIndexOf(')') - tracker-1);
					tokens.Add(Parse(subexpr));
					tracker += subexpr.Length+1;
				}
				else
				{
					tokens.Add(new(c.ToString()));
				}
			}
			Token tk = new(expr);
			tk.Tokens = tokens;
			return tk;
		}
	}
}
