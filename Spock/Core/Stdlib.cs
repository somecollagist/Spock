using System;
using System.Collections.Generic;

namespace Spock.Core
{
	/// <summary>
	/// A class describing an element of a circuit.
	/// </summary>
	public abstract class Component
	{
		public Component() { }

		/// <summary>
		/// List of inputs to the component.
		/// </summary>
		public List<Component> Inputs = new();
		/// <summary>
		/// The function to be run by the component.
		/// </summary>
		public Func<bool> Fn = () => false;

		/// <summary>
		/// The name of the component to be represented.
		/// </summary>
		public string ComponentContent = "";
	}

	/// <summary>
	/// Component representing a BUF gate.
	/// </summary>
	public class BUF : Component
	{
		public BUF()
		{
			Fn = () => Inputs[0].Fn();
			ComponentContent = "BUF";
		}
	}

	/// <summary>
	/// Component representing a NOT gate.
	/// </summary>
	public class NOT : Component
	{
		public NOT()
		{
			Fn = () => !Inputs[0].Fn();
			ComponentContent = "!";
		}
	}

	/// <summary>
	/// Component representing an AND gate.
	/// </summary>
	public class AND : Component
	{
		public AND()
		{
			Fn = () => Inputs[0].Fn() && Inputs[1].Fn();
			ComponentContent = "&";
		}
	}

	/// <summary>
	/// Component representing an OR gate.
	/// </summary>
	public class OR : Component
	{
		public OR()
		{
			Fn = () => Inputs[0].Fn() || Inputs[1].Fn();
			ComponentContent = "|";
		}
	}

	/// <summary>
	/// Component representing an XOR gate.
	/// </summary>
	public class XOR : Component
	{
		public XOR()
		{
			Fn = () => Inputs[0].Fn() ^ Inputs[1].Fn();
			ComponentContent = "^";
		}
	}

	/// <summary>
	/// Component used for yielding output.
	/// </summary>
	public class Lamp : BUF
	{
		public Lamp() : base()
		{
			ComponentContent = "";
		}

		public string GetState() => Fn() ? "1" : "0";
	}

	/// <summary>
	/// Component used for giving inputs to the circuit.
	/// </summary>
	public class Input : Component
	{
		public Input()
		{
			Fn = () => State;
		}

		public bool State { get; set; } = false;
	}

	/// <summary>
	/// Component representing a constant zero.
	/// </summary>
	public class ConstantZero : Component
	{
		public ConstantZero()
		{
			Fn = () => false;
			ComponentContent = "0";
		}
	}

	/// <summary>
	/// Component representing a constant one.
	/// </summary>
	public class ConstantOne : Component
	{
		public ConstantOne()
		{
			Fn = () => true;
			ComponentContent = "1";
		}
	}
}
