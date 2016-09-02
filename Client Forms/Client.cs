using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServerData;

namespace Client_Forms {
    class Client {

        string chatName;
        public string ID;

        public bool isConnected = false;

        public Socket connectionSocket;
        
        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        int port;
        
        Thread receiveThread;

        Action OnConnect;
        Action<string> OnConnectionFail;

        Action<string> Write;
        Action<string> WriteLine;

        Action OnServerDisconnect;
        Action OnDisconnect;


        public Client( ) {
            this.chatName = "Client";
        }

        public Client(string name) {
            this.chatName = name;
        }

        public void Connect( IPAddress hostIP, int port ) {

            this.hostIPAddress = hostIP;
            this.port = port;
            this.hostAddress = new IPEndPoint( hostIP, port );

            if (!isConnected) {
                AttemptConnection connect = new AttemptConnection( ConnectToServer );
                connect.BeginInvoke( null, null );
            }

        }

        delegate void AttemptConnection();
        void ConnectToServer() {
            if (!isConnected) {

                //TODO: quitar la ip harcodeada
                hostIPAddress = NetData.localhost;

                connectionSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

                hostAddress = new IPEndPoint( hostIPAddress, port );

                // Try to connect to host
                int connAttempts = 0;

                while (connAttempts < 10) {

                    try {

                        connAttempts++;
                        connectionSocket.Connect( hostAddress );
                        isConnected = true;
                        break;
                    }
                    catch (SocketException ex) {

                        OnConnectionFail( "Could not connect to host... Attempt " + connAttempts.ToString() );

                        if (connAttempts == 10)
                            OnConnectionFail( ex.Message );
                    }
                }

                if (isConnected) {
                    receiveThread = new Thread( ReadThread );
                    receiveThread.Start();
                }
            }
        }

        public void Disconnect() {
            if (isConnected) {

                Packet packet = new Packet( PacketType.Client_LogOut, ID );
                packet.data.Add( chatName );

                //TODO: Change it to a Queue
                connectionSocket.Send( packet.ToBytes() );

                ShutdownClient();
            }
        }

        void ChangeName(string newName) {
            this.chatName = newName;
        }

        void ReadThread() {
            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int recivedData;

            try {
                while (true) {

                    recivedData = connectionSocket.Receive( buffer );
                    if (recivedData > 0) {

                        Packet packet = new Packet( buffer );
                        DispatchPacket( packet );
                    }
                }
            }
            catch (SocketException ex) {
                if (isConnected) {
                    OnServerDisconnect(); //( "ERROR 500: An existing connection was forcibly closed by the server " );
                    ShutdownClient();
                    OnDisconnect(); //  "Disconnected from server..." );
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
        }

        void DispatchPacket( Packet p ) {

            switch (p.type) {
                case PacketType.Server_Registration: {

                        ID = p.senderID;
                        WriteLine( "Connected to server." );
                        WriteLine( "Client id recieved: " + ID );
                        break;
                    }
                case PacketType.Server_Closing: {

                        WriteLine( "Server is closing..." );
                        ShutdownClient();
                        //TODO: Poder cambiar el texto del boton desde cualquier thread
                        //btnConnect.Text = "Connect";
                        break;
                    }
                case PacketType.Chat: {

                        WriteLine( p.data[0] + ": " + p.data[1] );

                        break;
                    }

                case PacketType.Client_LogOut: {
                        WriteLine( "Client disconnected: " + p.data[0] );

                        break;
                    }
            }

        }

    }
}
