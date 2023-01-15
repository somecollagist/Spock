using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spock.Core
{
	public abstract class Component
	{
		public Component()
		{
			GUID = Guid.NewGuid();
		}

		public List<Component> Inputs = new();
		public Func<bool> Fn = () => false;

		public Guid GUID;

		public string ComponentContent = "";
	}

	public class BUF : Component
	{
		public BUF()
		{
			Fn = () => Inputs[0].Fn();
			ComponentContent = "BUF";
		}
	}

	public class NOT : Component
	{
		public NOT()
		{
			Fn = () => !Inputs[0].Fn();
			ComponentContent = "!";
		}
	}

	public class AND : Component
	{
		public AND()
		{
			Fn = () => Inputs[0].Fn() && Inputs[1].Fn();
			ComponentContent = "&";
		}
	}

	public class OR : Component
	{
		public OR()
		{
			Fn = () => Inputs[0].Fn() || Inputs[1].Fn();
			ComponentContent = "|";
		}
	}

	public class XOR : Component
	{
		public XOR()
		{
			Fn = () => Inputs[0].Fn() ^ Inputs[1].Fn();
			ComponentContent = "^";
		}
	}

	public class Lamp : BUF
	{
		public Lamp() : base()
		{
			ComponentContent = "";
		}

		public string GetState() => Fn() ? "1" : "0";
	}

	public class Input : Component
	{
		public Input()
		{
			Fn = () => State;
		}

		public bool State { get; set; } = false;
	}

	public class ConstantZero : Component
	{
		public ConstantZero()
		{
			Fn = () => false;
			ComponentContent = "0";
		}
	}

	public class ConstantOne : Component
	{
		public ConstantOne()
		{
			Fn = () => true;
			ComponentContent = "1";
		}
	}
}
