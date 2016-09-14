using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using ServerData;


namespace Client_Forms {


    public partial class ChatForm : Form {


        Client client;
        Stopwatch pingStopWatch;
        Point windowPosition;
        float time = 0;


        public ChatForm() {
            InitializeComponent();
            pingStopWatch = new Stopwatch();
        }

        private void ChatForm_Load( object sender, EventArgs e ) {
            client = new Client();

            client.OnConnect = () => { WriteLine( txtIn, "Connected to server..." ); };
            client.OnError = ( s ) => { WriteLine( txtIn, s ); };
            client.OnConnectionFail = ( s ) => {
                WriteLine( txtIn, s );
                EditButtonText( btnConnect, "Connect" );
            };


            client.OnPacketReceived = DispatchPacket;

            client.OnServerDisconnect = () => {
                WriteLine( txtIn, "ERROR 500: An existing connection was forcibly closed by the server " );
                EditButtonText( btnConnect, "Connect" );
            };

            client.OnDisconnect = () => {
                WriteLine( txtIn, "Disconnected from server..." );
                EditButtonText( btnConnect, "Connect" );
            };

        }

        private void btnSend_Click( object sender, EventArgs e ) {


            //HAY UN BUG
            if (txtOut.Text != "" && txtOut.Text[0] == '/') {
                DispatchCommand( txtOut.Text );
            }
            else {
                if (client.isConnected) {
                    Packet p = new Packet( PacketType.Chat, client.ID );
                    p.data.Add( txtName.Text );
                    p.data.Add( txtOut.Text );

                    WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );

                    //Send message to server
                    //TODO: Implement Queue
                    client.connectionSocket.Send( p.ToBytes() );
                }
            }
            txtOut.Text = "";
            txtOut.Focus();
        }



        private void chatKeyDown( object sender, KeyEventArgs e ) {

            if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control) {
                btnSend_Click( sender, (EventArgs)e );
                e.Handled = true;
            }
        }

        private void btnConnect_Click( object sender, EventArgs e ) {
            if (!client.isConnected) {
                client.Connect( NetData.localhost, NetData.PORT );
                btnConnect.Text = "Disconnect";
            }
            else {
                client.Disconnect();
                btnConnect.Text = "Connect";
            }
        }

        //TODO: Quitar el bug con el boton de desconectar
        //TODO: Pasar esto a una clase donde pueda reutilizarlo

        delegate void EditButtonDelegate( Button btn, string text );
        void EditButtonText( Button btn, string text ) {

            EditButtonDelegate edit = new EditButtonDelegate( EditButtonText );
            if (btn.InvokeRequired == false) {
                btn.Text = text;
            }
            else {
                this.Invoke( edit, new object[] { btn, text } );
            }
        }

        delegate void WriteDelegate( RichTextBox obj, string text );
        void Write( RichTextBox obj, string text ) {
            WriteDelegate write = new WriteDelegate( Write );
            if (obj.InvokeRequired == false)
                obj.AppendText( text );
            else
                this.Invoke( write, new object[] { obj, text } );
        }

        delegate void WriteLineDelegate( RichTextBox obj, string text );
        void WriteLine( RichTextBox obj, string text ) {

            WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );



            if (obj.InvokeRequired == false) {
                txtIn.ReadOnly = false;
                obj.AppendText( text + '\n' );
                //while (txtIn.Text.Contains( "CAT" )) {
                //    int ind = txtIn.Text.IndexOf( "CAT" );
                //    txtIn.Select( ind, "CAT".Length );
                //    Clipboard.Clear();
                //    Clipboard.SetImage( (Image)Client_Forms.Properties.Resources.cat );
                //    txtIn.Paste();
                //    break;
                //}
                //txtIn.ReadOnly = false;
            }
            else
                this.Invoke( writeLine, new object[] { obj, text } );
        }


        private void ChatForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if (client.isConnected)
                client.Disconnect();
        }

        private void txtIn_TextChanged( object sender, EventArgs e ) {
            txtIn.Select( txtIn.Text.Length, 0 );
            txtIn.ScrollToCaret();
        }
        private void DispatchCommand( string command ) {

            command = command.ToLower();
            if (client.isConnected) {
                switch (command) {
                    case "/ping": {
                            pingStopWatch.Restart();
                            Packet p = new Packet( PacketType.Ping, client.ID );
                            client.connectionSocket.Send( p.ToBytes() );
                            break;
                        }
                    case "/buzz": {
                            Packet p = new Packet( PacketType.Chat_Buzzer, client.ID );
                            client.connectionSocket.Send( p.ToBytes() );
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        void DispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Chat: {
                        WriteLine( txtIn, p.data[0] + ": " + p.data[1] );

                        break;
                    }

                case PacketType.Client_LogOut: {
                        WriteLine( txtIn, "Client disconnected: " + p.data[0] );

                        break;
                    }
                case PacketType.Pong: {
                        pingStopWatch.Stop();
                        WriteLine( txtIn, "Pong: " + pingStopWatch.ElapsedMilliseconds.ToString() );
                        break;
                    }
                case PacketType.Chat_Buzzer: {
                        if (!buzzTimer.Enabled) {
                            startTimer ST = new startTimer( StartTimer );

                            //buzzTimer.Start();
                            this.Invoke( ST );
                            time = 0;
                            windowPosition = this.Location;
                        }
                        break;
                    }
            }

        }

        delegate void startTimer();
        void StartTimer() {
            buzzTimer.Start();
        }

        private void buzzTimer_Tick( object sender, EventArgs e ) {

            time += 10;

            Random rand = new Random();

            this.Location = new Point( windowPosition.X + rand.Next( -5, 5 ), windowPosition.Y + rand.Next( -5, 5 ) );

            if (time > 500) {
                this.Location = windowPosition;
                buzzTimer.Stop();
            }

        }

    }
}

