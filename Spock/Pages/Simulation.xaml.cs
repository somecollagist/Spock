using Spock.Core;
using Spock.Core.CircuitControls;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Web.AtomPub;

namespace Spock.Pages
{
	/// <summary>
	/// Interaction logic for Simulation.xaml
	/// </summary>
	public partial class Simulation : Page
	{
		internal static Simulation CurrentSim;

		public Simulation()
		{
			InitializeComponent();
			CurrentSim = this;
		}

		internal Stream ActiveFile { get; private set; } = Stream.Null;

		public void LoadActiveFile(string FileName)
		{
			ActiveFile = File.OpenRead(FileName);
		}

		private void Simplify_Click(object sender, RoutedEventArgs e)
		{
			SimpExpr.Content = Solver.Simplify(UserExpr.Text);
		}

		private void ReturnToHomeClicked(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Home.xaml", UriKind.Relative));
		}

		private void NewSimulationClicked(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private void OpenSimulationClicked(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private void SaveSimulationClicked(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private struct TableLine
		{
			public TableLine(string expr)
			{
				Inputs = new();
				Output = '\0';
				Expr = expr;
			}

			public List<char> Inputs;
			public char Output;
			public string Expr;
		}

		private void PopulateTableRow(ref TableLine[] table, int reps, ref int row, char id, char c)
		{
			for (int y = 0; y < reps; y++)
			{
				int r = row++;
				table[r].Inputs.Add(c);
				table[r].Expr = table[r].Expr.Replace(id, c);
			}
		}

		private void GenerateDiagramTableClicked(object sender, ExecutedRoutedEventArgs e)
		{
			string expr = UserExpr.Text;

			//Generate Diagram

			//Generate Table
			List<char> inputs = new();
			foreach(char c in expr)
			{
				if (Solver.Re_Identifier.IsMatch(c.ToString())) inputs.Add(c);
			}

			TableLine[] table = new TableLine[(int)Math.Pow(2, inputs.Count) + 1];
			for (int x = 0; x < table.Length; x++) table[x] = new(expr);
			for(int x = 0; x < inputs.Count; x++)
			{
				table[0].Inputs.Add(inputs[x]);
				int reps = (int)Math.Pow(2, inputs.Count - x - 1);
				int row = 1;
				while (row < table.GetLength(0))
				{
					PopulateTableRow(ref table, reps, ref row, inputs[x], '0');
					PopulateTableRow(ref table, reps, ref row, inputs[x], '1');
				}
			}

			table[0].Output = '>';
			for(int x = 1; x < table.GetLength(0); x++)
			{
				table[x].Output = Solver.Simplify(table[x].Expr)[0]; //Should just be one character
			}
		}
	}
}
