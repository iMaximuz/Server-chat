using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ServerData;


namespace Server {
    class ServerManager {

        //TODO: Manejo de errores

        public List<ClientData> clients;
        public bool isOnline = false;

        Socket listenSocket;
        Thread listenThread;
        IPEndPoint address;

        public Action OnStarUp;
        public Action<ClientData> OnClientConnect;
        public Action<Packet> OnReceive;
        public Action<ClientData> OnClientDisconnect;
        public Action OnShutdown;

        public ServerManager( IPEndPoint serverAddress ) {
            this.address = serverAddress;
        }

        public void Start() {
            if (!isOnline) {
                listenSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
                clients = new List<ClientData>();

                try {
                    listenSocket.Bind( address );
                    isOnline = true;
                    listenThread = new Thread( ListenThread );
                    listenThread.Start();
                    OnStarUp();
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
                    ClientData client = new ClientData( ReadMessageThread, listenSocket.Accept() );
                    clients.Add( client );
                    OnClientConnect( client );
                }
            }
            catch (SocketException ex) {
                Console.WriteLine( ex.Message );
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "ListenThread aborted" );
            }
        }

        public void ReadMessageThread( object obj ) {
            ClientData client = (ClientData)obj;
            Socket socket = client.socket;

            byte[] buffer;
            int readBytes;
            try {
                while (true) {

                    buffer = new byte[socket.SendBufferSize];
                    readBytes = socket.Receive( buffer );

                    if (readBytes > 0) {
                        //Make a packet from serialized array of bytes
                        Packet packet = new Packet( buffer );
                        OnReceive( packet );

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

        void CloseAllConnections() {
            Packet p = new Packet( PacketType.Server_Closing, ServerInfo.ID );
            foreach (ClientData c in clients) {
                c.socket.Send( p.ToBytes() );
                c.Disconnect();
            }
            listenSocket.Close();
            listenThread.Abort();
        }

    }
}
