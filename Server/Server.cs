using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
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

            while (server.isOnline) {

                string command = Console.ReadLine().ToLower();
                DispatchServerCommands( command );

            }

            Console.WriteLine( "Server offline..." );
            Console.ReadKey();
        }

        static void DispatchServerCommands( string command ) {

            switch (command) {
                case "shutdown":
                    Console.WriteLine( "Server is shuting down" );
                    server.Shutdown();
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
