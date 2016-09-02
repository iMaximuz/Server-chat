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

        private void btnSend_Click( object sender, EventArgs e ) {

            if (client.isConnected) {
                Packet p = new Packet( PacketType.Chat, client.ID );
                p.data.Add( txtName.Text );
                p.data.Add( txtOut.Text );

                //Send message to server

                WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );

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
            if (connected)
                DisconnectFromServer();
        }

        private void txtIn_TextChanged( object sender, EventArgs e ) {
            txtIn.Select( txtIn.Text.Length, 0 );
            txtIn.ScrollToCaret();
        }
    }
}
