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

        IPAddress connectionAddress = NetData.remotehost;
        int PORT = NetData.PORT;


        LoginForm login;
        Dictionary<string, PrivateChatForm> chats;

        List<ClientState> loggedUsers;
        List<ChatRoom> chatRooms;

        public Client client;
        Stopwatch pingStopWatch;
        Point windowPosition;
        float elapsedBuzzTime = 0;
        

        public MainForm() {
            InitializeComponent();
            pingStopWatch = new Stopwatch();
        }

        delegate void CloseDelegate();

        private void ChatForm_Load( object sender, EventArgs e ) {

            LoadingForm loadingForm = new LoadingForm();
            CloseDelegate closeLoadingFrm = new CloseDelegate( loadingForm.Close );

            loggedUsers = new List<ClientState>();
            chats = new Dictionary<string, PrivateChatForm>();

            //Load the chat Emoticons with the back color from the richtextbox
            Emotes.LoadEmotes( txtIn.BackColor );

            client = new Client();

            client.OnConnect = () => {  txtIn.WriteLine( this, "Connected to server..." ); this.Invoke( closeLoadingFrm ); };
            client.OnError = ( s ) => { txtIn.WriteLine( this, s, Color.Red ); };
            client.OnConnectionFail = ( s ) => { MessageBox.Show( s, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error ); this.Invoke( closeLoadingFrm ); };

            client.OnPacketReceived = DispatchPacket;
       
            client.OnServerDisconnect = () => {
                txtIn.WriteLine( this, "ERROR 500: An existing connection was forcibly closed by the server, please restart the aplication.", Color.Red);
            };

            client.OnDisconnect = () => {
                txtIn.WriteLine( this, "Disconnected from server..." );
            };

            client.Connect( connectionAddress, PORT );

            loadingForm.ShowDialog( this );
            if (client.isConnected) {
                login = new LoginForm();
                login.ShowDialog( this );
                
            }
            else {
                Application.Exit();
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
                    p.data.Add( "chatroomid", client.sesionInfo.chatroomID );
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
                    //Send message to server
                    //TODO: Implement Queue
                    client.SendPacket( p );

                }
                txtIn.WriteLine( this, client.sesionInfo.username + ": " + txtOut.Text );
                if (!client.isConnected)
                    txtIn.WriteLine( this, "ERROR: Your message could not be sent.", Color.Red);
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

                case PacketType.Client_SignIn: {
                        bool confirmation = (bool)p.data["status"];
                        if (confirmation) {
                            login.signIn.Success();
                        }
                        else {
                            login.signIn.Fail();
                        }
                    } break;
                case PacketType.Client_LogIn: {

                        bool confirmation = (bool)p.data["status"];
                        
                        if ( confirmation ) {
                            int userID = (int)p.data["id"];
                            string username = (string)p.data["username"];
                            chatRooms = (List<ChatRoom>)p.data["chatrooms"];
                            loggedUsers = (List<ClientState>)p.data["users"];

                            login.Close( this );

                            client.sesionInfo.userID = userID;
                            client.sesionInfo.username = username;
                            client.sesionInfo.state = State.Online;
                            client.sesionInfo.chatroomID = 0;

                            lblUsername.ChangeText( this, username );

                            client.isLoggedIn = true;

                            LoadChatRooms();
                            LoadUsers();
                        }
                        else {
                            login.Fail();
                        }
                        break;
                    }
                case PacketType.User_LogIn: {
                        if (client.isLoggedIn) {
                            ClientState user = (ClientState)p.data["user"];
                            ChatRoom chatRoom = chatRooms[0];
                            loggedUsers.Add( user );
                            JoinRoom( user, chatRoom );
                        }
                    } break;
                case PacketType.User_LogOut: {
                        if (client.isLoggedIn) {
                            ClientState user = (ClientState)p.data["user"];
                            loggedUsers.Remove( user );
                            LeaveRoom( user );

                            txtIn.WriteLine( this, p.data["username"] + " disconnected." );
                        }
                        break;
                    }
                case PacketType.User_Status_Change: {
                        if (client.isLoggedIn) {
                            ClientState user = (ClientState)p.data["user"];
                            ClientState userToChange = loggedUsers.Find( x => x.username == user.username );
                            userToChange.state = user.state;

                            TreeNode[] node = tvRooms.Nodes.Find( user.username, true );
                            SetImageIndex( node[0], (int)user.state );

                            if (chats.ContainsKey( user.username )) {
                                chats[user.username].UpdateStateSafe();
                            }

                        }
                    }
                    break;
                case PacketType.ChatRoom_Create: {
                        if (client.isLoggedIn) {
                            int id = (int)p.data["id"];
                            string name = (string)p.data["name"];

                            ChatRoom room = new ChatRoom( id, name );
                            AddChatRoom( room );

                        }

                    } break;
                case PacketType.ChatRoom_Join: {
                        if (client.isLoggedIn) {
                            ChatRoom newRoom = (ChatRoom)p.data["room"];
                            ClientState changedUser = (ClientState)p.data["user"];

                            JoinRoom( changedUser, newRoom );
                        }
                    }
                    break;
                case PacketType.Chat: {
                        if (client.isLoggedIn) {
                            bool encrypted = (bool)p.data["encrypted"];
                            string message = (string)p.data["message"];
                            string name = (string)p.data["name"];
                            if (encrypted) {
                                message = Encryption.EncryptString( message, name );
                            }
                            txtIn.WriteLine( this, name + ": " + message );
                        }
                        break;
                    }
                case PacketType.Pong: {
                        pingStopWatch.Stop();
                        txtIn.WriteLine( this, "Pong: " + pingStopWatch.ElapsedMilliseconds.ToString() );
                        break;
                    }
                case PacketType.Chat_Buzzer: {
                        if (client.isLoggedIn) {
                            if (!buzzTimer.Enabled) {

                                txtIn.WriteLine( this, ":S Buzzzz! - " + p.data["username"], Color.Yellow );

                                StartTimerDelegate ST = new StartTimerDelegate( StartTimer );
                                this.Invoke( ST );
                                elapsedBuzzTime = 0;
                                windowPosition = this.Location;
                            }
                        }
                        break;
                    }
                case PacketType.Video:
                case PacketType.Video_Confirmation:
                case PacketType.Chat_File:
                case PacketType.Chat_Buzzer_Private:
                case PacketType.Chat_Private:
                case PacketType.Load_Private_Chat: {
                        if (client.isLoggedIn) {
                            string key = (string)p.data["partner"];
                            if (chats.ContainsKey( key )) {
                                if (!chats[key].IsDisposed) {
                                    chats[key].DispatchPacket( p );
                                }
                                else {
                                    ClientState user = loggedUsers.Find( x => x.username == key );
                                    chats[key] = new PrivateChatForm( ref user );
                                    chats[key].ShowSafe( this );
                                    chats[key].DispatchPacket( p );
                                }
                            }
                            else {
                                ClientState user = loggedUsers.Find( x => x.username == key );
                                chats.Add( key, new PrivateChatForm( ref user ) );
                                chats[key].ShowSafe( this );
                                chats[key].DispatchPacket( p );
                            }
                        }
                    }
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

        private void pbEmoticons_Click( object sender, EventArgs e ) {
            lvEmoticons.Visible = !lvEmoticons.Visible;
            lvEmoticons.Focus();
        }
        private void lvEmoticons_Leave( object sender, EventArgs e ) {
            lvEmoticons.Visible = false;
            txtOut.Focus();
        }
        private void pbStatus_Click( object sender, EventArgs e ) {
            lvStatus.Visible = !lvEmoticons.Visible;
            lvStatus.Focus();
        }
        private void lvStatus_Leave( object sender, EventArgs e ) {
            lvStatus.Visible = false;
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

        private void lvStatus_SelectedIndexChanged( object sender, EventArgs e ) {
            if (lvStatus.SelectedIndices.Count > 0) {
                int i = lvStatus.SelectedIndices[0]; 
                if (i != (int)client.sesionInfo.state) {
                    lvStatus.Items[i].Selected = false;
                    pbStatus.Image = statusImageList.Images[i + 1]; //NOTA: Esto es mientras no tenemos el modo desconectado
                    client.sesionInfo.state = (State)(i + 1);
                    Packet p = new Packet( PacketType.User_Status_Change, client.ID );
                    p.data.Add( "user", client.sesionInfo );
                    client.SendPacket( p );
                }
                txtOut.Focus();
            }
        }

        private void pbClose_Click( object sender, EventArgs e ) {
            client.Disconnect();
            while (client.isConnected) { Thread.Sleep( 100 ); } // NOTA: Puede ser mala idea hacer esto
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

        private void pbBuzzer_Click( object sender, EventArgs e ) {
            if (client.isConnected) {
                Packet p = new Packet( PacketType.Chat_Buzzer, client.ID );
                p.data.Add( "chatroomid", client.sesionInfo.chatroomID );
                p.data.Add( "username", client.sesionInfo.username );
                client.SendPacket( p );
                txtIn.WriteLine( this, " - Buzz! sent - ", Color.Yellow );
            }
        }

        private void createRoomToolStripMenuItem_Click( object sender, EventArgs e ) {

            InputDialog input = new InputDialog();

            if(input.ShowDialog(this) == DialogResult.OK) {

                string name = input.GetValue();
                bool valid = true;
                foreach(ChatRoom room in chatRooms) {
                    if(room.name == name) {
                        valid = false;
                        break;
                    }
                }

                if (valid) {
                    Packet p = new Packet( PacketType.ChatRoom_Create, client.ID );
                    p.data.Add( "name", name );
                    client.SendPacket( p );
                }
                else {
                    MessageBox.Show( "That chat room already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );

                }
            }


        }

        private void sendPrivateMessageToolStripMenuItem_Click( object sender, EventArgs e ) {

            string partner = tvRooms.SelectedNode.Name;

            ClientState user = loggedUsers.Find( x => x.username == partner );

            if (!chats.ContainsKey( partner )) {
                chats.Add( partner, new PrivateChatForm( ref user ) );
                chats[partner].Show( this );
            }
            else if (chats[partner].IsDisposed) {
                chats[partner] = new PrivateChatForm( ref user );
                chats[partner].Show(this);
            }


        }

        private void cmsRoomToolStripMenuItem_Click( object sender, EventArgs e ) {
            string roomName =  tvRooms.SelectedNode.Name;
            ChatRoom newRoom = chatRooms.Find( x => x.name == roomName );
            if (newRoom != null) {
                if (client.sesionInfo.chatroomID != newRoom.id) {
                    client.sesionInfo.chatroomID = newRoom.id;

                    Packet p = new Packet( PacketType.ChatRoom_Join, client.ID );
                    p.data.Add( "room", newRoom );
                    client.SendPacket( p );
                }
            }
        }



        delegate void AddChatRoomDelegate( ChatRoom room );
        private void AddChatRoom(ChatRoom room) {

            if (!tvRooms.InvokeRequired) {
                chatRooms.Add( room );
                TreeNode node = NewRoomNode( room.name );
                tvRooms.Nodes.Add( node );

            }
            else {
                AddChatRoomDelegate add = new AddChatRoomDelegate( AddChatRoom );
                this.Invoke( add, new object[] { room } );
            }

        }

        //delegate void RemoveChatRoomDelegate();
        //private void RemoveChatRoom(ChatRoom room) {

        //    if (!tvRooms.InvokeRequired) {

        //        chatRooms.Remove( room );
        //    }
        //    else {
        //        RemoveChatRoomDelegate remove = new RemoveChatRoomDelegate( RemoveChatRoom );
        //        this.Invoke( remove );
        //    }

        //}

        delegate void LoadChatRoomsDelegate();
        private void LoadChatRooms() {


            if (!tvRooms.InvokeRequired) {
                foreach (ChatRoom cr in chatRooms) {

                    TreeNode room = NewRoomNode( cr.name );
                    tvRooms.Nodes.Add( room );

                }
                
            }
            else {
                LoadChatRoomsDelegate load = new LoadChatRoomsDelegate( LoadChatRooms );
                this.Invoke( load );
            }

        }

        delegate void LoadUsersDelegate();
        private void LoadUsers() {

            if (!tvRooms.InvokeRequired) {
                foreach (ClientState user in loggedUsers) {
                    ChatRoom room = chatRooms.Find( x => x.id == user.chatroomID );
                    TreeNode node = NewUserNode( user );

                    TreeNode[] roomNodes = tvRooms.Nodes.Find( room.name, false );
                    roomNodes[0].Nodes.Add( node );
                }
                tvRooms.ExpandAll();
            }
            else {
                LoadUsersDelegate load = new LoadUsersDelegate( LoadUsers );
                this.Invoke( load );
            }

        }

        delegate void JoinRoomDelegate(ClientState user, ChatRoom destionation);
        private void JoinRoom(ClientState user, ChatRoom destination) {

            if (!tvRooms.InvokeRequired) {

                ClientState oldUser = loggedUsers.Find( x => x.username == user.username );
                LeaveRoom( oldUser );

                TreeNode newNode = NewUserNode( user ); 
                TreeNode[] roomNode = tvRooms.Nodes.Find( destination.name, false );
                roomNode[0].Nodes.Add( newNode );
                if(!roomNode[0].IsExpanded)
                    roomNode[0].Expand();

                oldUser.chatroomID = user.chatroomID;

            }
            else {
                JoinRoomDelegate join = new JoinRoomDelegate(JoinRoom);
                this.Invoke( join, new object[] { user, destination } );
            }
        }

        delegate void LeaveRoomDelegate( ClientState user );
        private void LeaveRoom( ClientState user ) {

            if (!tvRooms.InvokeRequired) {

                ChatRoom sourceRoom = chatRooms.Find( x => x.id == user.chatroomID );

                TreeNode[] roomNode = tvRooms.Nodes.Find( sourceRoom.name, false );
                TreeNode[] userNode = roomNode[0].Nodes.Find( user.username, false );
                if (userNode.Count() > 0)
                    roomNode[0].Nodes.Remove( userNode[0] );
            }
            else {
                LeaveRoomDelegate leave = new LeaveRoomDelegate( LeaveRoom );
                this.Invoke( leave, new object[] { user } );
            }

        }

        delegate void SetImageIndexDelegate( TreeNode node, int index );
        private void SetImageIndex( TreeNode node, int index ) {

            if (!tvRooms.InvokeRequired) {
                node.ImageIndex = index;
            }
            else {
                SetImageIndexDelegate set = new SetImageIndexDelegate( SetImageIndex );
                this.Invoke( set, new object[] { node, index } );
            }
        }


        private TreeNode NewRoomNode(string name) {
            TreeNode node = new TreeNode( name );
            node.Name = name;
            node.ContextMenuStrip = cmsRoom;
            return node;
        }

        private TreeNode NewUserNode( ClientState user ) {
            TreeNode node = new TreeNode( user.username );
            node.Name = user.username;
            node.ContextMenuStrip = cmsUser;
            node.ImageIndex = (int)user.state;
            return node;
        }


        private void tvRooms_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e ) {
            tvRooms.SelectedNode = e.Node;
        }

        private void pbSettings_Click( object sender, EventArgs e ) {
            SettingsDialog settings = new SettingsDialog();
            settings.ShowDialog( this );
        }

        private void pbMail_Click( object sender, EventArgs e ) {
            MailDialog mail = new MailDialog();
            mail.ShowDialog( this );
        }
    }
}

