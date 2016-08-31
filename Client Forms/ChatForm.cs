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



        public ChatForm() {
            InitializeComponent();
        }

        private void btnSend_Click( object sender, EventArgs e ) {

            if (connected) {
                Packet p = new Packet( PacketType.Chat, clientID );
                p.data.Add( txtName.Text );
                p.data.Add( txtOut.Text );

                //Send message to server

                WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );

                connectionSocket.Send( p.ToBytes() );

                txtOut.Text = "";
                txtOut.Focus();
            }
        }


        private void chatKeyDown( object sender, KeyEventArgs e ) {

            if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control) {
                btnSend_Click( sender, (EventArgs)e );
                e.Handled = true;
            }
        }

        private void btnConnect_Click( object sender, EventArgs e ) {
            if (!connected) {
                AttemptConnection connect = new AttemptConnection( ConnectToServer );
                connect.BeginInvoke( null, null );
                btnConnect.Text = "Disconnect";
            }
            else {
                DisconnectFromServer();
                btnConnect.Text = "Connect";
            }
        }

        //TODO: Quitar el bug con el boton de desconectar

        //TODO: Pasar esto a una clase donde pueda reutilizarlo
        delegate void WriteDelegate( RichTextBox obj, string text );
        void Write( RichTextBox obj, string text ) {
            WriteDelegate write = new WriteDelegate( Write );
            if( obj.InvokeRequired == false) 
                obj.Text += text;
            else
                this.Invoke( write, new object[] { obj, text } );
        }

        delegate void WriteLineDelegate( RichTextBox obj, string text );
        void WriteLine( RichTextBox obj, string text ) {

            WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );

            if ( obj.InvokeRequired == false )
                obj.Text += text + '\n';
            else
                this.Invoke( writeLine, new object[] { obj, text } );
        }

        delegate void AttemptConnection();
        void ConnectToServer() {

            if (!connected) {

                //TODO: quitar la ip harcodeada
                hostIPAddress = NetData.localhost;

                connectionSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

                hostAddress = new IPEndPoint( hostIPAddress, NetData.PORT );

                // Try to connect to host
                int connAttempts = 0;

                while (connAttempts < 10) {

                    try {

                        connAttempts++;
                        connectionSocket.Connect( hostAddress );
                        connected = true;
                        break;
                    }
                    catch (SocketException ex) {

                        WriteLine( txtIn, "Could not connect to host... Attempt " + connAttempts.ToString());

                        if (connAttempts == 10)
                            WriteLine( txtIn, ex.Message );
                    }
                }

                if (connected) {
                    receiveThread = new Thread( GetPacket );
                    receiveThread.Start();
                }
            }
            else {
                WriteLine(txtIn, "Already connected." );
            }
        }

        void DisconnectFromServer( ) {
            if (connected) {

                Packet packet = new Packet( PacketType.Client_LogOut, clientID );
                packet.data.Add( txtName.Text );

                connectionSocket.Send( packet.ToBytes() );
                ShutdownClient();
            }
        }

        void ShutdownClient() {

            connected = false;
            connectionSocket.Shutdown( SocketShutdown.Both );
            connectionSocket.Close();
            //receiveThread.Abort();
           
        }

        void GetPacket() {

            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int recivedData;

            try {
                while (true) {

                    //TODO: Server disconnection;

                    recivedData = connectionSocket.Receive( buffer );

                    if (recivedData > 0) {

                        Packet packet = new Packet( buffer );
                        DispatchPacket( packet );

                    }

                }
            }
            catch( SocketException ex) {
                if (connected) {
                    WriteLine( txtIn, "ERROR 500: An existing connection was forcibly closed by the server " );
                    ShutdownClient();
                    WriteLine( txtIn, "Disconnected from server..." );
                }
            }
            catch( ObjectDisposedException ex) {
                WriteLine( txtIn, "Disconnected from server..." );
            }

        }

        void DispatchPacket( Packet p ) {


            switch(p.type) {
                case PacketType.Server_Registration: {

                        clientID = p.senderID;
                        WriteLine( txtIn, "Connected to server." );
                        WriteLine( txtIn, "Client id recieved: " + clientID );
                        break;
                    }
                case PacketType.Server_Closing: {

                        WriteLine( txtIn, "Server is closing..." );
                        ShutdownClient();
                        //TODO: Poder cambiar el texto del boton desde cualquier thread
                        //btnConnect.Text = "Connect";
                        break;
                    }
                case PacketType.Chat: {

                        WriteLine( txtIn, p.data[0] + ": " + p.data[1] );
                        
                        break;
                    }
                
                case PacketType.Client_LogOut: {

                        WriteLine( txtIn, "Client disconnected: " + p.data[0] );

                        break;
                    }
            }

        }

        private void ChatForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if (connected)
                DisconnectFromServer();
        }

        private void txtIn_TextChanged( object sender, EventArgs e ) {
            txtIn.Select( txtIn.Text.Length, 0 );
            txtIn.ScrollToCaret();
        }
    }
}
