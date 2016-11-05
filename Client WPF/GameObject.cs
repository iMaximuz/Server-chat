using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client_WPF {
    public class GameObject {
        private Point position;
        private Point size;
        private SolidColorBrush colorBrush;

        public Point Position {
            get {
                return position;
            }

            set {
                position = value;
            }
        }

        protected Point Size {
            get {
                return size;
            }

            set {
                size = value;
            }
        }

        protected SolidColorBrush ColorBrush {
            get {
                return colorBrush;
            }

            set {
                colorBrush = value;
            }
        }

        public GameObject( Point position, Point size, Color color ) {
            this.position = position;
            this.size = size;
            this.colorBrush = new SolidColorBrush();
            this.colorBrush.Color = color;
        }

        public virtual void Render( Canvas canvas ) { }

    }
}
