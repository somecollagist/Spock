using System;
using System.Collections.Generic;
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
		public void MinimiseClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentWindow.WindowState = WindowState.Minimized;
		}

		public void MaximiseClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentWindow.WindowState = App.CurrentWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
		}

		public void CloseClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentWindow.Close();
		}
	}

	public class StyleConstants
	{
		public int CaptionHeight { get; init; }
	}

	public partial class App : Application
	{
		public Styles CurrentStyle { get; private set; }

		public enum Styles
		{
			System,
			Light,
			Dark
		}

		[DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
		private static extern bool ShouldSystemUseDarkMode();

		private ResourceDictionary Theme
		{
			get { return Resources.MergedDictionaries[0]; }
		}

		public void SetStyle(Styles style)
		{
			CurrentStyle = style;
			Theme.MergedDictionaries.Clear();
			switch (style)
			{
				case Styles.System:
					Theme.MergedDictionaries.Add(new ResourceDictionary()
					{
						Source = new Uri(ShouldSystemUseDarkMode() ?
						"pack://application:,,,/Styles/Dark.xaml" :
						"pack://application:,,,/Styles/Light.xaml")
					});
					break;

				case Styles.Light:
					Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Styles/Light.xaml") });
					break;

				case Styles.Dark:
					Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Styles/Dark.xaml") });
					break;
			}
		}
	}
}
