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
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page
	{
		public Settings()
		{
			InitializeComponent();

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

		public void ReturnClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Simulation.xaml", UriKind.Relative));
			Properties.Settings.Default.Save();
		}

		public void ThemeClicked(object sender, RoutedEventArgs e)
		{
			App.Styles style;
			switch((string)(((RadioButton)sender).Content))
			{
				case "Light":
					style = App.Styles.Light;
					break;

				case "Dark":
					style = App.Styles.Dark;
					break;

				default:
					style = App.Styles.System;
					break;
			}
			App.CurrentApp.SetStyle(style);
			Properties.Settings.Default.Theme = (short)style;
		}
	}
}
