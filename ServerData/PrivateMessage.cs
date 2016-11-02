using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Utilities {
    [Serializable]
    public class PrivateMessage {

        public int senderID;
        public int receiverID;
        public string message;
        public bool encrypted;
        public DateTime date;

        public PrivateMessage( int senderID, int receiverID, string message, bool encrypted, DateTime date ) {
            this.senderID = senderID;
            this.receiverID = receiverID;
            this.message = message;
            this.encrypted = encrypted;
            this.date = date;
        }
    }
}
