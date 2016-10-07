using Server_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Forms {
    public partial class PrivateChatForm : Form {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();

        MainForm owner;
        Client client;

        Stopwatch pingStopWatch;


        public PrivateChatForm() {
            InitializeComponent();
        }

        private void PrivateChatForm_Load( object sender, EventArgs e ) {

            owner = (MainForm)Owner;
            client = owner.client;

        }

        private void btnSend_Click( object sender, EventArgs e ) {
            //HAY UN BUG
            if (txtOut.Text != "" && txtOut.Text[0] == '/') {
                DispatchCommand( txtOut.Text );
            }
            else {
                if (client.isConnected) {
                    Packet p = new Packet( PacketType.Chat, owner.client.ID );
                    p.data.Add( "chatroomid", client.sesionInfo.chatroomID );
                    p.data.Add( "name", client.sesionInfo.username );
                    p.data.Add( "message", txtOut.Text );
                    //Send message to server
                    //TODO: Implement Queue
                    client.SendPacket( p );

                    //WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );
                }
                txtIn.WriteLine( this, client.sesionInfo.username + ": " + txtOut.Text );
                if (!client.isConnected)
                    txtIn.WriteLine( this, "ERROR: Your message could not be sent.", Color.Red );
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

        private void DispatchCommand( string command ) {

            command = command.ToLower();
            if (client.isConnected) {
                switch (command) {
                    case "/ping": {
                            pingStopWatch.Restart();
                            Packet p = new Packet( PacketType.Ping, client.ID );
                            client.SendPacket( p );
                            break;
                        }
                    case "/buzz": {
                            Packet p = new Packet( PacketType.Chat_Buzzer, client.ID );
                            client.SendPacket( p );
                            break;
                        }
                    default:
                        break;
                }
            }
        }


        public void DispatchPacket() {



        }


        private void PictureBoxHover( object sender, EventArgs e ) {
            ((PictureBox)sender).BackColor = ColorTranslator.FromHtml( "#035e7c" ); ;
        }

        private void PictureBoxLeave( object sender, EventArgs e ) {

            ((PictureBox)sender).BackColor = Color.Transparent;

        }

        private void pbTitleBar_MouseDown( object sender, MouseEventArgs e ) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }


    }
}
