using System;
using System.Windows;
using System.Windows.Controls;

namespace Spock.Pages
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page
	{
		public Settings()
		{
			InitializeComponent();

			// Switch theme according to whatever's loaded in the app manifest
			switch(App.CurrentApp.CurrentStyle)
			{
				case App.Styles.System:
					ThemeSystemDefault.IsChecked = true;
					break;

				case App.Styles.Light:
					ThemeLight.IsChecked = true;
					break;

				case App.Styles.Dark:
					ThemeDark.IsChecked = true;
					break;
			}
		}

		/// <summary>
		/// Return to simulation page.
		/// </summary>
		public void ReturnClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Simulation.xaml", UriKind.Relative));
			Properties.Settings.Default.Save(); // Save the user's settings to the manifest
		}

		/// <summary>
		/// Switches the user's theme
		/// </summary>
		public void ThemeClicked(object sender, RoutedEventArgs e)
		{
			Spock.App.Styles style = (string)((RadioButton)sender).Content switch
			{
				"Light" => App.Styles.Light,
				"Dark" => App.Styles.Dark,
				_ => App.Styles.System,
			};
			App.CurrentApp.SetStyle(style);
			Properties.Settings.Default.Theme = (short)style;
		}
	}
}
