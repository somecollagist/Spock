using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spock
{
	internal static class Circuit
	{
		public static Dictionary<Guid, Component> Components { get; private set; } = new();
		public static Dictionary<Guid, Connection> Connections { get; private set; } = new();

		public static void RegisterComponent(Component component)
		{
			if(!Components.ContainsKey(component.ID)) Components.Add(component.ID, component);
		}

		public static void RegisterConnection(Connection connection)
		{
			if (!Connections.ContainsKey(connection.ID))
			{
				Connections.Add(connection.ID, connection);
				Components[connection.Source]._outputConnections[connection.SourceSink] = connection;
				Components[connection.Target]._inputConnections[connection.TargetSink] = connection;
			}
		}

		//public static void Run()
		//{
		//	List<Guid> next_nodes =
		//		Components.Values
		//		.Where(t => t.IsInputGate)
		//		.Select(t => t.ID).ToList();

		//	while(next_nodes.Count > 0)
		//	{
		//		List<Guid> run_nodes = next_nodes;
		//		next_nodes = new();

		//		foreach(Guid g in run_nodes)
		//		{
		//			Component c = Components[g];
		//			c.Function();
		//			foreach(Connection x in c._outputConnections)
		//			{
		//				if (!next_nodes.Contains(x.Target)) next_nodes.Add(x.Target);
		//				x.Transmit();
		//			}
		//		}
		//	}

		//	Console.WriteLine("Done!");
		//}

		public static void Run()
		{
			List<Guid> end_nodes =
				Components.Values
				.Where(t => t is STDLib.Bulb)
				.Select(t => t.ID)
				.ToList();

			List<List<Guid>> FlatTrees = new();
			for(int x = 0; x < end_nodes.Count; x++)
			{
				FlatTrees.Add(new());
				FlatTrees[x].AddRange(GetChildrenAsFlatTree(Components[end_nodes[x]]));
			}

			FlatTrees = FlatTrees
				.Select(t => t.Reverse<Guid>().ToList())
				.OrderBy(t => t.Count)
				.ToList();
		}

		private static List<Guid> GetChildrenAsFlatTree(Component c)
		{
			if (c._inputConnections.Length == 0) return new() { c.ID };
			else
			{
				List<Guid> ret = new();
				ret.Add(c.ID);
				foreach(Connection x in c._inputConnections)
				{
					ret.AddRange(GetChildrenAsFlatTree(Components[x.Source]));
				}
				return ret;
			}
		}
	}
}
