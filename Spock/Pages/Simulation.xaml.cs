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
		public static Simulation CurrentSim;

		public Simulation()
		{
			InitializeComponent();
			CurrentSim = this;

			foreach(UserControl c in new UserControl[] {
				new Core.CircuitControls.BUF(),
				new Core.CircuitControls.NOT(),
				new Core.CircuitControls.AND(),
				new Core.CircuitControls.OR(),
				new Core.CircuitControls.XOR(),
				new Core.CircuitControls.Switch()
			})
			{
				Draglist.Items.Add(c);
			}
		}

		internal Stream ActiveFile { get; private set; } = Stream.Null;

		public void LoadActiveFile(string FileName)
		{
			ActiveFile = File.OpenRead(FileName);
		}

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603

		Point StartPoint;
		private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			StartPoint = e.GetPosition(null);
		}

		private void List_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				Point mousePos = e.GetPosition(null);
				Vector diff = StartPoint - mousePos;

				if (e.LeftButton == MouseButtonState.Pressed &&
					(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
					Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
					{
						ListView listView = sender as ListView;
						ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
						UserControl component = (UserControl)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
						DataObject dragData = new DataObject("myformat", component);
						DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
					}
			}
			catch (ArgumentNullException)
			{
				// Likely caused by an invalid drag - not enough to crash over
			}
		}

		private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
		{
			do
			{
				if (current is T || current.GetType().IsSubclassOf(typeof(T)))
				{
					return (T)current;
				}
				current = VisualTreeHelper.GetParent(current);
			} while (current != null);
			return null;
		}

		private void Workspace_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("myformat"))
			{
				UserControl component = e.Data.GetData("myformat") as UserControl;
				Canvas canvas = sender as Canvas;
				UserControl ctrl = component.GetType().Name switch
				{
					"BUF"		=> new Spock.Core.CircuitControls.BUF(),
					"NOT"		=> new Spock.Core.CircuitControls.NOT(),
					"AND"		=> new Spock.Core.CircuitControls.AND(),
					"OR"		=> new Spock.Core.CircuitControls.OR(),
					"XOR"		=> new Spock.Core.CircuitControls.XOR(),
					"Switch"	=> new Spock.Core.CircuitControls.Switch(),
					_			=> throw new Exception("?!")
				};
				ctrl.Margin = new Thickness(e.GetPosition(Workspace).X, e.GetPosition(Workspace).Y, 0, 0);
				canvas.Children.Add(ctrl);
			}
		}

		private void Workspace_DragEnter(object sender, DragEventArgs e)
		{
			if(!e.Data.GetDataPresent("myformat") || sender == e.Source)
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void Workspace_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Source is Component component)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					Point p = e.GetPosition(Workspace);
					Canvas.SetLeft(component, p.X - component.ActualWidth / 2);
					Canvas.SetTop(component, p.Y - component.ActualHeight / 2);
					component.CaptureMouse();
				}
				else component.ReleaseMouseCapture();
			}
		}

		private static Component? srcComponent;
		internal static Component ConnectionComponent
		{
			private get => srcComponent;
			set
			{
				if (srcComponent is null) srcComponent = value;
				else
				{
					value.Inputs.Add(srcComponent);

					Line line = new();
					Point srcPoint = srcComponent.TransformToAncestor(App.CurrentWindow).Transform(new(0, 0));
					Point tarPoint = value.TransformToAncestor(App.CurrentWindow).Transform(new(0, 0));

					line.X1 = srcPoint.X - 60;
					line.Y1 = srcPoint.Y - 90;
					line.X2 = tarPoint.X - 60;
					line.Y2 = tarPoint.Y - 90;

					line.Visibility = Visibility.Visible;
					line.Stroke = Brushes.Green;
					line.StrokeThickness = 4;

					CurrentSim.Workspace.Children.Add(line);

					srcComponent = null;
				}
			}
		}

		private void Simplify_Click(object sender, RoutedEventArgs e)
		{
			Console.Write(">> ");
			string simple = Solver.Simplify(Console.ReadLine());
			Console.WriteLine(simple);
		}
	}
}
