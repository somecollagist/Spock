using Spock.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spock.Core.CircuitControls
{
	public abstract class Component : UserControl
	{
		public Component()
		{
			GUID = Guid.NewGuid();
		}

		public List<Component> Inputs = new();
		public Func<bool> Fn = () => false;     // Might be an idea to set this to be null to ensure functions do things?

		public Guid GUID;

		public string GateContent;

		public event PropertyChangedEventHandler PropertyChanged;

		private void onChange(string propertyName)
		{
			if(PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
	}
}
