using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.IO;

namespace Server_Utilities {
    [Serializable]
    public class Packet {
        public Dictionary<string, object> data;
        public string senderID;
        public PacketType type;

        public Packet( PacketType type, string senderID ) {
            data = new Dictionary<string, object>();
            this.senderID = senderID;
            this.type = type;
        }

        public Packet( byte[] packetBytes ) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream( packetBytes );

            Packet p = (Packet)bf.Deserialize( ms );
            ms.Close();

            this.data = p.data;
            this.senderID = p.senderID;
            this.type = p.type;
        }

        public byte[] ToBytes() {

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize( ms, this );
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
    }

    public class PacketFormater {

        //Returns an array of bytes containing the size of the packet in the first 4 bytes and the serealized packet after them
        static public byte[] Format(Packet p) {

            byte[] packetBytes = p.ToBytes();
            byte[] packetSize = BitConverter.GetBytes( packetBytes.Length );
            byte[] buffer = new byte[packetBytes.Length + packetSize.Length];

            MemoryStream ms = new MemoryStream( buffer );

            ms.Write( packetSize, 0, 4 );
            ms.Write( packetBytes, 0, packetBytes.Length );

            ms.Close();

            return buffer;
        }

        //Returns the size of the packet contained in an array of bytes
        static public int GetPacketSize(byte[] packet) {
            int size = BitConverter.ToInt32( packet, 0 );
            return size;
        }

        //Returns the packet from Formated array of bytes
        static public Packet MakePacket(byte[] packet) {
            Packet result;
            int size = GetPacketSize( packet );
            byte[] buffer = new byte[size];

            Array.Copy( packet, 4, buffer, 0, size );

            result = new Packet( buffer );

            return result;
        }

    }

    public class UdpPacket {
        private MemoryStream data;
        private byte[] dataBytes;
        private UdpPacketType packetType;
        public UdpPacketType PacketType {
            get { return packetType; }
        }
        public byte[] Data {
            get {
                if (dataBytes == null) {
                    dataBytes = data.ToArray();
                }
                return dataBytes;
            }
        }

        public byte[] ReadData( int lenght, int position ) { return dataBytes.Skip( position ).Take( lenght ).ToArray(); }
        public byte[] ReadData( int lenght ) {
            byte[] readData = new byte[lenght];
            data.Read( readData, 0, lenght );
            return readData;
        }
        public int ReadInt( int position ) { return BitConverter.ToInt32( dataBytes, position ); }
        public int ReadInt() {
            byte[] integer = new byte[4];
            data.Read( integer, 0, 4 );
            return BitConverter.ToInt32( integer, 0 );
        }
        public void WriteData( byte[] bytes ) { data.Write( bytes, 0, bytes.Length ); }
        public void WriteData( byte[] bytes, int offset ) { data.Write( bytes, offset, bytes.Length - offset ); }

        public UdpPacket( UdpPacketType packetType ) {
            data = new MemoryStream();
            this.packetType = packetType;
        }
        protected UdpPacket( UdpPacketType packetType, byte[] bytes ) {
            dataBytes = bytes;
            data = new MemoryStream( dataBytes );
            data.Position = 0;
            this.packetType = packetType;
        }
        public byte[] ToBytes() {
            using (MemoryStream stream = new MemoryStream()) {
                stream.Write( BitConverter.GetBytes( (int)packetType ), 0, sizeof( int ) );
                if (dataBytes == null) {
                    byte[] bytes = data.ToArray();
                    stream.Write( bytes, 0, bytes.Length );
                }
                else {
                    stream.Write( dataBytes, 0, dataBytes.Length );
                }
                return stream.ToArray();
            }
        }
        public static UdpPacket CreateFromStream( byte[] bytes ) {
            UdpPacketType packetType = (UdpPacketType)BitConverter.ToInt32( bytes, 0 );
            UdpPacket packet = new UdpPacket( packetType, bytes.Skip( 4 ).ToArray() );
            return packet;
        }
    }

    public enum PacketType {
        Server_Registration,
        Server_Closing,
        Client_SignIn,
        Client_LogOut,
        Client_LogIn,
        User_LogIn,
        User_LogOut,
        User_Status_Change,
        Ping,
        Pong,
        ChatRoom_Create,
        ChatRoom_Remove,
        ChatRoom_Join,
        ChatRoom_Leave,
        Chat,
        Chat_Private,
        Chat_Buzzer,
        Chat_Buzzer_Private,
        Chat_File,
        Video,
        Video_Confirmation,
        Load_Private_Chat,
        Audio_SetUp,
        Audio_Stop
    }

    public enum UdpPacketType {
        Client_Registration,
        Audio,
        Game_Start,
        Game_End,
        Game_Restart,
        Game_Cursor,
        Game_Click
    }

}
