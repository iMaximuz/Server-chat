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
                serverAddress = new IPEndPoint( IPAddress.Parse( NetData.GetIP4Address() ), NetData.PORT );
            }
            else {
                WriteLine( "Starting server on " + NetData.localhost );
                serverAddress = new IPEndPoint( NetData.localhost, NetData.PORT );
            }

            server = new ServerManager( serverAddress );

            server.OnStarUp = () => { WriteLine( "Server started" ); };
            //NOTA: Hacer dormir el thread ayuda a enviar los mensajes correctamente
            server.OnClientConnect = ( client ) => { WriteLine( "Client connected..." ); SendRegistrationPacket( client ); SendWelcomeMessage( client ); };
            server.OnClientDisconnect = ( client ) => { WriteLine( "Client correclty disconnected: " + client.sesionInfo.username ); };
            server.OnReceive = DispatchPacket;
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
                case PacketType.Chat_File:
                case PacketType.Chat:
                    foreach (ClientData client in server.clients) {
                        if (client.id != p.senderID)
                            server.SendPacket( client, p );
                    }
                    break;
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
                            confirmation.data.Add( "status", true );
                            confirmation.data.Add( "username", username );
                            confirmation.data.Add( "chatrooms", server.chatRooms );
                            sender.sesionInfo.username = username;
                            sender.sesionInfo.state = State.Online;
                            sender.sesionInfo.chatroomID = 0;
                            
                            Packet notification = new Packet( PacketType.User_LogIn, ServerInfo.ID );
                            notification.data.Add( "username", username );
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

                        WriteLine( "Client petition to disconnect " + p.senderID );

                        server.RemoveClient( p.senderID );
                        server.SendPacketToAll( p );

                        break;
                    }
                case PacketType.Ping: {

                        Packet pong = new Packet( PacketType.Pong, ServerInfo.ID );
                        server.SendPacket( sender, pong );
                        
                        break;
                    }

                case PacketType.Chat_Buzzer: {
                        foreach (ClientData client in server.clients) {
                            if (client.id != p.senderID)
                                server.SendPacket( client, p );
                        }
                        break;
                    }

                default:
                    WriteLine( "ERROR: Unhandled packet type" );
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