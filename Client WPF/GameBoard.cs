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
    public enum Player {
        X = 1,
        O
    }

    public enum GameState {
        InvalidMove,
        ValidMove,
        CrossWins,
        CircleWins,
        Draw
    }

    public class GameBoard : GameObject {
        Line[] lines;
        Line winingLine;
        List<GameObject> gameElements;

        byte[,] board;
        int moveCount = 0;

        Point CROSS_SIZE;
        Point CIRCLE_SIZE;
        Point squareSize;

        GameState gameState;

        public GameState GameState {
            get {
                return gameState;
            }
        }

        public GameBoard( Point position, Point size, int thickness, Color color ) : base( position, size, color ) {
            lines = new Line[4];
            gameElements = new List<GameObject>();
            board = new byte[3, 3];
            winingLine = new Line();

            squareSize = new Point( Size.X / 3, Size.Y / 3 );
            CROSS_SIZE = new Point( squareSize.X - 20, squareSize.Y - 20 );
            CIRCLE_SIZE = new Point( squareSize.X - 10, squareSize.Y - 10 );
            for (int i = 0; i < 4; i++) {
                lines[i] = new Line();
                if (i < 2) {
                    lines[i].X1 = 0;
                    lines[i].Y1 = 0;
                    lines[i].X2 = 0;
                    lines[i].Y2 = size.Y;
                }
                else {
                    lines[i].X1 = 0;
                    lines[i].Y1 = 0;
                    lines[i].X2 = size.X;
                    lines[i].Y2 = 0;
                }

                lines[i].Stroke = ColorBrush;
                lines[i].StrokeThickness = thickness;

            }

            SolidColorBrush winingColor = new SolidColorBrush();
            winingColor.Color = Colors.Gold;
            winingLine.Stroke = winingColor;
            winingLine.StrokeThickness = thickness;

        }
        override public void Render( Canvas canvas ) {

            for (int i = 0; i < 4; i++) {
                if (i < 2) {
                    Canvas.SetLeft( lines[i], Position.X + Size.X / 3 * (i + 1) );
                    Canvas.SetTop( lines[i], Position.Y );
                }
                else {
                    int j = i - 2;
                    Canvas.SetLeft( lines[i], Position.X );
                    Canvas.SetTop( lines[i], Position.Y + Size.Y / 3 * (j + 1) );
                }
                canvas.Children.Add( lines[i] );
            }

            foreach (GameObject obj in gameElements) {
                obj.Render( canvas );
            }

            if(gameState >= GameState.CrossWins) {
                Canvas.SetLeft( winingLine, Position.X );
                Canvas.SetTop( winingLine, Position.Y );
                canvas.Children.Add( winingLine );
            }


        }

        public GameState Move( Player player, Point screenPosition ) {
            if (gameState < GameState.CrossWins) {
                Point position = (Point)(screenPosition - this.Position);

                if (position.X < 0 || position.Y < 0)
                    return GameState.InvalidMove;

                if (position.X > this.Size.X || position.Y > this.Size.Y)
                    return GameState.InvalidMove;


                Point boardSquare = new Point( position.X / squareSize.X, position.Y / squareSize.Y );

                int X = (int)boardSquare.X;
                int Y = (int)boardSquare.Y;

                gameState = MakeMove( X, Y, player );
                if (gameState == GameState.InvalidMove)
                    return GameState.InvalidMove;

                if (player == Player.X) {
                    AddCross( boardSquare );
                }
                else {
                    AddCircle( boardSquare );
                }
            }
            return gameState;
        }

        void AddCross( Point boardSquare ) {

            int X = (int)boardSquare.X;
            int Y = (int)boardSquare.Y;

            Point crossPosition = new Point( ((X * squareSize.X) - (CROSS_SIZE.X * 0.5)) + (Position.X + squareSize.X * 0.5),
                ((Y * squareSize.Y) - (CROSS_SIZE.Y * 0.5)) + (Position.Y + squareSize.Y * 0.5) );

            gameElements.Add( new Cross( new Point( crossPosition.X, crossPosition.Y ), CROSS_SIZE, 10, Colors.IndianRed ) );
        }

        void AddCircle( Point boardSquare ) {

            int X = (int)boardSquare.X;
            int Y = (int)boardSquare.Y;

            Point circlePosition = new Point( ((X * squareSize.X) - (CIRCLE_SIZE.X * 0.5)) + (Position.X + squareSize.X * 0.5),
                ((Y * squareSize.Y) - (CIRCLE_SIZE.Y * 0.5)) + (Position.Y + squareSize.Y * 0.5) );

            gameElements.Add( new Circle( new Point( circlePosition.X, circlePosition.Y ), CIRCLE_SIZE, 10, Colors.LightBlue ) );
        }

        public GameState MakeMove( int x, int y, Player player ) {

            if (board[y, x] > 0)
                return GameState.InvalidMove;

            board[y, x] = (byte)player;
            moveCount++;
            //check cols
            for (int i = 0; i < 3; i++) {
                if (board[i, x] != (byte)player)
                    break;
                if (i == 2) {
                    DrawWiningLine( new Point( x, -1 ), new Point( x, i + 1 ) );
                    if (player == Player.X)
                        return GameState.CrossWins;
                    else
                        return GameState.CircleWins;
                }
            }

            //check rows
            for (int i = 0; i < 3; i++) {
                if (board[y, i] != (byte)player)
                    break;
                if (i == 2) {
                    DrawWiningLine( new Point( -1, y ), new Point( i + 1, y ) );
                    if (player == Player.X)
                        return GameState.CrossWins;
                    else
                        return GameState.CircleWins;
                }
            }

            //check diag
            if (x == y) {
                for (int i = 0; i < 3; i++) {
                    if (board[i, i] != (byte)player)
                        break;
                    if (i == 2) {
                        DrawWiningLine( new Point( -1, -1 ), new Point( i + 1, i + 1 ) );
                        if (player == Player.X)
                            return GameState.CrossWins;
                        else
                            return GameState.CircleWins;
                    }
                }
            }

            //Inverse diag
            if (x + y == 2) {
                for (int i = 0; i < 3; i++) {
                    if (board[i, (2) - i] != (byte)player)
                        break;
                    if (i == 2) {
                        DrawWiningLine( new Point( -1, 3 ), new Point( 3, -1 ) );
                        if (player == Player.X)
                            return GameState.CrossWins;
                        else
                            return GameState.CircleWins;
                    }
                }
            }

            if (moveCount == (Math.Pow( 3, 2 )))
                return GameState.Draw;
            return GameState.ValidMove;
        }

        private void DrawWiningLine(Point begin, Point end) {


            Point beginPoint = new Point( begin.X * squareSize.X + squareSize.X * 0.5f, 
                begin.Y * squareSize.Y + squareSize.Y * 0.5f );

            Point endPoint = new Point( end.X * squareSize.X + squareSize.X * 0.5f, 
                end.Y * squareSize.Y + squareSize.Y * 0.5f);

            winingLine.X1 = beginPoint.X;
            winingLine.Y1 = beginPoint.Y;
            winingLine.X2 = endPoint.X;
            winingLine.Y2 = endPoint.Y;
        }

        public void ResetGame() {
            gameElements.RemoveRange( 0, gameElements.Count );
            moveCount = 0;
            gameState = 0;
            Array.Clear( board, 0, board.Length );
        }

    }
}
