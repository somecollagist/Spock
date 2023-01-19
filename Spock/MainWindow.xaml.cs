using System;
using System.Collections.Generic;
using System.IO;
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

namespace Spock
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
			{
				DefaultValue = FindResource(typeof(Window))
			});

			App.CurrentApp.SetStyle(
				(App.Styles)Properties.Settings.Default.Theme
			);
		}
	}

	public partial class App : Application
	{
		/// <summary>
		/// Switches the current page shown to the user.
		/// </summary>
		/// <param name="uri">The URI of the page to be shown.</param>
		#pragma warning disable CA1822	// If we switch it to static then Simulation.xaml.cs and Settings.xaml.cs throw a hissy fit
		public void SwitchPage(Uri uri)
		{
			CurrentWindow.Frame.Source = uri;
		}
		#pragma warning restore CA1822
	}
}
