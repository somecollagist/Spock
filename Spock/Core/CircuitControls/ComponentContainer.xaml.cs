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
	/// Interaction logic for ComponentContainer.xaml
	/// </summary>
	public partial class ComponentContainer : UserControl
	{
		public ComponentContainer()
		{
			InitializeComponent();
		}

		private void ComponentPicked(object sender, SelectionChangedEventArgs e)
		{
			ComponentPicker.Visibility = Visibility.Collapsed;
			ComponentSpace.Visibility = Visibility.Visible;

#pragma warning disable CS8509
#pragma warning disable CS8602
			ComponentHolder.Children.Add(
				(e.AddedItems[0] as ComboBoxItem).Content switch
				{
					"NOT" => new NOT(),
					"AND" => new AND(),
					"OR" => new OR(),
					"XOR" => new XOR()
				});
#pragma warning restore CS8509
#pragma warning restore CS8602
		}

		private void DeleteComponent(object sender, RoutedEventArgs e)
		{
			ComponentPicker.Visibility = Visibility.Visible;
			ComponentSpace.Visibility = Visibility.Collapsed;

			ComponentHolder.Children.Clear();
		}
	}
}
