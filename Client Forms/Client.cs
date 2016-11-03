﻿using System;
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
        Socket udpSocket;

        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        int port;


        public Action OnConnect;
        public Action<string> OnError;
        public Action<string> OnConnectionFail;

        public Action<Packet> OnPacketReceived;

        public Action OnServerDisconnect;
        public Action OnDisconnect;


        Thread receiveThread;
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
                udpSocket = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
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
/*
        void ReadThread() {
            //connectionSocket.ReceiveBufferSize = 1500;
            byte[] buffer = new byte[connectionSocket.ReceiveBufferSize];
            int readBytes;

            try {
                while (true) {

                    readBytes = connectionSocket.Receive(buffer);

                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize(buffer);

                        if (packetSize == readBytes - sizeof(int)) {
                            Packet packet = PacketFormater.MakePacket(buffer);
                            DefaultDispatchPacket(packet);
                        }
                        else if(packetSize > readBytes - sizeof( int )){

                            int totalBytes = readBytes;
                            int fullBufferSize = packetSize + sizeof(int);
                            byte[] fullPacketBuffer = new byte[fullBufferSize];
                            MemoryStream ms = new MemoryStream(fullPacketBuffer);
                            ms.Write(buffer, 0, readBytes);

                            while (totalBytes < fullBufferSize) {
                                readBytes = connectionSocket.Receive(buffer);
                                if (readBytes + totalBytes <= fullBufferSize) {
                                    totalBytes += readBytes;
                                    ms.Write( buffer, 0, readBytes );
                                }
                                else {

                                }
                            }

                            ms.Close();

                            Packet packet = PacketFormater.MakePacket(fullPacketBuffer);
                            DefaultDispatchPacket(packet);
                        } else if( packetSize < readBytes - sizeof( int )) {

                            int totalBytes = readBytes;
                            MemoryStream ms = null;
                            Packet packet = null;
                            while (totalBytes > 0) {
                                packetSize = PacketFormater.GetPacketSize( buffer );
                                int packetSection = packetSize + sizeof( int );
                                byte[] fullPacketBuffer = new byte[packetSection];

                                if (packetSection <= totalBytes) {
                                    

                                    ms = new MemoryStream( fullPacketBuffer );
                                    ms.Write( buffer, 0, packetSection );
                                    ms.Close();

                                    packet = PacketFormater.MakePacket( fullPacketBuffer );
                                    DefaultDispatchPacket( packet );

                                    totalBytes -= packetSection;

                                    Array.Copy( buffer, packetSection, buffer, 0, buffer.Length - packetSection );
                                }
                                else {

                                    //totalBytes = readBytes;
                                    int residualBytes = totalBytes;

                                    ms = new MemoryStream( fullPacketBuffer );
                                    ms.Write( buffer, 0, residualBytes );

                                    while (totalBytes < packetSection) {
                                        readBytes = connectionSocket.Receive( buffer );
                                        totalBytes += readBytes;
                                        ms.Write( buffer, 0, readBytes );
                                    }

                                    ms.Close();

                                    packet = PacketFormater.MakePacket( fullPacketBuffer );
                                    DefaultDispatchPacket( packet );

                                }

                            }

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
*/
        void ShutdownClient() {
            SendPacket( null );
            sendThread.Join();
            waitHandler.Close();
            isConnected = false;
            connectionSocket.Shutdown(SocketShutdown.Both);
            connectionSocket.Close();
            receiveThread.Abort();

            OnDisconnect();
        }

        void DefaultDispatchPacket(Packet p) {

            switch (p.type) {
                case PacketType.Server_Registration: {
                        ID = p.senderID;
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
