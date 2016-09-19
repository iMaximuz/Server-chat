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
using System.Runtime.InteropServices;

namespace Client_Forms {


    public partial class ChatForm : Form {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();


        Client client;
        Stopwatch pingStopWatch;
        Point windowPosition;
        float time = 0;

        Dictionary<string, Image> emoticons;

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


            //Load emoticons
            emoticons = new Dictionary<string, Image>();
            emoticons.Add( ":)", Properties.Resources.emote_smile );
            emoticons.Add( ":D", Properties.Resources.emote_happy );
            emoticons.Add( ":P", Properties.Resources.emote_P );
            emoticons.Add( ":angry:", Properties.Resources.emote_angry );
            emoticons.Add( ":S", Properties.Resources.emote_badfeeling );
            emoticons.Add( ":laugh:", Properties.Resources.emote_laugh );
            emoticons.Add( ":l", Properties.Resources.emote_pokerface );
            emoticons.Add( ":(", Properties.Resources.emote_sad2 );
            emoticons.Add( ":'(", Properties.Resources.emote_sad );
            emoticons.Add( "D':", Properties.Resources.emote_sad3 );
            emoticons.Add( ";)", Properties.Resources.emote_winkyface );

            List<string> keys = new List<string>( emoticons.Keys );
            foreach(string key in keys) {

                Bitmap emote = new Bitmap( emoticons[key].Width, emoticons[key].Height );
                Graphics graphics = Graphics.FromImage( emote );
                graphics.Clear( txtIn.BackColor );
                graphics.DrawImage( emoticons[key], new Rectangle( Point.Empty, new Size( emote.Width, emote.Height ) ) );
                emoticons[key] = emote;
            }

        }

        private void btnSend_Click( object sender, EventArgs e ) {


            //HAY UN BUG
            if (txtOut.Text != "" && txtOut.Text[0] == '/') {
                DispatchCommand( txtOut.Text );
            }
            else {
                if (client.isConnected) {
                    Packet p = new Packet( PacketType.Chat, client.ID );
                    p.data.Add( "name", txtName.Text );
                    p.data.Add( "message", txtOut.Text );
                    //Send message to server
                    //TODO: Implement Queue
                    client.connectionSocket.Send( p.ToBytes() );

                    //WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );
                }
                WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );
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
                if (!client.attemtingConnection) {
                    client.Connect( NetData.localhost, NetData.PORT );
                    btnConnect.Text = "Disconnect";
                }
                else {
                    client.attemtingConnection = false;
                    btnConnect.Text = "Connect";
                }
            }
            else {
                client.Disconnect();
                btnConnect.Text = "Connect";
            }
        }

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
            if (obj.InvokeRequired == false) {
                obj.AppendText( text );
                SetEmoticons( obj );
            }
            else
                this.Invoke( write, new object[] { obj, text } );
        }

        delegate void WriteLineDelegate( RichTextBox obj, string text );
        void WriteLine( RichTextBox obj, string text ) {
            WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );
            if (obj.InvokeRequired == false) {
                obj.AppendText( text + '\n' );
                SetEmoticons( obj );
            }
            else
                this.Invoke( writeLine, new object[] { obj, text } );
        }

        void SetEmoticons(RichTextBox rtb ) {
            rtb.ReadOnly = false;
            IDataObject currentClipboard = Clipboard.GetDataObject();

            foreach (string key in emoticons.Keys) {

                while (rtb.Text.Contains( key )) {

                    int index = txtIn.Text.IndexOf( key );
                    rtb.Select( index, key.Length );
                    Clipboard.Clear();
                    Clipboard.SetImage( emoticons[key] );
    
                    rtb.Paste();
                }
            }
            Clipboard.SetDataObject( currentClipboard );
            rtb.ReadOnly = false;
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
                        WriteLine( txtIn, p.data["name"] + ": " + p.data["message"] );

                        break;
                    }

                case PacketType.Client_LogOut: {
                        WriteLine( txtIn, "Client disconnected: " + p.data["name"] );

                        break;
                    }
                case PacketType.Pong: {
                        pingStopWatch.Stop();
                        WriteLine( txtIn, "Pong: " + pingStopWatch.ElapsedMilliseconds.ToString() );
                        break;
                    }
                case PacketType.Chat_Buzzer: {
                        if (!buzzTimer.Enabled) {
                            StartTimerDelegate ST = new StartTimerDelegate( StartTimer );
                            this.Invoke( ST );
                            time = 0;
                            windowPosition = this.Location;
                        }
                        break;
                    }
            }

        }

        delegate void StartTimerDelegate();
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

        private void pbEmoticons_Click( object sender, EventArgs e ) {
            lvEmoticons.Visible = !lvEmoticons.Visible;
            lvEmoticons.Focus();
        }

        private void lvEmoticons_Leave( object sender, EventArgs e ) {
            lvEmoticons.Visible = false;
        }


        private void lvEmoticons_SelectedIndexChanged( object sender, EventArgs e ) {
            if (lvEmoticons.SelectedIndices.Count > 0) {
                int i = lvEmoticons.SelectedIndices[0];
                txtOut.AppendText( lvEmoticons.Items[i].ToolTipText + " " );
                txtOut.Focus();
            }
        }

        private void pbClose_Click( object sender, EventArgs e ) {
            this.Owner.Show();
            Close();
        }

        private void pbTitleBar_MouseDown( object sender, MouseEventArgs e ) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }


        private void PictureBoxHover(object sender, EventArgs e ) {
            ((PictureBox)sender).BackColor = ColorTranslator.FromHtml( "#035e7c" ); ;
        }

        private void PictureBoxLeave(object sender, EventArgs e) {

            ((PictureBox)sender).BackColor = Color.Transparent;

        }


        private void lvEmoticons_MouseLeave( object sender, EventArgs e ) {
            lvEmoticons.Visible = false;
        }
    }
}

