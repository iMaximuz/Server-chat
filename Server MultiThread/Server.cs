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
    class Server {

        static Socket serverSocket;
        static List<ClientData> clients;

        

        static void Main( string[] args ) {

            // Setting server up
            Console.WriteLine( "Starting server on " + Packet.GetIP4Address() );
            serverSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            clients = new List<ClientData>();

            IPEndPoint ip = new IPEndPoint( IPAddress.Parse( Packet.GetIP4Address() ), Packet.PORT );
            serverSocket.Bind(ip);

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
            }
        }

        public static void ReadData( object obj ) {
            Socket clientSocket = (Socket)obj;

            byte[] buffer;
            int readBytes;

            while (true) {

                buffer = new byte[clientSocket.SendBufferSize];
                readBytes = clientSocket.Receive( buffer );

                if(readBytes > 0) { 
                    //Make a packet from serialized array of bytes
                    Packet packet = new Packet( buffer );
                    DataManager( packet );

                }
            }

        }

        public static void DataManager( Packet p ) {

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


    }
    class ClientData {

        public Socket socket;
        public Thread thread;
        public string id;


        public ClientData() {
            id = Guid.NewGuid().ToString();
            thread = new Thread( Server.ReadData );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public ClientData( Socket clientScoket ) {
            this.socket = clientScoket;
            id = Guid.NewGuid().ToString();

            thread = new Thread( Server.ReadData );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket() {
            Packet p = new Packet( PacketType.Registration, id);

            socket.Send( p.ToBytes() );

        }
    }
}
