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


        public ChatForm() {
            InitializeComponent();
        }

        private void ChatForm_Load( object sender, EventArgs e ) {
            client = new Client();

            client.OnConnect = () => { WriteLine(txtIn, "Connected to server..." ); };
            client.OnError = (s) => { WriteLine( txtIn, s ); };
            client.OnConnectionFail = (s) => {
                WriteLine( txtIn, s );
                EditButtonText( btnConnect, "Connect" );
            };


            client.OnPacketReceived = DispatchPacket;

            client.OnServerDisconnect = () => {
                WriteLine(txtIn, "ERROR 500: An existing connection was forcibly closed by the server " );
                EditButtonText( btnConnect, "Connect" );
            };

            client.OnDisconnect = () => {
                WriteLine( txtIn, "Disconnected from server..." );
                EditButtonText( btnConnect, "Connect" );
            };

        }

        private void btnSend_Click( object sender, EventArgs e ) {

            if (client.isConnected) {
                Packet p = new Packet( PacketType.Chat, client.ID );
                p.data.Add( txtName.Text );
                p.data.Add( txtOut.Text );

                //Send message to server

                WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );


                //TODO: Implement Queue
                client.connectionSocket.Send( p.ToBytes() );

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
            if (!client.isConnected) {
                client.Connect(NetData.localhost,  NetData.PORT);
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
        void EditButtonText(Button btn, string text ) {

            EditButtonDelegate edit = new EditButtonDelegate( EditButtonText );
            if (btn.InvokeRequired == false) {
                btn.Text = text;
            }
            else {
                this.Invoke(edit, new object[] { btn, text } );
            }
        }
        
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


        private void ChatForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if (client.isConnected)
                client.Disconnect();
        }

        private void txtIn_TextChanged( object sender, EventArgs e ) {
            txtIn.Select( txtIn.Text.Length, 0 );
            txtIn.ScrollToCaret();
        }

        void DispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Chat: {
                        WriteLine(txtIn, p.data[0] + ": " + p.data[1] );
                        break;
                    }
               
                case PacketType.Client_LogOut: {
                        WriteLine(txtIn, "Client disconnected: " + p.data[0] );

                        break;
                    }
            }

        }

    }
}

