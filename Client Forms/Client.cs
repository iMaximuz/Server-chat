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

        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        int port;

        Thread receiveThread;
        Thread sendThread;

        public Action OnConnect;
        public Action<string> OnError;
        public Action<string> OnConnectionFail;

        public Action<Packet> OnPacketReceived;

        public Action OnServerDisconnect;
        public Action OnDisconnect;

        Queue<Packet> messageQueue;

        public Client() {
            sesionInfo = new ClientState("N/A");
        }

        public Client(string username) {
            sesionInfo = new ClientState(username);
        }

        public void Connect(IPAddress hostIP, int port) {

            messageQueue = new Queue<Packet>();

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
                    receiveThread = new Thread(ReadThread);
                    receiveThread.Start();
                    sendThread = new Thread(SendThread);
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

                messageQueue.Enqueue(p);

                connectionSocket.Send(PacketFormater.Format(p));
            }
            else {
                OnError("ERROR: This client is not connect to a server.");
            }
        }

        void SendThread()
        {
            try
            {

                while (true)
                {
                    if(messageQueue.Count > 0)
                    {
                        connectionSocket.Send(PacketFormater.Format(messageQueue.Dequeue()));
                        Thread.Sleep(12);
                    }
                    else { Thread.Sleep(50); }
                }
            }
            catch (ThreadAbortException ex)
            {

            }
        }
        void ReadThread() {
            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int readBytes;

            connectionSocket.ReceiveBufferSize = 200;

            try {
                while (true) {

                    readBytes = connectionSocket.Receive(buffer);
                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize(buffer);

                        if (packetSize == readBytes - sizeof(int)) {
                            Packet packet = PacketFormater.MakePacket(buffer);
                            DefaultDispatchPacket(packet);
                        }
                        else {

                            int totalBytes = readBytes;
                            int fullBufferSize = packetSize + sizeof(int);
                            byte[] fullPacketBuffer = new byte[fullBufferSize];
                            MemoryStream ms = new MemoryStream(fullPacketBuffer);
                            ms.Write(buffer, 0, buffer.Length);

                            while (totalBytes < fullBufferSize) {
                                readBytes = connectionSocket.Receive(buffer);
                                totalBytes += readBytes;
                                ms.Write(buffer, 0, readBytes);
                            }

                            ms.Close();

                            Packet packet = PacketFormater.MakePacket(fullPacketBuffer);
                            DefaultDispatchPacket(packet);
                        }
                    }
                    else {
                        Debug.Assert(true);
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
            connectionSocket.Shutdown(SocketShutdown.Both);
            connectionSocket.Close();
            sendThread.Abort();
            OnDisconnect();
        }

        void DefaultDispatchPacket(Packet p) {

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
                    OnPacketReceived(p);
                    break;
            }

        }


        public string GetDownloadFilePath() {
            return filePath + "\\";
        }

    }





}
