using Spock.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
		public Simulation()
		{
			InitializeComponent();
		}

		internal Stream ActiveFile { get; private set; } = Stream.Null;

		public void LoadActiveFile(string FileName)
		{
			ActiveFile = File.OpenRead(FileName);
		}

		private void Simplify_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SimpExpr.Text = Solver.Simplify(UserExpr.Text).Item1;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SettingsClicked(object sender, ExecutedRoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Settings.xaml", UriKind.Relative));
		}

		private void ClearClicked(object sender, ExecutedRoutedEventArgs e)
		{
			TruthTable.Content = null;
			UserExpr.Text = null;
			SimpExpr.Text = null;
			PolishExpression.Content = null;
			HumanExpression.Content = null;
			Result.Content = null;
			Switches.Children.Clear();
		}

		private void LoadClicked(object sender, ExecutedRoutedEventArgs e)
		{
			string expr = UserExpr.Text;

			GenerateTable(expr);
			GenerateSwitchboard(expr);
		}

		private Lamp Circuit;

		private void SwitchClicked(object sender, RoutedEventArgs e)
		{
			if (Circuit == null) return;
			foreach(Input i in Solver.InputBinds[(char)((sender as ToggleButton).Content)])
			{
				i.State = (bool)(sender as ToggleButton).IsChecked;
			}
			Result.Content = Circuit.GetState();
		}

		private void GenerateSwitchboard(string expr)
		{
			Circuit = new Lamp()
			{ 
				Inputs = new List<Component>()
				{
					Solver.GenerateCircuit(expr)
				}
			};
			PolishExpression.Content = expr;
			HumanExpression.Content = Solver.GenerateSubExpr(Circuit);
			Switches.Children.Clear();
			
			foreach(KeyValuePair<char, List<Input>> kvp in Solver.InputBinds)
			{
				foreach (Input i in kvp.Value) i.State = false;

				ToggleButton btn = new ToggleButton();
				btn.Content = kvp.Key;
				btn.Click += SwitchClicked;
				Switches.Children.Add(btn);
			}

			Result.Content = Circuit.GetState();
		}

		private struct TableLine
		{
			public TableLine(string expr)
			{
				Inputs = new();
				Output = expr;
			}

			public List<char> Inputs;
			public string Output;
		}

		private static void PopulateTableRow(ref TableLine[] table, int reps, ref int row, char id, char c)
		{
			for (int y = 0; y < reps; y++)
			{
				int r = row++;
				table[r].Inputs.Add(c);
				table[r].Output = table[r].Output.Replace(id, c);
			}
		}

		private void GenerateTable(string expr)
		{
			TruthTable.Content = null;
			try
			{
				string simpexpr = Solver.Simplify(expr).Item1;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			List<char> inputs = new();
			foreach (char c in expr)
			{
				if (Solver.Re_Identifier.IsMatch(c.ToString()) && !inputs.Contains(c)) inputs.Add(c);
			}

			TableLine[] table = new TableLine[(int)Math.Pow(2, inputs.Count) + 1];
			for (int x = 0; x < table.Length; x++) table[x] = new(expr);
			for (int x = 0; x < inputs.Count; x++)
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

			table[0].Output = expr;
			for (int x = 1; x < table.GetLength(0); x++)
			{
				table[x].Output = Solver.Simplify(table[x].Output).Item1;
			}

			StackPanel tableV = new();
			foreach(TableLine line in table)
			{
				StackPanel ln = new() { Orientation = Orientation.Horizontal };
				foreach (char c in line.Inputs)
				{
					ln.Children.Add(new Label() { Content = c, Foreground = (Brush)FindResource("ForegroundDim") });
				}
				ln.Children.Add(new Label() { Content = line.Output });
				tableV.Children.Add(ln);
			}

			TruthTable.Content = tableV;
		}
	}
}
