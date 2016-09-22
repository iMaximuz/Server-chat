﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server_Utilities;


namespace Server {
    class ServerManager {

        //TODO: Manejo de errores

        public List<ClientData> clients;
        public bool isOnline = false;

        Socket listenSocket;

        Thread listenThread;
        Thread sendThread;
        IPEndPoint address;

        public Action OnStarUp;
        public Action<ClientData> OnClientConnect;
        public Action<Packet> OnReceive;
        public Action<ClientData> OnClientDisconnect;
        public Action OnShutdown;

        class Message {
            public Socket socket;
            public Packet packet;
            public bool toEveryOne;
            public Message() { }
            public Message(Socket s, Packet p, bool toEveryOne = false) {
                socket = s;
                packet = p;
                this.toEveryOne = toEveryOne;
            }
        }

        Queue<Message> messageQueue;

        public ServerManager( IPEndPoint serverAddress ) {
            this.address = serverAddress;
            messageQueue = new Queue<Message>();
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
                    sendThread = new Thread( SendThread );
                    sendThread.Start();
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
                    ClientData client = new ClientData( ClientThread, listenSocket.Accept() );
                    clients.Add( client );
                    OnClientConnect( client );
                }
            }
            catch (SocketException ex) {
                Console.WriteLine( ex.Message );
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Listen Thread aborted" );
            } 
        }

        void SendThread() {

            try {
                while (true) {
                    Message m = new Message();
                    if (messageQueue.Count > 0) {
                        m = messageQueue.Dequeue();
                        if (!m.toEveryOne) {
                            //Verificar si el cliente aun sigue connectado
                            if (m.socket != null) {
                                m.socket.Send( PacketFormater.Format( m.packet ) );
                            }
                        }
                        else {
                            foreach (ClientData client in clients) {
                                client.socket.Send( PacketFormater.Format( m.packet ) );
                            }
                        }
                        Thread.Sleep( 10 );
                    }
                    else {
                        Thread.Sleep( 50 );
                    }
                }
            }
            catch (ThreadAbortException ex) {
                Console.WriteLine( "Send Thread aborted" );
            }
        }


        public void ClientThread( object obj ) {
            ClientData client = (ClientData)obj;
            Socket socket = client.socket;

            byte[] buffer;
            int readBytes;
            try {
                while (true) {

                    buffer = new byte[socket.ReceiveBufferSize];
                    readBytes = socket.Receive( buffer );

                    if (readBytes > 0) {

                        int packetSize = PacketFormater.GetPacketSize( buffer );

                        if( packetSize == readBytes - sizeof( int ) ) {
                            Packet packet = PacketFormater.MakePacket( buffer );
                            OnReceive( packet );
                        }


                        

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

        //SendPacket mandará el packete a la fila para ser enviado posteriormente
        public void SendPacket( ClientData dest, Packet packet ) {
            messageQueue.Enqueue( new Message( dest.socket, packet ) );
        }

        public void SendPacket( Socket dest, Packet packet ) {
            messageQueue.Enqueue( new Message( dest, packet ) );
        }

        //SendPacket mandará el packete a todos los clientes agregandalos a la fila de envios
        public void SendPacketToAll(Packet packet ) {
            messageQueue.Enqueue( new Message(null, packet, true) );
        }

        public void RemoveClient(string id) {
            clients.RemoveAll( x => x.id == id );
        }

        void CloseAllConnections() {
            Packet p = new Packet( PacketType.Server_Closing, ServerInfo.ID );
            foreach (ClientData c in clients) {
                c.socket.Send( p.ToBytes() );
                c.Disconnect();
            }
            listenSocket.Close();
            listenThread.Abort();
            sendThread.Abort();
        }

    }
}
