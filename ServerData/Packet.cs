using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.IO;

namespace ServerData
{
    [Serializable]
    public class Packet
    {
        public List<string> data;
        public int packetInt;
        public bool packetBool;
        public string senderID;
        public PacketType type;

        public Packet( PacketType type, string senderID ) {
            data = new List<string>();
            this.senderID = senderID;
            this.type = type;
        }

        public Packet( byte[] packetBytes ) { 
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetBytes);

            Packet p = (Packet)bf.Deserialize( ms );
            ms.Close();

            this.data = p.data;
            this.packetInt = p.packetInt;
            this.packetBool = p.packetBool;
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

        public static int PORT = 4040;
        public static string GetIP4Address() {

            IPAddress[] ips = Dns.GetHostAddresses( Dns.GetHostName() );

            foreach( IPAddress i in ips) {

                if(i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                    return i.ToString();
            }

            return "127.0.0.1";
        }

    }

    public enum PacketType {
        Registration,
        Chat
    }
}
