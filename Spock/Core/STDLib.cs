using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spock
{
	internal static class STDLib
	{
		/// <summary>
		/// A logical BUF gate.
		/// </summary>
		internal class BUF : Component
		{
			public BUF() : base(1, 1)
			{
				Function = new Action(delegate ()
				{
					_outputs[0] = _inputs[0];
				});
			}
		}

		/// <summary>
		/// A logical NOT gate.
		/// </summary>
		internal class NOT : Component
		{
			public NOT() : base(1, 1)
			{
				Function = new Action(delegate ()
				{
					_outputs[0] = !_inputs[0];
				});
			}
		}

		/// <summary>
		/// A logical AND gate.
		/// </summary>
		internal class AND : Component
		{
			public AND() : base(2, 1)
			{
				Function = new Action(delegate ()
				{
					_outputs[0] = _inputs[0] && _inputs[1];
				});
			}
		}

		/// <summary>
		/// A logical OR gate.
		/// </summary>
		internal class OR : Component
		{
			public OR() : base(2, 1)
			{
				Function = new Action(delegate ()
				{
					_outputs[0] = _inputs[0] || _inputs[1];
				});
			}
		}

		/// <summary>
		/// A logical XOR gate.
		/// </summary>
		internal class XOR : Component
		{
			public XOR() : base(2, 1)
			{
				Function = new Action(delegate ()
				{
					_outputs[0] = _inputs[0] ^ _inputs[1];
				});
			}
		}

		/// <summary>
		/// A switch, which may be on or off. This is a constant output gate.
		/// </summary>
		internal class Switch : Component
		{
			public bool State { get; init; }

			public Switch(bool state) : base(0, 1)
			{
				State = state;
				Function = new Action(delegate ()
				{
					_outputs[0] = State;
				});
			}
		}

		/// <summary>
		/// A bulb, which only has an output.
		/// </summary>
		internal class Bulb : Component
		{
			public bool State { get; internal set; }
			
			public Bulb() : base(1, 0)
			{
				Function = new Action(delegate ()
				{
					State = _inputs[0];
				});
			}
		}
	}
}
