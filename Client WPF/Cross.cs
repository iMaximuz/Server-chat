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
    public class Cross : GameObject {
        Line line1;
        Line line2;

        public Cross( Point position, Point size, int thickness, Color color ) : base( position, size, color ) {
            line1 = new Line();
            line2 = new Line();

            line1.X1 = 0;
            line1.Y1 = 0;
            line1.X2 = Size.X;
            line1.Y2 = Size.Y;
            line1.StrokeThickness = thickness;
            line1.Stroke = ColorBrush;

            line2.X1 = 0;
            line2.Y1 = Size.Y;
            line2.X2 = Size.X;
            line2.Y2 = 0;
            line2.StrokeThickness = thickness;
            line2.Stroke = ColorBrush;
        }

        override public void Render( Canvas canvas ) {
            Canvas.SetLeft( line1, Position.X );
            Canvas.SetTop( line1, Position.Y );
            Canvas.SetLeft( line2, Position.X );
            Canvas.SetTop( line2, Position.Y );

            canvas.Children.Add( line1 );
            canvas.Children.Add( line2 );
        }

    }
}
