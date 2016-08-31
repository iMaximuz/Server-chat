using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Client_Forms {
    class Client {
        Socket connectionSocket;
        string clientID;
        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        bool connected = false;
        Thread receiveThread;


    }
}
