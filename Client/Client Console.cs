using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ServerData;

namespace Client
{


    class Console {

        static Socket socket;
        public static string name;
        public static int chatColor;
        public static string id = "";
        public static bool connected = false;


        static void Main( string[] args ) {

            //TODO: Mejorar la estrucutra del main
            socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            IPAddress ipAddress = IPAddress.Any;

            System.Console.Write( "Enter username: \n>" );
            name = System.Console.ReadLine();
            System.Console.Write( "Enter color id: \n>" );
            chatColor = int.Parse( System.Console.ReadLine());
            bool validIp = true;
            do {

                System.Console.Write( "Enter host IP address: \n>" );
                string ip = System.Console.ReadLine();
                try {
                    ipAddress = IPAddress.Parse( ip );
                    validIp = true;
                }
                catch (FormatException ex) {
                    System.Console.WriteLine( ex.Message );
                    validIp = false;
                    
                }

            } while (!validIp);


            IPEndPoint endPoint = new IPEndPoint( ipAddress, NetData.PORT );

            int connAttempts = 0;

            while (connAttempts < 10) {

                try {
                    connAttempts++;
                    socket.Connect( endPoint );
                    connected = true;
                    break;
                }
                catch (SocketException ex) {
                    System.Console.WriteLine( "Could not connect to host! Attempt... {0}", connAttempts );
                    if (connAttempts == 10) {
                        System.Console.WriteLine( "There was an error when trying to connect: " + ex.Message );
                        Thread.Sleep( 2000 );
                    }
                }

            }

            if (connected) {

                Thread thread = new Thread( GetPacket );
                thread.Start();

                System.Console.WriteLine( "Connected to server..." );
                System.Console.WriteLine( "Joining chat room..." );
                
                // Wait so we can get an id back from the server
                Thread.Sleep( 500 );

                while (true) {
                    Packet chatPacket = new Packet( PacketType.Chat, id );

                    //Console.Write( ">" );
                    string message = System.Console.ReadLine();

                    chatPacket.data.Add( name );
                    chatPacket.data.Add( message );
                    chatPacket.packetInt = chatColor;

                    socket.Send( chatPacket.ToBytes() );
                }
            }
        }

        static void GetPacket() {

            byte[] buffer;
            int readBytes;

            while (true) {
                buffer = new byte[socket.ReceiveBufferSize];
                readBytes = socket.Receive( buffer );

                if( readBytes > 0) {
                    Packet packet = new Packet( buffer );
                    DispatchPacket( packet );
                }
            }

        }

        static void DispatchPacket( Packet p ) {

            switch(p.type) {
                case PacketType.Registration:
                    System.Console.WriteLine( "Registration completed with server.\nClient Id Received... ");
                    id = p.senderID;
                    break;
                case PacketType.Chat:
                    {
                        ConsoleColor c = System.Console.ForegroundColor;
                        System.Console.ForegroundColor = (ConsoleColor)p.packetInt;

                        System.Console.WriteLine( "{0}: {1}", p.data[0], p.data[1] );
                        System.Console.ForegroundColor = c;
                    }
                    break;
                    
                    
            }

        }

    }

}
