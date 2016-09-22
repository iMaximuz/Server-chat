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
using Server_Utilities;
using System.Runtime.InteropServices;
using System.IO;

namespace Client_Forms {


    public partial class MainForm : Form {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();


        public Client client;
        Stopwatch pingStopWatch;
        Point windowPosition;
        float elapsedBuzzTime = 0;

        ChatUtilities chat;


        public MainForm() {
            InitializeComponent();
            pingStopWatch = new Stopwatch();
        }

        private void ChatForm_Load( object sender, EventArgs e ) {

            //Load the chat utilities for later use
            chat = new ChatUtilities( this );

            client = new Client();

            client.OnConnect = () => { chat.WriteLine( txtIn, "Connected to server..." ); };
            client.OnError = ( s ) => { chat.WriteLine( txtIn, s, Color.Red ); };
            client.OnConnectionFail = ( s ) => {
                chat.WriteLine( txtIn, s );
                EditButtonText( btnConnect, "Connect" );
            };


            client.OnPacketReceived = DispatchPacket;

            client.OnServerDisconnect = () => {
                chat.WriteLine( txtIn, "ERROR 500: An existing connection was forcibly closed by the server " );
                EditButtonText( btnConnect, "Connect" );
            };

            client.OnDisconnect = () => {
                chat.WriteLine( txtIn, "Disconnected from server..." );
                EditButtonText( btnConnect, "Connect" );
            };


            //Set emoticons' back color to be like the richTextBox
            chat.SetEmoticonsBackColor( txtIn.BackColor );


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
                    client.SendPacket( p );

                    //WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );
                }
                chat.WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );
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

        void DispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Chat: {
                        chat.WriteLine( txtIn, p.data["name"] + ": " + p.data["message"] );

                        break;
                    }

                case PacketType.Client_LogOut: {
                        chat.WriteLine( txtIn, "Client disconnected: " + p.data["name"] );

                        break;
                    }
                case PacketType.Pong: {
                        pingStopWatch.Stop();
                        chat.WriteLine( txtIn, "Pong: " + pingStopWatch.ElapsedMilliseconds.ToString() );
                        break;
                    }
                case PacketType.Chat_Buzzer: {
                        if (!buzzTimer.Enabled) {
                            StartTimerDelegate ST = new StartTimerDelegate( StartTimer );
                            this.Invoke( ST );
                            elapsedBuzzTime = 0;
                            windowPosition = this.Location;
                        }
                        break;
                    }
                case PacketType.Chat_File: {

                        string fileName = (string)p.data["fileName"];
                        byte[] file = (byte[])p.data["file"];

                        File.WriteAllBytes( client.GetDownloadFilePath() + fileName, file );

                        chat.WriteLine( txtIn, fileName + " (" + (float)file.Length / 1024.0 + " Kb) " + "received." );

                        break;
                    }
            }

        }

        delegate void StartTimerDelegate();
        void StartTimer() {
            buzzTimer.Start();
        }

        private void buzzTimer_Tick( object sender, EventArgs e ) {

            elapsedBuzzTime += 10;

            Random rand = new Random();

            this.Location = new Point( windowPosition.X + rand.Next( -5, 5 ), windowPosition.Y + rand.Next( -5, 5 ) );

            if (elapsedBuzzTime > 500) {
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

        private void pbFile_Click( object sender, EventArgs e ) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {

                ofd.Title = "Send File";
                ofd.Filter = "All Files (*.*)|*.*";
                ofd.FilterIndex = 0;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    using (FileStream fs = (FileStream)ofd.OpenFile()) {

                        byte[] buffer = new byte[2 * 1024];
                        using(MemoryStream ms = new MemoryStream()) {
                            int readBytes;
                            while((readBytes = fs.Read(buffer,0,buffer.Length)) > 0) {

                                ms.Write( buffer, 0, readBytes );

                            }

                            byte[] file = ms.ToArray();

                            Packet p = new Packet( PacketType.Chat_File, client.ID );
                            p.data.Add( "fileName", ofd.SafeFileName);
                            p.data.Add( "file", file );
                            client.SendPacket( p );

                            chat.WriteLine(txtIn, "Sending " + ofd.SafeFileName + " (" + (float)file.Length / 1024.0 + " Kb)...", Color.Red);

                        }
                    }
                }
            }
        }

        private void pbBuzzer_Click( object sender, EventArgs e ) {
            Packet p = new Packet( PacketType.Chat_Buzzer, client.ID );
            client.SendPacket( p );
        }
    }
}

