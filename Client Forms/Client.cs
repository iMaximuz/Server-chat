using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server_Utilities;
using System.Diagnostics;

namespace Client_Forms {
   public class Client {

        string chatName;
        public string ID;

        public bool isConnected = false;
        public bool attemtingConnection = false;


        Socket connectionSocket;

        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        int port;

        Thread receiveThread;

        public Action OnConnect;
        public Action<string> OnError;
        public Action<string> OnConnectionFail;

        public Action<Packet> OnPacketReceived;

        public Action OnServerDisconnect;
        public Action OnDisconnect;


        public Client() {
            this.chatName = "Client";
        }

        public Client( string name ) {
            this.chatName = name;
        }

        public void Connect( IPAddress hostIP, int port ) {

            this.hostIPAddress = hostIP;
            this.port = port;
            this.hostAddress = new IPEndPoint( hostIP, port );

            AttemptConnection connect = new AttemptConnection( ConnectToServer );

            if (!isConnected) {
                if (!attemtingConnection) {
                    
                    connect.BeginInvoke( null, null );
                }
            }

        }

        delegate void AttemptConnection();
        void ConnectToServer() {
            if (!isConnected) {
                attemtingConnection = true;
                connectionSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
                // Try to connect to host
                int connAttempts = 0;

                while (connAttempts < 10) {

                    if (!attemtingConnection) {
                        break;
                    }

                    try {

                        connAttempts++;
                        connectionSocket.Connect( hostAddress );
                        isConnected = true;
                        break;
                    }
                    catch (SocketException ex) {

                        OnError( "Could not connect to host... Attempt " + connAttempts.ToString() );

                        if (connAttempts == 10)
                            OnConnectionFail( ex.Message );
                    }
                }

                if (isConnected) {
                    receiveThread = new Thread( ReadThread );
                    receiveThread.Start();
                }
                attemtingConnection = false;
            }
        }

        public void Disconnect() {
            if (isConnected) {

                Packet packet = new Packet( PacketType.Client_LogOut, ID );
                packet.data.Add( "name" , chatName );

                //TODO: Change it to a Queue
                SendPacket( packet );

                ShutdownClient();
            }
        }


        //TODO: Maybe not have the client info here?
        void ChangeName( string newName ) {
            this.chatName = newName;
        }

        public void SendPacket(Packet p) {
            
            connectionSocket.Send( PacketFormater.Format( p ) );

        }

        void ReadThread() {
            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int readBytes;

            try {
                while (true) {

                    readBytes = connectionSocket.Receive( buffer );
                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize( buffer );

                        if (packetSize == readBytes - sizeof( int )) {
                            Packet packet = PacketFormater.MakePacket( buffer );
                            DefaultDispatchPacket( packet );
                        }
                    }
                    else {
                        Debug.Assert( true );
                        break;
                    }

                }
            }
            catch (SocketException ex) {
                if (isConnected) {
                    OnServerDisconnect();
                    ShutdownClient();
                }
            }
            catch (ObjectDisposedException ex) {
                OnDisconnect();
            }

        }

        void ShutdownClient() {
            isConnected = false;
            connectionSocket.Shutdown( SocketShutdown.Both );
            connectionSocket.Close();
            OnDisconnect();
        }

        void DefaultDispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Server_Registration: {

                        ID = p.senderID;
                        //WriteLine( "Connected to server." );
                        //WriteLine( "Client id recieved: " + ID );

                        break;
                    }
                case PacketType.Server_Closing: {
                        ShutdownClient();
                        //TODO: Poder cambiar el texto del boton desde cualquier thread
                        //btnConnect.Text = "Connect";
                        break;
                    }
                default:
                    OnPacketReceived( p );
                    break;
            }

        }

    }
}
