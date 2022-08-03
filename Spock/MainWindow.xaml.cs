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

			Switch alpha = new(true) { IsInputGate = true };
			Switch beta = new(true) { IsInputGate = false };
			AND gate = new();
			Bulb output = new();

			Connection alpha_and = new(alpha, gate, 0, 0);
			Connection beta_and = new(beta, gate, 0, 1);
			Connection light = new(gate, output, 0, 0);

			alpha_and.Transmit();
			beta_and.Transmit();
			light.Transmit();

			Circuit.Run();

			Console.Write(output.State);
		}
	}
}
