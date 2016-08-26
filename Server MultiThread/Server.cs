using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ServerData;


namespace Server
{

    //TODO: Crear una queue para el envio de mensajes;

    struct ServerInfo{
        
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
            server.OnClientConnect = ( client ) => { Thread.Sleep( 50 ); SendWelcomeMessage( client.socket ); };
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

        static void DispatchServerCommands(string command) {

            switch (command) {
                case "shutdown":
                    Console.WriteLine( "Server is shuting down" );
                    server.Shutdown();
                    break;
            }

        }

        public static void DispatchPacket( Packet p ) {

            switch ( p.type ) {
                case PacketType.Chat:

                    Console.WriteLine( "Message recived from: " + p.data[0] );
                    Console.WriteLine( "Retransmiting..." );

                    foreach(ClientData client in server.clients) {
                        if( client.id != p.senderID )
                            client.socket.Send( p.ToBytes() );
                    }

                    break;
                case PacketType.Client_LogOut: {

                        Console.WriteLine( "Client petition to disconnect " + p.senderID );

                        server.clients.RemoveAll( x => x.id == p.senderID );
                        foreach (ClientData client in server.clients) {
                            client.socket.Send( p.ToBytes() );
                        }
                                

                        break;
                    }
                default:
                    Console.WriteLine( "ERROR: Unhandled packet type" );
                    break;
            }


        }

        public static void SendWelcomeMessage(Socket socket) {
            Packet p = new Packet( PacketType.Chat, ServerInfo.ID );
            p.data.Add( ServerInfo.name);
            p.data.Add( ServerInfo.motd );
            socket.Send( p.ToBytes() );
        }

    }

}
