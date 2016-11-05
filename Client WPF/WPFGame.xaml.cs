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
using Server_Utilities;

namespace Client_WPF {
    /// <summary>
    /// Interaction logic for WPFGame.xaml
    /// </summary>
    public partial class WPFGame : Window {


        GameBoard board;

        private TimeSpan timeSpan = new TimeSpan( 0, 0, 0, 0, 16 );

        float BOARD_SIZE = 150;

        bool yourTurn = true;

        int playerNumber = 1;
        Cursor[] playerCursors;

        Client client;
        string partner;

        public WPFGame( ) {
            InitializeComponent();
            Title = "P " + playerNumber + "Tic Tac Toe";

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

        public WPFGame(Client client, string partnerName, int playerNumber) {
            InitializeComponent();
            Title = "P " + playerNumber + "Tic Tac Toe";

            this.client = client;
            this.partner = partnerName;
            this.playerNumber = playerNumber;
            if (playerNumber == 2) yourTurn = false;


            canvas.Background = Brushes.White;
            board = new GameBoard( new Point( canvas.Width / 2 - BOARD_SIZE * 0.5, canvas.Height / 2 - BOARD_SIZE * 0.5 ), new Point( BOARD_SIZE, BOARD_SIZE ), 5, Colors.Black );

            playerCursors = new Cursor[2];
            playerCursors[0] = new Cursor( "P1", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Red );
            playerCursors[1] = new Cursor( "P2", new Point( 0, 0 ), new Point( 5, 5 ), Colors.Blue );

            //Creamos el timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler( timer_Tick );

            timer.Interval = timeSpan;
            timer.Start();

        }

        private void RestartGame() {
            Title = "P " + playerNumber + "Tic Tac Toe";
            board.ResetGame();
            if (playerNumber == 2) yourTurn = false;
            else yourTurn = true;
        }


        private void Window_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.R && board.GameState > GameState.CrossWins) {
                RestartGame();
                if (partner != null) {
                    UdpPacket packet = new UdpPacket( UdpPacketType.Game_Restart );
                    packet.WriteData( BitConverter.GetBytes( partner.Length ) );
                    packet.WriteData( Encoding.ASCII.GetBytes( partner ) );
                    client.SendUdpPacket( packet );
                }

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
                Player shape = (Player)(playerNumber);
                GameState state = board.Move( shape, e.GetPosition( this ) );

                if (board.GameState == GameState.CrossWins)
                    Title = "P1 Wins!";
                else if (board.GameState == GameState.CircleWins)
                    Title = "P2 Wins!";
                else if (board.GameState == GameState.Draw)
                    Title = "Draw";
                if (state == GameState.ValidMove) {
                    yourTurn = false;
                }
            }

            UdpPacket clickPacket = new UdpPacket( UdpPacketType.Game_Click );
            Point position = e.GetPosition( this );
            if (partner != null) {
                clickPacket.WriteData( BitConverter.GetBytes( partner.Length ) );
                clickPacket.WriteData( Encoding.ASCII.GetBytes( partner ) );
                clickPacket.WriteData( BitConverter.GetBytes( (int)position.X ) );
                clickPacket.WriteData( BitConverter.GetBytes( (int)position.Y ) );
                client.SendUdpPacket( clickPacket );
            }

        }

        private void Window_MouseMove( object sender, MouseEventArgs e ) {
            playerCursors[playerNumber - 1].Position = e.GetPosition( this );
            if (partner != null) {
                UdpPacket cursorPacket = new UdpPacket( UdpPacketType.Game_Cursor );
                Point position = e.GetPosition( this );
                cursorPacket.WriteData( BitConverter.GetBytes( partner.Length ) );
                cursorPacket.WriteData( Encoding.ASCII.GetBytes( partner ) );
                cursorPacket.WriteData( BitConverter.GetBytes( (int)position.X ) );
                cursorPacket.WriteData( BitConverter.GetBytes( (int)position.Y ) );
                client.SendUdpPacket( cursorPacket );
            }
        }

        public void DispatchPacket(UdpPacket p) {

            switch (p.PacketType) {
                case UdpPacketType.Game_Restart: {
                        RestartGame();
                    }
                    break;
                case UdpPacketType.Game_Cursor: {
                        int usernameSize = p.ReadInt();
                        string username = Encoding.ASCII.GetString( p.ReadData( usernameSize ) );
                        int partnerSize = p.ReadInt();
                        string partnerName = Encoding.ASCII.GetString( p.ReadData( partnerSize ) );
                        int X = p.ReadInt();
                        int Y = p.ReadInt();
                        int otherPlayer = playerNumber == 1 ? 2 : 1;
                        playerCursors[otherPlayer - 1].Position = new Point( X, Y );
                    }
                    break;
                case UdpPacketType.Game_Click: {
                        if (!yourTurn) {

                            int usernameSize = p.ReadInt();
                            string username = Encoding.ASCII.GetString( p.ReadData( usernameSize ) );
                            int partnerSize = p.ReadInt();
                            string partnerName = Encoding.ASCII.GetString( p.ReadData( partnerSize ) );

                            int X = p.ReadInt();
                            int Y = p.ReadInt();

                            int otherPlayer = playerNumber == 1 ? 2 : 1;
                            Player shape = (Player)(otherPlayer);
                            GameState state = board.Move( shape, new Point( X, Y ) );

                            if (board.GameState == GameState.CrossWins)
                                Title = "P1 Wins!";
                            else if (board.GameState == GameState.CircleWins)
                                Title = "P2 Wins!";
                            else if (board.GameState == GameState.Draw)
                                Title = "Draw";

                            if (state == GameState.ValidMove) {
                                yourTurn = true;
                            }
                        }
                    }
                    break;
            }


        }

    }
}
