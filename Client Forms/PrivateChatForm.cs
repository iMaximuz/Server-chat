using Server_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        string chatId;

        MainForm owner;
        Client client;
        ClientState partner;

        private int elapsedBuzzTime;
        private Point windowPosition;

        public PrivateChatForm() {
            InitializeComponent();
        }

        public PrivateChatForm(ref ClientState partner) {
            InitializeComponent();
            this.partner = partner;
            chatId = Guid.NewGuid().ToString();

        }

        private void PrivateChatForm_Load( object sender, EventArgs e ) {
            owner = (MainForm)Owner;
            client = owner.client;
            lblUsername.Text = partner.username;
            pbStatus.Image = statusImageList.Images[(int)partner.state];

            Packet p = new Packet( PacketType.Load_Private_Chat, client.ID );
            p.data.Add( "name", client.sesionInfo.username );
            p.data.Add( "partner", partner.username );

            client.SendPacket( p );

        }

        private void txtIn_TextChanged( object sender, EventArgs e ) {
            txtIn.Select( txtIn.Text.Length, 0 );
            txtIn.ScrollToCaret();
        }


        private void btnSend_Click( object sender, EventArgs e ) {
            //HAY UN BUG
            if (txtOut.Text != "" && txtOut.Text[0] == '/') {
                DispatchCommand( txtOut.Text );
            }
            else {
                if (client.isConnected) {
                    Packet p = new Packet( PacketType.Chat_Private, owner.client.ID );
                    p.data.Add( "partner", partner.username );
                    p.data.Add( "name", client.sesionInfo.username );

                    bool encrypting = Properties.Settings.Default.EncryptMessages;
                    p.data.Add( "encrypted", encrypting );
                    if (encrypting) {
                        string encryptedMessage = Encryption.EncryptString( txtOut.Text, client.sesionInfo.username );
                        p.data.Add( "message", encryptedMessage );
                    }
                    else {
                        p.data.Add( "message", txtOut.Text );
                    }

                    client.SendPacket( p );
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
                    case "/buzz": {
                            Packet p = new Packet( PacketType.Chat_Buzzer_Private, client.ID );
                            p.data.Add( "partner", partner.username );
                            client.SendPacket( p );
                            break;
                        }
                    default:
                        break;
                }
            }
        }


        public void DispatchPacket(Packet p) {

            switch (p.type) {
                case PacketType.Load_Private_Chat: {

                        int count = (int)p.data["count"];
                        if(count > 0) {
                            List<PrivateMessage> pms = (List<PrivateMessage>)p.data["messages"];

                            foreach( PrivateMessage pm in pms) {
                                string message = pm.message;
                                string name = "";

                                if (pm.senderID == client.sesionInfo.userID) 
                                    name = client.sesionInfo.username;
                                else 
                                    name = partner.username;

                                if (pm.encrypted) 
                                    message = Encryption.EncryptString( message, name );

                                txtIn.WriteLine( this, name + ": " + message );
                            }

                        }

                    }
                    break;
                case PacketType.Chat_Private: {
                        bool encrypted = (bool)p.data["encrypted"];
                        string message = (string)p.data["message"];
                        string name = (string)p.data["name"];
                        if (encrypted) {
                            message = Encryption.EncryptString( message, name );
                        }
                        txtIn.WriteLine( this, name + ": " + message );
                    }
                    break;
                case PacketType.Chat_File: {
                        string fileName = (string)p.data["fileName"];
                        byte[] file = (byte[])p.data["file"];

                        File.WriteAllBytes( client.GetDownloadFilePath() + fileName, file );

                        txtIn.WriteLine( this, fileName + " (" + (float)file.Length / 1024.0 + " Kb) " + "received." );
                    }
                    break;
                case PacketType.Chat_Buzzer_Private: {
                        if (!buzzTimer.Enabled) {

                            txtIn.WriteLine( this, ":S Buzzzz! - " + p.data["username"], Color.Yellow );

                            StartTimerDelegate ST = new StartTimerDelegate( StartTimer );
                            this.Invoke( ST );
                            elapsedBuzzTime = 0;
                            windowPosition = this.Location;
                        }
                    }
                    break;
                case PacketType.Video: {
                        ReceiveCameraPacket( p );
                    }
                    break;
                case PacketType.Video_Confirmation:
                    {
                        Camera.CanSend = true;
                    }break;
                default:
                    txtIn.WriteLine( this, "ERROR: Packet type not supported", Color.Red);
                    break;
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

        private void pbFile_Click( object sender, EventArgs e ) {
            if (client.isConnected) {
                using (OpenFileDialog ofd = new OpenFileDialog()) {

                    ofd.Title = "Send File";
                    ofd.Filter = "All Files (*.*)|*.*";
                    ofd.FilterIndex = 0;
                    ofd.Multiselect = false;

                    if (ofd.ShowDialog() == DialogResult.OK) {
                        using (FileStream fs = (FileStream)ofd.OpenFile()) {

                            byte[] buffer = new byte[2 * 1024];
                            using (MemoryStream ms = new MemoryStream()) {
                                int readBytes;
                                while ((readBytes = fs.Read( buffer, 0, buffer.Length )) > 0) {

                                    ms.Write( buffer, 0, readBytes );

                                }

                                byte[] file = ms.ToArray();

                                Packet p = new Packet( PacketType.Chat_File, client.ID );
                                p.data.Add( "partner", partner.username );
                                p.data.Add( "fileName", ofd.SafeFileName );
                                p.data.Add( "file", file );
                                client.SendPacket( p );


                                txtIn.WriteLine( this, "Sending " + ofd.SafeFileName + " (" + (int)(file.Length / 1024.0) + " Kb)...", Color.Red );
                            }
                        }
                    }
                }
            }
            else {
                txtIn.WriteLine( this, "ERROR: This client is not connected to a server.", Color.Red );
            }
        }


        private void pbClose_Click( object sender, EventArgs e ) {
            if (Camera.OwnerChat == chatId) {
                Camera.Stop();
                Microphone.EndRecording();
            }
            Dispose();
        }

        private void pbVideo_Click( object sender, EventArgs e ) {
            //Start camera
            //If camera is in no use, we can send a webcam request
            if (client.hasCamera) {
                if (Camera.Detect()) {
                    if (!Camera.IsRunning) {
                        StartWebcam();
                        //Packet packet = new Packet( PacketType.WebCamRequest );
                        //packet.tag["chatID"] = chatID;
                        //packet.tag["sender"] = ClientSession.username;
                        //packet.tag["channels"] = Microphone.Channels;
                        //ClientSession.Connection.SendPacket( packet );
                        //Camera.OwnerChat = chatID;
                        //Camera.Start();
                        //Camera.OnNewFrameCallback(SendCameraPacket);
                        ////start audio record
                        //Microphone.OnAudioInCallback(SendAudioStream);
                        //Microphone.StartRecording();

                    }
                    else {
                        //Camera is in use;
                        if (Camera.OwnerChat == chatId) {
                            Camera.Stop();
                            Microphone.EndRecording();
                        }
                        else {
                            MessageBox.Show( "La camara se encuentra en uso por otro chat", "Camara en uso", MessageBoxButtons.OK );
                        }
                    }
                }
                else {
                    MessageBox.Show( "No webcam was found on your machine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                }
            }
            txtIn.Focus();
        }

        public void StartWebcam() {
            if (!Camera.IsRunning) {
                Camera.OwnerChat = chatId;
                Camera.Start();
                Camera.OnNewFrameCallback( SendCameraPacket );
                //start audio record
                //Microphone.OnAudioInCallback( SendAudioStream );
                //Microphone.StartRecording();
            }
        }

        //public void SendAudioStream( byte[] bytes ) {
        //    UdpPacket packet = new UdpPacket( UdpPacketType.AudioStream );
        //    packet.WriteData( BitConverter.GetBytes( chatID ) );
        //    packet.WriteData( BitConverter.GetBytes( (int)bytes.Length ) );
        //    packet.WriteData( bytes );
        //    //Get string bytes
        //    //https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx
        //    string username = ClientSession.username;
        //    byte[] stringBytes = Encoding.Unicode.GetBytes( username );
        //    //System.Buffer.BlockCopy(username.ToCharArray(), 0, stringBytes, 0, stringBytes.Length);
        //    packet.WriteData( BitConverter.GetBytes( stringBytes.Length ) );
        //    packet.WriteData( stringBytes );
        //    //wave format

        //    ClientSession.Connection.SendUdpPacket( packet );
        //}

        //camera new frame event
        public void SendCameraPacket( Bitmap bitmap ) {
            pbWebCamOut.Image = new Bitmap( bitmap, pbWebCamOut.Size );
            Bitmap img = new Bitmap( bitmap, pbWebCamIn.Size );
            if (Camera.CanSend) {
                //pictureBoxCam.Image = img;
                Packet packet = new Packet( PacketType.Video, client.ID);
                packet.data["bitmap"] = img;
                packet.data["partner"] = partner.username;
                //packet.tag["user"] = Header.Text
                client.SendPacket(packet);
                Camera.CanSend = false;
            }
        }

        public void ReceiveCameraPacket( Packet packet ) {
            pbWebCamIn.Image = (Bitmap)packet.data["bitmap"];
            Packet confirmation = new Packet(PacketType.Video_Confirmation, client.ID);
            confirmation.data["partner"] = partner.username;
            client.SendPacket(confirmation);
        }

        private void pbEmoticons_Click( object sender, EventArgs e ) {
            lvEmoticons.Visible = !lvEmoticons.Visible;
            lvEmoticons.Focus();
        }
        private void lvEmoticons_Leave( object sender, EventArgs e ) {
            lvEmoticons.Visible = false;
            txtOut.Focus();
        }

        private void lvEmoticons_SelectedIndexChanged( object sender, EventArgs e ) {
            if (lvEmoticons.SelectedIndices.Count > 0) {
                int i = lvEmoticons.SelectedIndices[0];
                lvEmoticons.Items[i].Selected = false;
                txtOut.AppendText( lvEmoticons.Items[i].ToolTipText + " " );
                txtOut.Focus();
            }
        }

        private void pbBuzzer_Click( object sender, EventArgs e ) {
            Packet p = new Packet( PacketType.Chat_Buzzer_Private, client.ID );
            p.data.Add( "partner", partner.username);
            p.data.Add( "username", client.sesionInfo.username );
            client.SendPacket( p );
            txtIn.WriteLine( this, " - Buzz! sent - ", Color.Yellow );
        }

        public void UpdateStateSafe() {
            UpdateStateDelegate update = new UpdateStateDelegate(UpdateState);
            this.Invoke( update );
        }
        delegate void UpdateStateDelegate();
        public void UpdateState() {
            pbStatus.Image = statusImageList.Images[(int)partner.state];
        }


    }
}
