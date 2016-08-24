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
    struct ServerInfo{
        public static string ID = "server";
        public static string name = "max's Server";
        public static string motd = "Welcome!";
    }

    class Server {

        static Socket serverSocket;
        static List<ClientData> clients;
        public static IPEndPoint serverAddress;

        static void Main( string[] args ) {

            // Setting server up
            serverSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            clients = new List<ClientData>();

            Console.WriteLine( "Start on localhost? (Y/N)" );
            string ans = Console.ReadLine().ToLower();

            if (ans[0] != 'y') {
                Console.WriteLine( "Starting server on " + NetData.GetIP4Address() );

                serverAddress = new IPEndPoint( IPAddress.Parse( NetData.GetIP4Address() ), NetData.PORT );
                serverSocket.Bind( serverAddress );
            }
            else {
                Console.WriteLine( "Starting server on " + NetData.localhost );

                serverAddress = new IPEndPoint( NetData.localhost, NetData.PORT );
                serverSocket.Bind( serverAddress );
            }
            Thread listenThread = new Thread( ListenThread );
            listenThread.Start();
            Console.WriteLine( "Server started" );
            Console.WriteLine( "Waiting for incomming connections..." );
        }

        //Listen to incomming connections
        static void ListenThread() {
            
            while (true) {
                serverSocket.Listen( 0 );
                ClientData client = new ClientData( serverSocket.Accept() );
                clients.Add( client );
                Console.WriteLine( "Client connected: " + client.id );
                SendWelcomeMessage( client.socket );
            }
        }

        public static void GetPacket( object obj ) {
            Socket clientSocket = (Socket)obj;

            byte[] buffer;
            int readBytes;

            while (true) {

                buffer = new byte[clientSocket.SendBufferSize];
                readBytes = clientSocket.Receive( buffer );

                if(readBytes > 0) { 
                    //Make a packet from serialized array of bytes
                    Packet packet = new Packet( buffer );
                    DispatchPacket( packet );

                }
            }

        }

        public static void DispatchPacket( Packet p ) {

            switch ( p.type ) {
                case PacketType.Chat:

                    Console.WriteLine( "Message recived from: " + p.data[0] );
                    Console.WriteLine( "Retransmiting..." );

                    foreach(ClientData client in clients) {
                        if( client.id != p.senderID )
                            client.socket.Send( p.ToBytes() );
                    }

                    break;
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
    class ClientData {

        public Socket socket;
        public Thread thread;
        public string id;


        public ClientData() {
            id = Guid.NewGuid().ToString();
            thread = new Thread( Server.GetPacket );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public ClientData( Socket clientScoket ) {
            this.socket = clientScoket;
            id = Guid.NewGuid().ToString();

            thread = new Thread( Server.GetPacket );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket() {
            Packet p = new Packet( PacketType.Registration, id );
            socket.Send( p.ToBytes() );
        }


    }
}
