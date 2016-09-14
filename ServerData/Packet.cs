using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.IO;

namespace ServerData {
    [Serializable]
    public class Packet {
        //public Dictionary<string, object> data;
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
            MemoryStream ms = new MemoryStream( packetBytes );

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
    }

    public enum PacketType {
        Server_Registration,
        Server_Closing,
        Client_LogOut,
        Client_LogIn,
        Ping,
        Pong,
        Chat,
        Chat_Buzzer
    }
}
