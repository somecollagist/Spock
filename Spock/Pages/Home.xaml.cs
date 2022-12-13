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

namespace Spock.Pages
{
	/// <summary>
	/// Interaction logic for Home.xaml
	/// </summary>
	public partial class Home : Page
	{
		public Home()
		{
			InitializeComponent();
		}

		public void NewSimulationClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Simulation.xaml", UriKind.Relative));
		}

		public void LoadSimulationClicked(object sender, RoutedEventArgs e)
		{

		}

		public void SettingsClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Settings.xaml", UriKind.Relative));
		}
	}
}
