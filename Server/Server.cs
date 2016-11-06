using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server.DataBase;
using Server_Utilities;


namespace Server {

    //TODO: Crear una queue para el envio de mensajes;

    struct ServerInfo {

        public static string ID = "server";
        public static string name = "max's Server";
        public static string motd = "Welcome!";

    }

    class Server {

        static ServerManager server;
        static IPEndPoint serverAddress;
        


        static void Main( string[] args ) {

            WriteLine( "Start on localhost? (Y/N)" );
            string ans = Console.ReadLine().ToLower();

            if (ans[0] != 'y') {
                WriteLine( "Starting server on " + NetData.GetIP4Address() );
                serverAddress = new IPEndPoint( IPAddress.Parse( NetData.GetIP4Address() ), NetData.TCP_PORT );
            }
            else {
                WriteLine( "Starting server on " + NetData.localhost );
                serverAddress = new IPEndPoint( NetData.localhost, NetData.TCP_PORT );
            }

            server = new ServerManager( serverAddress );

            server.OnStarUp = () => { WriteLine( "Server started" ); };
            //NOTA: Hacer dormir el thread ayuda a enviar los mensajes correctamente
            server.OnClientConnect = ( client ) => { WriteLine( "Client connected..." ); SendRegistrationPacket( client ); SendWelcomeMessage( client ); };
            server.OnClientDisconnect = ( client ) => { WriteLine( "Client correclty disconnected: " + client.sesionInfo.username ); };
            server.OnReceive = DispatchPacket;
            server.OnUdpReceive = DispatchUdpPacket;
            server.OnShutdown = () => { WriteLine( "Server closed..." ); };

            server.Start();

            while (server.isOnline) {
                Console.Write(" > ");
                string command = Console.ReadLine();
                DispatchServerCommands( command );

            }

           
            WriteLine( "Server offline..." );
            Console.ReadKey();
        }

        static void DispatchServerCommands( string command ) {


            string[] args = command.Split( ' ' );

            switch (args[0].ToLower()) {
                case "shutdown":
                    WriteLine( "Server is shuting down" );
                    server.Shutdown();
                    break;
                case "add":
                    if (args.Length > 1 && args[1] != null) {
                        switch (args[1].ToLower()) {
                            case "user": {
                                    if (server.CreateUser( args[2], args[3], true, 0 ))
                                        WriteLine( "User: " + args[2] + " added to database." );
                                    else
                                        WriteLine( "Could not create user. Invalid parameters" );
                                }
                                break;
                            case "chatroom": {
                                    if (!String.IsNullOrEmpty( args[2] )) {
                                        if (args[2] == "public") {
                                            if (server.CreateRoom( args[3] ))
                                                WriteLine( "Public chat room [" + args[3] + "] added to database." );
                                            else
                                                WriteLine( "Could not create chat room. Invalid parameters" );
                                        }
                                        else if (args[2] == "private") {
                                            ServerDatabase.PrivateChatRoomRow chatroomRow = server.database.PrivateChatRoom.NewPrivateChatRoomRow();
                                            chatroomRow.name = args[3];
                                            server.database.PrivateChatRoom.AddPrivateChatRoomRow( chatroomRow );
                                            server.database.WriteXml( "Database.xml" );
                                            WriteLine( "Private chat room " + args[3] + " added to database." );
                                        }
                                    }
                                } break;
                            default:
                                WriteLine( args[1] + " is not a valid argument for add" );
                                break;
                        }//switch 2
                    }
                    break;
                case "list": {
                        if (args[1] != null) {
                            switch (args[1].ToLower()) {
                                case "user": {
                                        ServerDatabase.UserDataTable userTable = server.database.User;
                                        var query = from usuario in userTable
                                                    select usuario;
                                        foreach (var element in query) {
                                            WriteLine( "[" + element.UserID.ToString() + "] " + element.username.ToString() + " " + element.password.ToString() );
                                        }
                                    }
                                    break;
                                case "chatroom": {
                                        if (String.IsNullOrEmpty( args[2] )) {

                                        }
                                        else { 
                                            if (args[2] == "public") {

                                            }
                                            else if (args[2] == "private") {

                                            }
                                        }
                                    }
                                    break;
                                default:
                                    WriteLine( args[1] + " is not a valid argument for add" );
                                    break;
                            }//switch 2
                        }
                    } break;
                case "save":

                    server.database.WriteXml( "Database.xml" );
                    WriteLine( "Database saved." );
                    break;
                default:
                    WriteLine( "Unknown command." );
                    break;
            }

        }

