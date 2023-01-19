using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
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
	public partial class Style
	{
		/// <summary>
		/// Triggered when the minimise button is clicked.
		/// </summary>
		public void MinimiseClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentWindow.WindowState = WindowState.Minimized;
		}

		/// <summary>
		/// Triggered when the close button is clicked.
		/// </summary>
		public void CloseClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentWindow.Close();
		}
	}

	//public class StyleConstants
	//{
	//	public int CaptionHeight { get; init; }
	//}

	public partial class App : Application
	{
		/// <summary>
		/// The current style in use by the app.
		/// </summary>
		public Styles CurrentStyle { get; private set; }

		/// <summary>
		/// Themes that can be used by the app.
		/// </summary>
		public enum Styles
		{
			System,
			Light,
			Dark
		}

		/// <summary>
		/// Accesses a system library to determine if Windows is using light or dark mode.
		/// </summary>
		[DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
		private static extern bool ShouldSystemUseDarkMode();

		/// <summary>
		/// Sets the current app style.
		/// </summary>
		/// <param name="style"></param>
		public void SetStyle(Styles style)
		{
			// Reset style as static variable
			CurrentStyle = style;
			// Unload current style dictionary
			Resources.MergedDictionaries.RemoveAt(1);

			// How I wish we had macros in C#...
			switch (style)
			{
				case Styles.System:
					// Load either light or dark mode depending on system default.
					Resources.MergedDictionaries.Add(new ResourceDictionary()
					{
						Source = new Uri(ShouldSystemUseDarkMode() ?
						"pack://application:,,,/Styles/Dark.xaml" :
						"pack://application:,,,/Styles/Light.xaml")
					});
					break;

				case Styles.Light:
					Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Styles/Light.xaml") });
					break;

				case Styles.Dark:
					Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Styles/Dark.xaml") });
					break;
			}
		}
	}
}
