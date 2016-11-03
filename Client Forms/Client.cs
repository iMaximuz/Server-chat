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

        public string ID;
        string filePath = KnownFolders.GetPath(KnownFolder.Downloads);

        public bool isConnected = false;
        public bool attemtingConnection = false;
        public bool isLoggedIn = false;
        public bool hasCamera = true;


        public ClientState sesionInfo;

        Socket connectionSocket;
        UdpClient udpSocket;
        IPEndPoint UdpEndpoint;

        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        int port;


        public Action OnConnect;
        public Action<string> OnError;
        public Action<string> OnConnectionFail;

        public Action<Packet> OnPacketReceived;
        public Action<UdpPacket> OnUdpPacketReceived;
        public Action OnServerDisconnect;
        public Action OnDisconnect;


        Thread receiveThread;
        Thread UdpReceiveThread;
        Thread sendThread;
        readonly object locker = new object();
        Queue<Packet> messageQueue;
        //Mutex queueMutex;
        EventWaitHandle waitHandler = new AutoResetEvent( false );

        public Client() {
            sesionInfo = new ClientState("N/A");
        }

        public Client(string username) {
            sesionInfo = new ClientState(username);
        }

        public void Connect(IPAddress hostIP, int port) {

            messageQueue = new Queue<Packet>();
            //queueMutex = new Mutex();

            this.hostIPAddress = hostIP;
            this.port = port;
            this.hostAddress = new IPEndPoint(hostIP, port);

            AttemptConnection connect = new AttemptConnection(ConnectToServer);

            if (!isConnected) {
                if (!attemtingConnection) {

                    connect.BeginInvoke(null, null);
                }
            }

        }

        delegate void AttemptConnection();
        void ConnectToServer() {
            if (!isConnected) {
                attemtingConnection = true;
                connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                bool validPort = false;
                int increment = 0;
                while (!validPort) {
                    try {
                        udpSocket = new UdpClient( NetData.UDP_CLIENT_PORT + increment );
                        validPort = true;
                    }
                    catch (SocketException ex) {
                        increment++;
                    }
                }
                //udpSocket = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
                // Try to connect to host
                int connAttempts = 0;

                while (connAttempts < 10) {

                    if (!attemtingConnection) {
                        break;
                    }

                    try {

                        connAttempts++;
                        connectionSocket.Connect(hostAddress);
                        isConnected = true;
                        break;
                    }
                    catch (SocketException ex) {

                        if (OnError != null)
                            OnError("Could not connect to host... Attempt " + connAttempts.ToString());

                        if (connAttempts == 10) {
                            if (OnConnectionFail != null)
                                OnConnectionFail(ex.Message);
                        }
                    }
                }

                if (isConnected) {
                    UdpEndpoint = new IPEndPoint( hostIPAddress, NetData.UDP_PORT );
                    udpSocket.Connect( UdpEndpoint );

                    UdpReceiveThread = new Thread( UdpReadThread );
                    UdpReceiveThread.Name = "Worker UDP receive";
                    UdpReceiveThread.Start();

                    receiveThread = new Thread(ReadThread);
                    receiveThread.Name = "Worker receive";
                    receiveThread.Start();
                    sendThread = new Thread(SendThread);
                    sendThread.Name = "Worker send";
                    sendThread.Start();
                    if (OnConnect != null)
                        OnConnect();
                }
                attemtingConnection = false;
            }
        }

        public void Disconnect() {
            if (isConnected) {

                Packet packet = new Packet(PacketType.Client_LogOut, ID);
                packet.data.Add("username", sesionInfo.username);

                //TODO: Change it to a Queue
                SendPacket(packet);

                ShutdownClient();
            }
        }

        public void SendPacket(Packet p) {
            if (isConnected) {
                //queueMutex.WaitOne();

                lock (locker) {
                    messageQueue.Enqueue( p );
                }
                waitHandler.Set();
                
                //connectionSocket.Send(PacketFormater.Format(p));
                //queueMutex.ReleaseMutex();
            }
            else {
                OnError("ERROR: This client is not connect to a server.");
            }
        }
        public void SendUdpPacket(UdpPacket p) {
            byte[] packetBytes = p.ToBytes();
            udpSocket.Send( packetBytes, packetBytes.Length );
        }

        void SendThread() {
            try {

                while (true) {
                    Packet packet = null;
                    lock (locker) {
                        if (messageQueue.Count > 0) {
                            packet = messageQueue.Dequeue();
                            if (packet == null) return;
                        }
                    }
                    if (packet != null) {
                        connectionSocket.Send( PacketFormater.Format( packet ) );
                        Thread.Sleep( 10 );
                    }
                    else
                        waitHandler.WaitOne();
                }
            }
            catch (ThreadAbortException ex) {

            }
        }


        byte[] ReadSocketStream() {

            byte[] result = null;

            byte[] rawBuffer = new byte[connectionSocket.ReceiveBufferSize];
            int readBytes = connectionSocket.Receive( rawBuffer );

            if (readBytes > 0) {
                result = new byte[readBytes];
                Array.Copy( rawBuffer, result, readBytes );
            }

            return result;
        }

        void ReadAndExecutePacket(ref MemoryStream fullData) {
            int packetSection = PacketFormater.GetPacketSize( fullData.ToArray() ) + sizeof( int );
            
            while(packetSection > fullData.Length) {
                byte[] newData = ReadSocketStream();
                fullData.Write( newData, 0, newData.Length );
            }

            byte[] packetBuffer = new byte[packetSection];

            fullData.Seek( 0, SeekOrigin.Begin );
            fullData.Read( packetBuffer, 0, packetSection );
            Packet packet = PacketFormater.MakePacket( packetBuffer );

            DefaultDispatchPacket( packet );

            int extraBytes = (int)fullData.Length - packetSection;
            byte[] extraBuffer = new byte[extraBytes];
            fullData.Read( extraBuffer, 0, extraBytes );
            fullData.Close();
            fullData = new MemoryStream();
            fullData.Write( extraBuffer, 0, extraBytes );
        }

        void ReadThread() {

            MemoryStream dataStream = new MemoryStream();

            try {
                while (true) {

                    byte[] incomingData = ReadSocketStream();
                    if (incomingData != null) {
                        dataStream.Write( incomingData, 0, incomingData.Length );
                        while (dataStream.Length > 0) {
                            ReadAndExecutePacket( ref dataStream );
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

            dataStream.Close();
        }

        void UdpReadThread() {
            try {
                while (true) {
                    byte[] receivedData = udpSocket.Receive( ref UdpEndpoint );
                    UdpPacket packet = UdpPacket.CreateFromStream( receivedData );
                    OnUdpPacketReceived( packet );
                }
            }
            catch(ThreadAbortException ex) {

            }
        }

        void ShutdownClient() {
            SendPacket( null );
            sendThread.Join();
            waitHandler.Close();
            isConnected = false;
            connectionSocket.Shutdown(SocketShutdown.Both);
            connectionSocket.Close();
            UdpReceiveThread.Abort();
            receiveThread.Abort();
            OnDisconnect();
        }

        void DefaultDispatchPacket(Packet p) {

            switch (p.type) {
                case PacketType.Server_Registration: {
                        ID = p.senderID;

                        UdpPacket udpRegistration = new UdpPacket( UdpPacketType.Client_Registration );
                        udpRegistration.WriteData( Encoding.ASCII.GetBytes( ID ) );
                        SendUdpPacket( udpRegistration );
                        break;
                    }
                case PacketType.Server_Closing: {
                        ShutdownClient();
                        break;
                    }
                default:
                    OnPacketReceived(p);
                    break;
            }

        }


        public string GetDownloadFilePath() {
            return filePath + "\\";
        }

    }





}
