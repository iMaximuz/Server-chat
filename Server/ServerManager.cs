﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server_Utilities;
using System.IO;
using Server.DataBase;

namespace Server {
    class ServerManager {

        //TODO: Manejo de errores

        public List<ClientData> clients;
        public List<ChatRoom> chatRooms;
        public bool isOnline = false;

        Socket listenSocket;

        Thread listenThread;
        Thread sendThread;
        IPEndPoint address;

        public Action OnStarUp;
        public Action<ClientData> OnClientConnect;
        public Action<ClientData, Packet> OnReceive;
        public Action<ClientData> OnClientDisconnect;
        public Action OnShutdown;

        public ServerDatabase database;
        static public string dataBasePath = "Database.xml";

        class Message {
            public Socket socket;
            public Packet packet;
            public bool toEveryOne;
            public Message() { }
            public Message(Socket s, Packet p, bool toEveryOne = false) {
                socket = s;
                packet = p;
                this.toEveryOne = toEveryOne;
            }
        }

        Queue<Message> messageQueue;

        public ServerManager( IPEndPoint serverAddress ) {
            this.address = serverAddress;
            messageQueue = new Queue<Message>();
            
        }

        public void Start() {
            if (!isOnline) {
                listenSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
                clients = new List<ClientData>();
                chatRooms = new List<ChatRoom>();
                try {
                    listenSocket.Bind( address );
                    isOnline = true;
                    listenThread = new Thread( ListenThread );
                    listenThread.Start();
                    sendThread = new Thread( SendThread );
                    sendThread.Start();
                    database = new ServerDatabase();
                    database.ReadXml( dataBasePath );
                    OnStarUp();

                    ServerDatabase.ChatRoomDataTable chatRoomTable = database.ChatRoom;
                    var query = from chatRoom in chatRoomTable
                                select chatRoom;
                    if (query.Count() > 0) {
                        foreach (ServerDatabase.ChatRoomRow chatRoom in query) {
                            ChatRoom room = new ChatRoom( chatRoom.ChatRoomID, chatRoom.name );
                            chatRooms.Add( room );
                        }
                    }
                }
                catch (SocketException ex) {
                    Console.WriteLine( "ERROR: Server could not start. \n\t" + ex.Message );
                }

            }
        }

        //Listen to incomming connections
        void ListenThread() {
            Console.WriteLine( "Listening for incomming connections..." );
            try {
                while (true) {
                    listenSocket.Listen( 0 );
                    ClientData client = new ClientData( ClientThread, listenSocket.Accept() );
                    clients.Add( client );
                    OnClientConnect( client );
                }
            }
            catch (SocketException ex) {
                Console.WriteLine( ex.Message );
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Listen Thread aborted" );
            } 
        }

        void SendThread() {

            try {
                while (true) {
                    Message m = new Message();
                    if (messageQueue.Count > 0) {
                        m = messageQueue.Dequeue();
                        if (!m.toEveryOne) {
                            //Verificar si el cliente aun sigue connectado
                            if (m.socket != null) {
                                m.socket.Send( PacketFormater.Format( m.packet ) );
                            }
                        }
                        else {
                            foreach (ClientData client in clients) {
                                client.socket.Send( PacketFormater.Format( m.packet ) );
                            }
                        }
                        Thread.Sleep( 10 );
                    }
                    else {
                        Thread.Sleep( 50 );
                    }
                }
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Send Thread aborted" );
            }
        }


        public void ClientThread( object obj ) {
            ClientData client = (ClientData)obj;
            Socket socket = client.socket;

            //socket.ReceiveBufferSize = 200;

            byte[] buffer;
            int readBytes;
            try {
                while (true) {

                    buffer = new byte[socket.ReceiveBufferSize];
                    readBytes = socket.Receive( buffer );

                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize( buffer );

                        if( packetSize == readBytes - sizeof( int ) ) {
                            Packet packet = PacketFormater.MakePacket( buffer );
                            OnReceive( client, packet );
                        }
                        else {

                            int totalBytes = readBytes;
                            int fullBufferSize = packetSize + sizeof( int );
                            byte[] fullPacketBuffer = new byte[fullBufferSize];
                            MemoryStream ms = new MemoryStream( fullPacketBuffer );
                            ms.Write( buffer, 0, buffer.Length );

                            while(totalBytes < fullBufferSize) {
                                readBytes = socket.Receive( buffer );
                                totalBytes += readBytes;
                                ms.Write( buffer, 0, readBytes );
                            }

                            ms.Close();

                            Packet packet = PacketFormater.MakePacket( fullPacketBuffer );
                            OnReceive( client, packet );
                        }

                    }
                    else {
                        OnClientDisconnect( client );
                        break;
                    }
                }
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Client thread aborted" );
            }
        }

        public void Shutdown() {
            isOnline = false;
            CloseAllConnections();
        }

        //SendPacket mandará el packete a la fila para ser enviado posteriormente
        public void SendPacket( ClientData dest, Packet packet ) {
            if (dest != null) {
                messageQueue.Enqueue( new Message( dest.socket, packet ) );
            }
        }

