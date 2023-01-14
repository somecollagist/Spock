using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spock.Core.CircuitControls
{
	/// <summary>
	/// Interaction logic for Lamp.xaml
	/// </summary>
	public partial class Lamp : Component
	{
		public Lamp(Visual ancestor)
		{
			InitializeComponent();
			Height = 30;
			Width = 160;
			//Container.Children.Add(DrawCircuitPathBetween(LampComponent, C1, ancestor));
		}
	}
}
