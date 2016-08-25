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
        static List<Client> clients;
        public static IPEndPoint serverAddress;
        static bool online = false;
        static Thread listenThread;

        static void Main( string[] args ) {

            // Setting server up
            serverSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            clients = new List<Client>();

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
            try {
                serverSocket.Bind( serverAddress );
                online = true;
                listenThread = new Thread( ListenThread );
                listenThread.Start();
                Console.WriteLine( "Server started" );
            }
            catch (SocketException ex){
                Console.WriteLine( "ERROR: Server could not start. \n\t" + ex.Message );
            }

            while (online) {

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
                    online = false;
                    CloseAllConnections();
                    break;
            }

        }

        static void CloseAllConnections() {
            //listenThread.Abort();
            Packet p = new Packet( PacketType.Server_Closing, ServerInfo.ID );
            foreach (Client c in clients) {
                c.socket.Send( p.ToBytes() );
                c.Disconnect();
            }
            serverSocket.Close();
            listenThread.Abort();
        }

        //Listen to incomming connections
        static void ListenThread() {
            Console.WriteLine( "Listening for incomming connections..." );
            try {
                while (true) {
                    serverSocket.Listen( 0 );
                    Client client = new Client( serverSocket.Accept() );
                    clients.Add( client );
                    Console.WriteLine( "Client connected: " + client.id );
                    SendWelcomeMessage( client.socket );
                }
            }
            catch ( SocketException ex) {
                Console.WriteLine( ex.Message );
            }
            catch ( ThreadAbortException ex) {
                Console.WriteLine( "ListenThread aborted" );
            }
        }

        public static void GetPacket( object obj ) {
            Socket clientSocket = (Socket)obj;

            byte[] buffer;
            int readBytes;
            try {
                while (true) {

                    buffer = new byte[clientSocket.SendBufferSize];
                    readBytes = clientSocket.Receive( buffer );

                    if (readBytes > 0) {
                        //Make a packet from serialized array of bytes
                        Packet packet = new Packet( buffer );
                        DispatchPacket( packet );

                    }
                    else {
                        break;
                    }
                }
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Client thread aborted" );
            }
        }

        public static void DispatchPacket( Packet p ) {

            switch ( p.type ) {
                case PacketType.Chat:

                    Console.WriteLine( "Message recived from: " + p.data[0] );
                    Console.WriteLine( "Retransmiting..." );

                    foreach(Client client in clients) {
                        if( client.id != p.senderID )
                            client.socket.Send( p.ToBytes() );
                    }

                    break;
                case PacketType.Client_LogOut: {

                        Console.WriteLine( "Client disconnected: " + p.senderID );

                        clients.RemoveAll( x => x.id == p.senderID );
                        foreach (Client client in clients) {
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
    class Client {

        public Socket socket;
        public Thread thread;
        public string id;


        public Client() {
            id = Guid.NewGuid().ToString();
            thread = new Thread( Server.GetPacket );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public Client( Socket clientScoket ) {
            this.socket = clientScoket;
            id = Guid.NewGuid().ToString();

            thread = new Thread( Server.GetPacket );
            thread.Start( socket );
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket() {
            Packet p = new Packet( PacketType.Server_Registration, id );
            socket.Send( p.ToBytes() );
        }

        public void Disconnect() {
            thread.Abort();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
