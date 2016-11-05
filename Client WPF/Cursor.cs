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
    class Cursor : GameObject {

        TextBlock textDisplay;
        Polygon triangle;


        public Cursor( string text, Point position, Point size, Color color ) : base( position, size, color ) {
            textDisplay = new TextBlock();
            textDisplay.Text = text;
            textDisplay.Foreground = ColorBrush;

            triangle = new Polygon();
            triangle.Points.Add( new Point( 0, 0 ) );
            triangle.Points.Add( new Point( size.X, 0 ) );
            triangle.Points.Add( new Point( 0, size.Y ) );
            triangle.Fill = ColorBrush;
            triangle.Stroke = ColorBrush;
            triangle.StrokeThickness = 2;
        }

        public override void Render( Canvas canvas ) {

            Canvas.SetLeft( textDisplay, Position.X + Size.X *0.5);
            Canvas.SetTop( textDisplay, Position.Y + Size.Y * 0.5);

            Canvas.SetLeft( triangle, Position.X );
            Canvas.SetTop( triangle, Position.Y );

            canvas.Children.Add( textDisplay );
            canvas.Children.Add( triangle );

        }
    }
}
