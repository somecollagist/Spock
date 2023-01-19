using Spock.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

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
		private void Simplify_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SimpExpr.Text = Solver.Simplify(UserExpr.Text).Item1;
			}
			catch (Exception ex)
			{
				//If an error occurs, don't HCF
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Navigates to settings page.
		/// </summary>
		private void SettingsClicked(object sender, ExecutedRoutedEventArgs e)
		{
			App.CurrentApp.SwitchPage(new Uri("Pages/Settings.xaml", UriKind.Relative));
		}

		/// <summary>
		/// Clears the output controls of their content.
		/// </summary>
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

		/// <summary>
		/// Loads a user expression and generates a table and switchboard accordingly.
		/// </summary>
		private void LoadClicked(object sender, ExecutedRoutedEventArgs e)
		{
			string expr = UserExpr.Text;
			GenerateTable(expr);
			GenerateSwitchboard(expr);
		}

		/// <summary>
		/// Stores the currently used circuit.
		/// </summary>
		private Lamp Circuit = new();

		/// <summary>
		/// Handler for when a switch in the dynamic switchboard is clicked.
		/// </summary>
		private void SwitchClicked(object sender, RoutedEventArgs e)
		{
			if (Circuit == null) return;	// Shouldn't fire anyhow but defensive programming is good
			#pragma warning disable CS8602
			foreach(Input i in Solver.InputBinds[(char)(sender as ToggleButton).Content]) 	// Flip all given inputs to a state
			{
				#pragma warning disable CS8629
				i.State = (bool)(sender as ToggleButton).IsChecked;
				#pragma warning restore CS8629
			}
			Result.Content = Circuit.GetState();
			#pragma warning restore CS8602
		}

		private void GenerateSwitchboard(string expr)
		{
			// Clear the list of input binds (cheers Harry)
			Solver.InputBinds.Clear();
			// Figure out what the circuit is and display expressions
			Circuit = new Lamp()
			{ 
				Inputs = new List<Component>()
				{
					Solver.GenerateCircuit(expr)
				}
			};
			PolishExpression.Content = expr;
			HumanExpression.Content = Solver.GenerateInfixExpr(Circuit);
			
			// Remove all switches and rebuild them
			Switches.Children.Clear();
			foreach(KeyValuePair<char, List<Input>> kvp in Solver.InputBinds)
			{
				#pragma warning disable IDE0017
				ToggleButton btn = new();
				btn.Content = kvp.Key;					// To be displayed on the button
				btn.Click += SwitchClicked;             // Attach to function (listed above)
				#pragma warning restore IDE0017
				Switches.Children.Add(btn);				// Add to collection
			}

			Result.Content = Circuit.GetState();
		}

		/// <summary>
		/// Stores information about a line in a truth table.
		/// </summary>
		private struct TableLine
		{
			/// <summary>
			/// Creates a new TableLine struct.
			/// </summary>
			/// <param name="expr">The expression of the circuit to generate a TableLine for.</param>
			public TableLine(string expr)
			{
				Inputs = new();
				Output = expr;
			}

			/// <summary>
			/// List of inputs the circuit has.
			/// </summary>
			public List<char> Inputs;
			/// <summary>
			/// The output of the circuit for a particular state.
			/// </summary>
			public string Output;
		}

		/// <summary>
		/// Fills in a row of the table.
		/// </summary>
		/// <param name="table">Reference to table to be populated.</param>
		/// <param name="reps">How many times this identifer appears continuously without changing state.</param>
		/// <param name="row">Row to be modified.</param>
		/// <param name="id">Current identifier.</param>
		/// <param name="c">Value of current identifier.</param>
		private static void PopulateTableRow(ref TableLine[] table, int reps, ref int row, char id, char c)
		{
			for (int y = 0; y < reps; y++)
			{
				int r = row++;
				table[r].Inputs.Add(c);
				table[r].Output = table[r].Output.Replace(id, c);
			}
		}

		/// <summary>
		/// Generates a truth table for a given expression.
		/// </summary>
		/// <param name="expr">Expression to generate the table for.</param>
		private void GenerateTable(string expr)
		{
			// Remove content from the TruthTable
			TruthTable.Content = null;

			string simpexpr;
			try
			{
				// We need to simplify the expression at the end, so we might as well do some heavy lifting now.
				simpexpr = Solver.Simplify(expr).Item1;
			}
			catch (Exception ex)
			{
				// If it can't be simplified without error, the expression must be problematic
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Find all inputs in the circuit
			List<char> inputs = new();
			foreach (char c in expr)
			{
				if (Solver.Re_Identifier.IsMatch(c.ToString()) && !inputs.Contains(c)) inputs.Add(c);
			}

			// Each circuit has 2^n possible input states for n inputs, add 1 for the top row (key)
			TableLine[] table = new TableLine[(int)Math.Pow(2, inputs.Count) + 1];
			for (int x = 0; x < table.Length; x++) table[x] = new(simpexpr);	// Populate with expressions, use simplified to save computation time later
			for (int x = 0; x < inputs.Count; x++)
			{
				table[0].Inputs.Add(inputs[x]);									// First line gets the name of each identifier
				int reps = (int)Math.Pow(2, inputs.Count - x - 1);				// See PopulateTableRow
				int row = 1;
				while (row < table.GetLength(0))
				{
					PopulateTableRow(ref table, reps, ref row, inputs[x], '0');	// Plug in 0s
					PopulateTableRow(ref table, reps, ref row, inputs[x], '1');	// Plug in 1s (take advantage of ref side affects)
				}
			}

			table[0].Output = expr;	// Even though we've found the simplified expression, we have to show the original one
			for (int x = 1; x < table.GetLength(0); x++)
			{
				// Simplify expressions, now with only 0s and 1s (ergo can be collapsed to definite states)
				table[x].Output = Solver.Simplify(table[x].Output).Item1;
			}

			// Generate a control
			StackPanel tableV = new();
			foreach(TableLine line in table)
			{
				// StackPanels for lines of the table
				StackPanel ln = new() { Orientation = Orientation.Horizontal };
				foreach (char c in line.Inputs)
				{
					// List off the inputs
					ln.Children.Add(new Label() { Content = c, Foreground = (Brush)FindResource("ForegroundDim") });
				}
				// List of the expression/result
				ln.Children.Add(new Label() { Content = line.Output });
				tableV.Children.Add(ln);
			}

			// Set to content to display
			TruthTable.Content = tableV;
		}
	}
}
