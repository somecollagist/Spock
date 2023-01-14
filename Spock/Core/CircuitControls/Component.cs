using Spock.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spock.Core.CircuitControls
{
	public abstract class Component : UserControl
	{
		public Component()
		{
			GUID = Guid.NewGuid();
		}

		public List<Component> Inputs = new();
		public Func<bool> Fn = () => false;     // Might be an idea to set this to be null to ensure functions do things?

		public Guid GUID;

		protected static Path DrawCircuitPathBetween(UIElement src, UIElement dst, Visual ancestor)
		{
			GeneralTransform srct = src.TransformToAncestor(ancestor);
			GeneralTransform dstt = dst.TransformToAncestor(ancestor);

			return new Path
			{
				Data = new CombinedGeometry
				{
					Geometry1 = new CombinedGeometry
					{
						Geometry1 = new LineGeometry
						{
							StartPoint = srct.Transform(new(0,0)),
							EndPoint = srct.Transform(new(-15,0))
						},
						Geometry2 = new LineGeometry
						{
							StartPoint = srct.Transform(new(-15,0)),
							EndPoint = dstt.Transform(new(15, 0))
						}
					},
					Geometry2 = new LineGeometry
					{
						StartPoint = dstt.Transform(new(15,0)),
						EndPoint = dstt.Transform(new(0,0))
					}
				},
			};
		}
	}
}
