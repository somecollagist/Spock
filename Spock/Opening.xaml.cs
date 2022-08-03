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

using static Spock.STDLib;

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

			Switch alpha = new(true);
			Switch beta = new(false);
			Switch gamma = new(true);

			OR gate1 = new();
			AND gate2 = new();

			Bulb bulb1 = new();
			Bulb bulb2 = new();

			Connection c1 = new(alpha, gate1, 0, 0);
			Connection c2 = new(beta, gate1, 0, 1);
			Connection c3 = new(gate1, gate2, 0, 0);
			Connection c4 = new(gamma, gate2, 0, 1);

			Connection b1 = new(gate1, bulb1, 0, 0);
			Connection b2 = new(gate2, bulb2, 0, 0);

			Circuit.Run();

			Console.Write(bulb1.State);
			Console.Write(bulb2.State);
		}
	}
}
