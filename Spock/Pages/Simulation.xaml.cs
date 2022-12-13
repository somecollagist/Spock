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

			foreach(UserControl c in new UserControl[] {
				new Core.CircuitControls.BUF(),
				new Core.CircuitControls.NOT(),
				new Core.CircuitControls.AND(),
				new Core.CircuitControls.OR(),
				new Core.CircuitControls.XOR()
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

		Point StartPoint;
		Point EndPoint;

		private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			StartPoint = e.GetPosition(null);
		}

		private void List_MouseMove(object sender, MouseEventArgs e)
		{
			Point mousePos = e.GetPosition(null);
			Vector diff = StartPoint - mousePos;

			if(e.LeftButton == MouseButtonState.Pressed &&
				Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
				Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
			{
				ListView listView = sender as ListView;
				ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
				UserControl component = (UserControl)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
				DataObject dragData = new DataObject("myformat", component);
				DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
			}
		}

		private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
		{
			do
			{
				if (current is T)
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
				ListView listView = sender as ListView;
				Draglist.Items.Remove(component);
				listView.Items.Add(component);
			}
		}

		private void Workspace_DragEnter(object sender, DragEventArgs e)
		{
			if(!e.Data.GetDataPresent("myformat") || sender == e.Source)
			{
				e.Effects = DragDropEffects.None;
			}
		}
	}
}
