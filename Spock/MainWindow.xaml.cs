using System;
using System.Windows;

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
