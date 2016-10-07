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
using System.IO;

namespace Client_Forms {
   public class Client {

        string chatName;
        public string ID;
        string filePath = KnownFolders.GetPath( KnownFolder.Downloads );

        public bool isConnected = false;
        public bool attemtingConnection = false;

        public ClientState sesionInfo;

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
            sesionInfo = new ClientState();
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

                        if(OnError != null)
                            OnError( "Could not connect to host... Attempt " + connAttempts.ToString() );

                        if (connAttempts == 10) {
                            if (OnConnectionFail != null)
                                OnConnectionFail( ex.Message );
                        }
                    }
                }

                if (isConnected) {
                    receiveThread = new Thread( ReadThread );
                    receiveThread.Start();
                    if(OnConnect != null)
                        OnConnect();
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
            if (isConnected) {
                connectionSocket.Send( PacketFormater.Format( p ) );
            }
            else {
                OnError( "ERROR: This client is not connect to a server." );
            }
        }

        void ReadThread() {
            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int readBytes;

            connectionSocket.ReceiveBufferSize = 200;

            try {
                while (true) {

                    readBytes = connectionSocket.Receive( buffer );
                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize( buffer );

                        if (packetSize == readBytes - sizeof( int )) {
                            Packet packet = PacketFormater.MakePacket( buffer );
                            DefaultDispatchPacket( packet );
                        }
                        else {

                            int totalBytes = readBytes;
                            int fullBufferSize = packetSize + sizeof( int );
                            byte[] fullPacketBuffer = new byte[fullBufferSize];
                            MemoryStream ms = new MemoryStream( fullPacketBuffer );
                            ms.Write( buffer, 0, buffer.Length );

                            while (totalBytes < fullBufferSize) {
                                readBytes = connectionSocket.Receive( buffer );
                                totalBytes += readBytes;
                                ms.Write( buffer, 0, readBytes );
                            }

                            ms.Close();

                            Packet packet = PacketFormater.MakePacket( fullPacketBuffer );
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


        public string GetDownloadFilePath() {
            return filePath + "\\";
        }

    }
}