        public static void DispatchPacket( ClientData sender, Packet p ) {

            switch (p.type) {
                case PacketType.Client_SignIn: {
                        string username = (string)p.data["username"];
                        string password = (string)p.data["password"];

                        Packet confirmation = new Packet( PacketType.Client_SignIn, ServerInfo.ID );

                        if (server.CreateUser( username, password, false, State.Offline )) {
                            confirmation.data.Add( "status", true );
                        }
                        else {
                            confirmation.data.Add( "status", false );
                        }
                        server.SendPacket( sender, confirmation );
                    }
                    break;
                case PacketType.Client_LogIn: {
                        string username = (string)p.data["username"];
                        string password = (string)p.data["password"];

                        Packet confirmation = new Packet( PacketType.Client_LogIn, ServerInfo.ID );

                        ServerDatabase.UserDataTable userTable = server.database.User;

                        var query = from usuario in userTable
                                    where usuario.username == username && usuario.password == password
                                    select usuario;
                        if (query.Count() > 0) {

                            int id = query.ElementAt( 0 ).UserID;
                            int victories = query.ElementAt( 0 ).gameVictories;

                            sender.sesionInfo.userID = id;
                            sender.sesionInfo.username = username;
                            sender.sesionInfo.state = State.Online;
                            sender.sesionInfo.chatroomID = 0;
                            sender.sesionInfo.gameVictories = victories;

                            List<ClientState> users = new List<ClientState>();
                            foreach (ClientData client in server.clients) {
                                if(client.sesionInfo.username != "N/A")
                                    users.Add( client.sesionInfo );
                            }

                            confirmation.data.Add( "status", true );

                            confirmation.data.Add( "id", id );
                            confirmation.data.Add( "username", username );
                            confirmation.data.Add( "victories", victories );
                            confirmation.data.Add( "chatrooms", server.chatRooms );
                            confirmation.data.Add( "users", users );

                            Packet notification = new Packet( PacketType.User_LogIn, ServerInfo.ID );
                            notification.data.Add( "user", sender.sesionInfo );

                            server.SendPacketToAll( sender, notification );

                            WriteLine( username + " logged in." );
                        }
                        else {
                            confirmation.data.Add( "status", false );
                        }

                        server.SendPacket( sender, confirmation );

                    }
                    break;
                case PacketType.Client_LogOut: {

                        WriteLine( "Client petition to disconnect " + p.data["username"]);

                        ClientState disconnectedUser = server.clients.Find( x => x.id == p.senderID ).sesionInfo;

                        server.RemoveClient( p.senderID );

                        p.type = PacketType.User_LogOut;
                        p.data.Add( "user", disconnectedUser );

                        server.SendPacketToAll( p );

                        break;
                    }
                case PacketType.User_Status_Change: {
                        ClientState user = (ClientState)p.data["user"];
                        sender.sesionInfo.state = user.state;
                        server.SendPacketToAll( p );
                    }
                    break;
                case PacketType.ChatRoom_Create: {

                        string name = (string)p.data["name"];

                        if (server.CreateRoom( name )) {

                            int id = server.chatRooms.Last().id;

                            Packet confirmation = new Packet( PacketType.ChatRoom_Create, ServerInfo.ID );
                            confirmation.data.Add( "id", id );
                            confirmation.data.Add( "name", name );
                            server.SendPacketToAll( confirmation );

                        }

                    } break;
                case PacketType.ChatRoom_Join: {

                        ChatRoom room = (ChatRoom)p.data["room"];

                        sender.sesionInfo.chatroomID = room.id;

                        Packet notification = new Packet( PacketType.ChatRoom_Join, ServerInfo.ID );
                        notification.data.Add( "user", sender.sesionInfo );
                        notification.data.Add( "room", room );

                        server.SendPacketToAll( notification );
                    }
                    break;
                case PacketType.Ping: {

                        Packet pong = new Packet( PacketType.Pong, ServerInfo.ID );
                        server.SendPacket( sender, pong );
                        
                        break;
                    }

                case PacketType.Chat_Buzzer: {
                        foreach (ClientData client in server.clients) {
                            if (client.id != p.senderID && client.sesionInfo.chatroomID == (int)p.data["chatroomid"])
                                server.SendPacket( client, p );
                        }
                        break;
                    }
                
                case PacketType.Chat:
                    foreach (ClientData client in server.clients)
                    {
                        if (client.id != p.senderID && client.sesionInfo.chatroomID == (int)p.data["chatroomid"])
                            server.SendPacket(client, p);
                    }
                    break;
                case PacketType.Video:
                case PacketType.Video_Confirmation: 
                case PacketType.Chat_File:
                case PacketType.Chat_Buzzer_Private:
                case PacketType.Audio_SetUp:
                case PacketType.Audio_Stop:
                case PacketType.Audio_Confirmation: {
                        string username = (string)p.data["partner"];
                        p.data["partner"] = sender.sesionInfo.username;
                        ClientData client = server.clients.Find( x => x.sesionInfo.username == username );
                        server.SendPacket( client, p );
                    }
                    break;
                case PacketType.Chat_Private:
                    {
                        string senderName = (string)p.data["name"];
                        string receiverName = (string)p.data["partner"];
                        string message = (string)p.data["message"];
                        bool encrypted = (bool)p.data["encrypted"];

                        server.CreatePM( senderName, receiverName, message, encrypted );

                        string username = (string)p.data["partner"];
                        p.data["partner"] = sender.sesionInfo.username;
                        ClientData client = server.clients.Find( x => x.sesionInfo.username == username );
                        server.SendPacket( client, p );
                    }
                    break;
                case PacketType.Load_Private_Chat: {

                        List<PrivateMessage> pms;

                        string senderName = (string)p.data["name"];
                        string receiverName = (string)p.data["partner"];

                        pms = server.GetPMs( senderName, receiverName );
                        if (pms != null) {
                            int count = pms.Count();

                            p.data.Add( "count", count );
                            p.data.Add( "messages", pms );
                        }
                        else {
                            p.data.Add( "count", 0 );
                            p.data.Add( "messages", 0 );
                        }

                        server.SendPacket( sender, p );

                    }
                    break;
                case PacketType.Game_Victory: {
                        sender.sesionInfo.gameVictories++;
                        server.PlayerWon( sender.sesionInfo.username );
                        server.SendPacket( sender, p );
                    }
                    break;
                default:
                    WriteLine( "ERROR: Unhandled packet type" );
                    break;
            }


        }

