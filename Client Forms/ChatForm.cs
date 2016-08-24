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

        Socket clientScoket;
        string clientID;
        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        bool connected = false;

        public ChatForm() {
            InitializeComponent();
        }

        private void btnSend_Click( object sender, EventArgs e ) {
            MessageBox.Show( e.ToString() );
        }


        private void chatKeyDown( object sender, KeyEventArgs e ) {

            if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control) {
                btnSend_Click( sender, (EventArgs)e );
                e.Handled = true;
            }
        }

        private void btnConnect_Click( object sender, EventArgs e ) {
            AttemptConnection connect = new AttemptConnection( ConnectToServer );
            connect.BeginInvoke(null, null );
        }

        delegate void WriteDelegate( RichTextBox obj, string text );
        void Write( RichTextBox obj, string text ) {
            Debug.Assert( txtOut.InvokeRequired == false );
            obj.Text += text;
        }

        delegate void WriteLineDelegate( RichTextBox obj, string text );
        void WriteLine( RichTextBox obj, string text ) {
            Debug.Assert( txtOut.InvokeRequired == false );
            obj.Text += text + '\n';
        }

        delegate void AttemptConnection();
        void ConnectToServer() {

            WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );

            //TODO: quitar la ip harcodeada
            hostIPAddress = NetData.localhost;

            clientScoket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

            hostAddress = new IPEndPoint( hostIPAddress, NetData.PORT );

            // Try to connect to host
            int connAttempts = 0;

            while (connAttempts < 10) {

                try {

                    connAttempts++;
                    clientScoket.Connect( hostAddress );
                    connected = true;
                }
                catch (SocketException ex) {

                   this.Invoke(writeLine, new object[] { txtOut, "Could not connect to host... Attempt " + connAttempts.ToString() } );

                    if (connAttempts == 10)
                        this.Invoke( writeLine, new object[] { txtOut, ex.Message } );
                }
            }

            if (connected) {

            }
        }

        void GetPacket() {

        }

        void DispatchPacket() {

        }

    }
}
