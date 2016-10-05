using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Utilities {

    public enum State{
        Offline,
        Online,
        Busy,
        Away,
    }

    public class ClientState {

        public State state = State.Offline;
        public string username;

    }
}