        public static void DispatchUdpPacket(ClientData sender, UdpPacket p ) {
            switch (p.PacketType) {
                case UdpPacketType.Game_Start:
                case UdpPacketType.Game_End:
                case UdpPacketType.Game_Restart:
                case UdpPacketType.Game_Cursor:
                case UdpPacketType.Game_Click: {

                        int usernameSize = p.ReadInt( 0 );
                        string username = Encoding.ASCII.GetString( p.ReadData( usernameSize, sizeof( int ) ) );
                        ClientData client = server.clients.Find( x => x.sesionInfo.username == username );
                        UdpPacket redirect = new UdpPacket( p.PacketType );
                        redirect.WriteData( BitConverter.GetBytes( sender.sesionInfo.username.Length ) );
                        redirect.WriteData( Encoding.ASCII.GetBytes( sender.sesionInfo.username ) );
                        redirect.WriteData( p.Data );

                        server.SendUdpPacket( client.udpEndpoint, redirect );

                    }
                    break;
                case UdpPacketType.Audio: {



                        int usernameSize = p.ReadInt( 0 );
                        string username = Encoding.ASCII.GetString( p.ReadData( usernameSize, sizeof( int ) ) );
                        ClientData client = server.clients.Find( x => x.sesionInfo.username == username );
                        server.SendUdpPacket( client.udpEndpoint, p );
                    }
                    break;
                default:
                    Console.WriteLine( "Unhandled datagram from " + sender.udpEndpoint );
                    break;
            }
        }

        public static void SendWelcomeMessage( ClientData client ) {
            Packet p = new Packet( PacketType.Chat, ServerInfo.ID );
            p.data.Add( "name", ServerInfo.name );
            p.data.Add( "message", ServerInfo.motd );
            server.SendPacket( client, p );
        }

        public static void SendRegistrationPacket( ClientData client ) {
            Packet p = new Packet( PacketType.Server_Registration, client.id );
            server.SendPacket( client, p );
        }

        static void WriteLine(string text ) {
            ClearCurrentConsoleLine();
            string time = DateTime.Now.ToString("hh:mm:ss");
            Console.WriteLine( "[" + time + "]: " + text );
        }
        public static void ClearCurrentConsoleLine() {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition( 0, Console.CursorTop );
            Console.Write( new string( ' ', Console.WindowWidth ) );
            Console.SetCursorPosition( 0, currentLineCursor );
        }

    }

}