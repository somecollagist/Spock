using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Spock
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static MainWindow CurrentWindow = (MainWindow)Current.MainWindow;
		public static App CurrentApp = (App)Current;
	}
}
