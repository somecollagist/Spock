﻿using System;
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
		#pragma warning disable CA2211
		public static MainWindow CurrentWindow = (MainWindow)Current.MainWindow;
		public static App CurrentApp = (App)Current;
		#pragma warning restore CA2211

	}
}