        public void SendPacket( Socket dest, Packet packet ) {
            if (dest != null) {
                messageQueue.Enqueue( new Message( dest, packet ) );
            }
        }

        //SendPacket mandará el packete a todos los clientes agregandalos a la fila de envios
        public void SendPacketToAll(Packet packet ) {
            messageQueue.Enqueue( new Message(null, packet, true) );
        }

        public void SendPacketToAll( ClientData exclude, Packet packet ) {
            foreach (ClientData c in clients) {
                if(c.id != exclude.id) {
                    messageQueue.Enqueue( new Message( c.socket, packet ) );
                }
            }
        }

        public void RemoveClient(string id) {
            clients.RemoveAll( x => x.id == id );
        }

        void CloseAllConnections() {
            Packet p = new Packet( PacketType.Server_Closing, ServerInfo.ID );
            foreach (ClientData c in clients) {
                c.socket.Send( p.ToBytes() );
                c.Disconnect();
            }
            listenSocket.Close();
            listenThread.Abort();
            sendThread.Abort();
        }

        public bool CreateUser(string username, string password, bool isAdmin, State state) {
            
            ServerDatabase.UserDataTable userTable = database.User;

            var query = from usuario in userTable
                        where usuario.username == username
                        select usuario;
            if (query.Count() > 0)
                return false;

            if (String.IsNullOrEmpty( username ) || String.IsNullOrEmpty( password ))
                return false;

            ServerDatabase.UserRow userRow = database.User.NewUserRow();

            userRow.username = username;
            userRow.password = password;
            userRow.admin = isAdmin;
            userRow.state = (int)state;

            database.User.AddUserRow( userRow );
            database.WriteXml( dataBasePath );
            return true;
        }

        public bool CreateRoom(string name) {
            ServerDatabase.ChatRoomDataTable chatRoomTable = database.ChatRoom;

            var query = from chatRoom in chatRoomTable
                        where chatRoom.name == name
                        select chatRoom;
            if (query.Count() > 0)
                return false;

            if (String.IsNullOrEmpty( name ))
                return false;


            ServerDatabase.ChatRoomRow chatRoomRow = database.ChatRoom.NewChatRoomRow();

            chatRoomRow.name = name;
            database.ChatRoom.AddChatRoomRow( chatRoomRow );
            database.WriteXml( dataBasePath );
            chatRooms.Add( new ChatRoom( chatRoomRow.ChatRoomID, chatRoomRow.name ) );
            return true;
        }

        public bool CreatePM(string senderName, string receiverName, string message, bool encrypted) {
            ServerDatabase.PMDataTable pmTable = database.PM;
            ServerDatabase.UserDataTable userTable = database.User;


            if (String.IsNullOrEmpty( senderName ) || String.IsNullOrEmpty( receiverName ))
                return false;


            var senderIDQuery = from usuario in userTable
                        where usuario.username == senderName
                        select usuario;

            if (senderIDQuery.Count() <= 0)
                return false;

            var receiverIDQuery = from usuario in userTable
                                where usuario.username == receiverName
                                select usuario;

            if (senderIDQuery.Count() <= 0)
                return false;

            int senderID = senderIDQuery.ElementAt( 0 ).UserID;
            int receiverID = receiverIDQuery.ElementAt( 0 ).UserID;

            ServerDatabase.PMRow pmRow = database.PM.NewPMRow();
            pmRow.SenderID = senderID;
            pmRow.ReceiverID = receiverID;
            pmRow.message = message;
            pmRow.encrypted = encrypted;
            pmRow.date = System.DateTime.Now;
            database.PM.AddPMRow( pmRow );
            database.WriteXml( dataBasePath );
            return true;
        }

        public List<PrivateMessage> GetPMs(string senderName, string receiverName) {

            ServerDatabase.PMDataTable pmTable = database.PM;
            ServerDatabase.UserDataTable userTable = database.User;

            if (String.IsNullOrEmpty( senderName ) || String.IsNullOrEmpty( receiverName ))
                return null;


            var senderIDQuery = from usuario in userTable
                                where usuario.username == senderName
                                select usuario;

            if (senderIDQuery.Count() <= 0)
                return null;

            var receiverIDQuery = from usuario in userTable
                                  where usuario.username == receiverName
                                  select usuario;

            if (senderIDQuery.Count() <= 0)
                return null;

            int senderID = senderIDQuery.ElementAt( 0 ).UserID;
            int receiverID = receiverIDQuery.ElementAt( 0 ).UserID;

            var query = from privateMessage in pmTable
                        where   (privateMessage.SenderID == senderID && privateMessage.ReceiverID == receiverID) || 
                                (privateMessage.SenderID == receiverID && privateMessage.ReceiverID == senderID)
                        select privateMessage;

            List<PrivateMessage> result = new List<PrivateMessage>();

            for(int i = 0; i < query.Count(); i++) {
                result.Add( new PrivateMessage(query.ElementAt(i).SenderID, query.ElementAt( i ).ReceiverID , query.ElementAt( i ).message , query.ElementAt( i ).encrypted , query.ElementAt( i ).date) );
            }

            return result;

        }

    }
}
