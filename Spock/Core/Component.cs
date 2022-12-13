using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spock
{
	abstract internal class Component : UserControl
	{
		/// <summary>
		/// The function performed by the gate.
		/// </summary>
		public virtual Action Function { get; init; } = new Action(delegate () { });
		/// <summary>
		/// The gate's unique identifier.
		/// </summary>
		public Guid ID { get; init; } = Guid.NewGuid();

		/// <summary>
		/// The inputs of the logic gate.
		/// </summary>
		protected bool[] _inputs;
		/// <summary>
		/// The outputs of the logic gate.
		/// </summary>
		protected bool[] _outputs;

		public Connection[] _inputConnections;
		public Connection[] _outputConnections;

		/// <summary>
		/// Sets the value of a certain input.
		/// </summary>
		/// <param name="sink">The input to set.</param>
		/// <param name="state">The value to set.</param>
		public void SetInputSink(int sink, bool state)
		{
			_inputs[sink] = state;
		}
		
		/// <summary>
		/// Gets the value of a certain output
		/// </summary>
		/// <param name="sink">The output to get.</param>
		/// <returns>The value of the given output.</returns>
		public bool GetOutputSink(int sink)
		{
			return _outputs[sink];
		}

		/// <summary>
		/// Base Constructor for a logic gate.
		/// </summary>
		/// <param name="Inputs">The number of inputs into the gate.</param>
		/// <param name="Outputs">The number of outputs from the gate.</param>
		public Component(int Inputs, int Outputs)
		{
			_inputs = new bool[Inputs];
			_outputs = new bool[Outputs];
			_inputConnections = new Connection[Inputs];
			_outputConnections = new Connection[Outputs];
			Circuit.RegisterComponent(this);
		}
	}
}
