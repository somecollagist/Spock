using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spock
{
	internal class Connection
	{
		/// <summary>
		/// The connections's unique identifier.
		/// </summary>
		public Guid ID { get; init; } = Guid.NewGuid();

		public Guid Source { get; init; }
		public Guid Target { get; init; }
		
		public int SourceSink { get; init; }
		public int TargetSink { get; init; }

		public Connection(Guid source, Guid target, int sourceSink, int targetSink)
		{
			Source = source;
			Target = target;
			SourceSink = sourceSink;
			TargetSink = targetSink;
			Circuit.RegisterConnection(this);
		}

		public Connection(Component source, Component target, int sourceSink, int targetSink)
			: this(source.ID, target.ID, sourceSink, targetSink) { }

		public void Transmit()
		{
			Circuit.Components[Target].SetInputSink(TargetSink, Circuit.Components[Source].GetOutputSink(SourceSink));
		}
	}
}
