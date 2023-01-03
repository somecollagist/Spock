using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spock.Core.CircuitControls
{
	public abstract class Component : UserControl
	{
		public List<Component> Inputs = new();
		public Func<bool> Fn = () => false;		// Might be an idea to set this to be null to ensure functions do things?
	}
}
