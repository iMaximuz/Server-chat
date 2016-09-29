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
        static ServerDatabase database;


        static void Main( string[] args ) {

            Console.WriteLine( "Start on localhost? (Y/N)" );
            string ans = Console.ReadLine().ToLower();

            if (ans[0] != 'y') {
                Console.WriteLine( "Starting server on " + NetData.GetIP4Address() );
                serverAddress = new IPEndPoint( IPAddress.Parse( NetData.GetIP4Address() ), NetData.PORT );
            }
            else {
                Console.WriteLine( "Starting server on " + NetData.localhost );
                serverAddress = new IPEndPoint( NetData.localhost, NetData.PORT );
            }

            server = new ServerManager( serverAddress );

            server.OnStarUp = () => { Console.WriteLine( "Server started" ); };
            //NOTA: Hacer dormir el thread ayuda a enviar los mensajes correctamente
            server.OnClientConnect = ( client ) => { Console.WriteLine( "Client connected..." ); SendRegistrationPacket( client ); SendWelcomeMessage( client ); };
            server.OnClientDisconnect = ( client ) => { Console.WriteLine( "Client correclty disconnected: " + client.id ); };
            server.OnReceive = DispatchPacket;
            server.OnShutdown = () => { Console.WriteLine( "Server closed..." ); };

            server.Start();

            database = new ServerDatabase();
            database.ReadXml( "Database.xml" );


            while (server.isOnline) {
                Console.Write(" > ");
                string command = Console.ReadLine();
                DispatchServerCommands( command );

            }

            


            Console.WriteLine( "Server offline..." );
            Console.ReadKey();
        }

        static void DispatchServerCommands( string command ) {


            string[] args = command.Split( ' ' );

            switch (args[0].ToLower()) {
                case "shutdown":
                    Console.WriteLine( "Server is shuting down" );
                    server.Shutdown();
                    break;
                case "add":
                    if (args[1] != null) {
                        switch (args[1].ToLower()) {
                            case "user": {
                                    ServerDatabase.UserRow userRow = database.User.NewUserRow();

                                    userRow.username = args[2];
                                    userRow.password = args[3];
                                    userRow.admin = true;
                                    userRow.state = 0;

                                    database.User.AddUserRow( userRow );
                                    database.WriteXml( "Database.xml" );

                                    Console.WriteLine( "User: " + args[2] + " added to database." );
                                }
                                break;
                            case "chatroom": {
                                    if (!String.IsNullOrEmpty( args[2] )) { 
                                        if (args[2] == "public") {
                                            ServerDatabase.ChatRoomRow chatroomRow = database.ChatRoom.NewChatRoomRow();
                                            chatroomRow.name = args[3];
                                            database.ChatRoom.AddChatRoomRow(chatroomRow);
                                            database.WriteXml( "Database.xml" );
                                            Console.WriteLine( "Public chat room " + args[2] + " added to database." );
                                        }
                                        else if(args[2] == "private") {
                                            ServerDatabase.PrivateChatRoomRow chatroomRow = database.PrivateChatRoom.NewPrivateChatRoomRow();
                                            chatroomRow.name = args[3];
                                            database.PrivateChatRoom.AddPrivateChatRoomRow( chatroomRow );
                                            database.WriteXml( "Database.xml" );
                                            Console.WriteLine( "Private chat room " + args[2] + " added to database." );
                                        }
                                    }
                                } break;
                            default:
                                Console.WriteLine( args[1] + " is not a valid argument for add" );
                                break;
                        }//switch 2
                    }
                    break;
                case "list": {
                        if (args[1] != null) {
                            switch (args[1].ToLower()) {
                                case "user": {
                                        ServerDatabase.UserDataTable userTable = database.User;
                                        var query = from usuario in userTable
                                                    select usuario;
                                        foreach (var element in query) {
                                            Console.WriteLine( "[" + element.UserID.ToString() + "] " + element.username.ToString() + " " + element.password.ToString() );
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
                                    Console.WriteLine( args[1] + " is not a valid argument for add" );
                                    break;
                            }//switch 2
                        }
                    } break;
                case "save":

                    database.WriteXml( "Database.xml" );
                    Console.WriteLine( "Database saved." );
                    break;
                default:
                    Console.WriteLine( "Unknown command." );
                    break;
            }

        }

        public static void DispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Chat_File:
                case PacketType.Chat:
                    foreach (ClientData client in server.clients) {
                        if (client.id != p.senderID)
                            server.SendPacket( client, p );
                    }
                    break;
                case PacketType.Client_LogOut: {

                        Console.WriteLine( "Client petition to disconnect " + p.senderID );


                        server.RemoveClient( p.senderID );
                        foreach (ClientData client in server.clients) {
                            server.SendPacket( client, p );
                        }


                        break;
                    }
                case PacketType.Ping: {

                        Packet pong = new Packet( PacketType.Pong, ServerInfo.ID );

                        foreach (ClientData client in server.clients) {
                            if (p.senderID == client.id)
                                server.SendPacket( client, pong );
                        }

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
                    Console.WriteLine( "ERROR: Unhandled packet type" );
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

    }

}