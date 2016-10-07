using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Utilities {
    [Serializable]
    public class ChatRoom {
        public int id;
        public string name;
        public ChatRoom(string name ) {
            this.id = 0;
            this.name = name;
        }
        public ChatRoom(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }
}
