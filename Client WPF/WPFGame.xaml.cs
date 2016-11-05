using Server_Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client_WPF {
    /// <summary>
    /// Interaction logic for WPFGame.xaml
    /// </summary>
    public partial class WPFGame : Window {


        GameBoard board;

        private TimeSpan timeSpan = new TimeSpan( 0, 0, 0, 0, 16 );

        float BOARD_SIZE = 150;

        bool yourTurn = true;

        int playerNumber = 0;
        Cursor[] playerCursors;

        public WPFGame( ) {
            InitializeComponent();
            Title = "Tic Tac Toe";

            board = new GameBoard( new Point( canvas.Width / 2 - BOARD_SIZE * 0.5, canvas.Height / 2 - BOARD_SIZE * 0.5 ), new Point( BOARD_SIZE, BOARD_SIZE ), 5, Colors.Aqua );

            playerCursors = new Cursor[2];
            playerCursors[0] = new Cursor( "P1", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Red );
            playerCursors[1] = new Cursor( "P2", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Blue );

            //Creamos el timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler( timer_Tick );

            timer.Interval = timeSpan;
            timer.Start();

        }

        public WPFGame(int playerNumber) {
            InitializeComponent();
            Title = "Tic Tac Toe";

            board = new GameBoard( new Point( canvas.Width / 2 - BOARD_SIZE * 0.5, canvas.Height / 2 - BOARD_SIZE * 0.5 ), new Point( BOARD_SIZE, BOARD_SIZE ), 5, Colors.Aqua );

            playerCursors = new Cursor[2];
            playerCursors[0] = new Cursor( "P1", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Red );
            playerCursors[1] = new Cursor( "P2", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Blue );

            this.playerNumber = playerNumber;
            if (playerNumber == 1)
                yourTurn = false;

            //Creamos el timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler( timer_Tick );

            timer.Interval = timeSpan;
            timer.Start();

        }

        private void Window_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.R && board.GameState > GameState.CrossWins) {
                Title = "Tic Tac Toe";
                board.ResetGame();
            }
        }

        private void timer_Tick( object sender, EventArgs e ) {

            canvas.Children.RemoveRange( 0, canvas.Children.Count );
            board.Render( canvas );
            playerCursors[0].Render( canvas );
            playerCursors[1].Render( canvas );
        }

        private void canvas_MouseDown( object sender, MouseButtonEventArgs e ) {

            //turn = turn == Player.O ? Player.X : Player.O;

            if (yourTurn) {
                Player shape = (Player)(playerNumber + 1);
                GameState state = board.Move( shape, e.GetPosition( this ) );

                if (board.GameState == GameState.CrossWins)
                    Title = "P1 Wins!";
                else if (board.GameState == GameState.CircleWins)
                    Title = "P2 Wins!";
                else if (board.GameState == GameState.Draw)
                    Title = "Draw";
                yourTurn = false;
            }
        }

        private void Window_MouseMove( object sender, MouseEventArgs e ) {
            playerCursors[playerNumber].Position = e.GetPosition( this );
        }

        public void DispatchPacket(UdpPacket p) {

        }

    }
}
