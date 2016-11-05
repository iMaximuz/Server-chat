using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client_WPF {
    class Circle : GameObject {
        Ellipse circle;

        public Circle( Point position, Point size, int thickness, Color color ) : base( position, size, color ) {
            circle = new Ellipse();
            circle.Width = Size.X;
            circle.Height = Size.Y;
            circle.Stroke = ColorBrush;
            circle.StrokeThickness = thickness;
        }

        override public void Render( Canvas canvas ) {
            Canvas.SetLeft( circle, Position.X );
            Canvas.SetTop( circle, Position.Y );
            canvas.Children.Add( circle );
        }
    }
}
