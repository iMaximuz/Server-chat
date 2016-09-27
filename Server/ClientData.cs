using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server_Utilities;


namespace Server {
    class ClientData {

        public Socket socket;
        public Thread thread;
        public string id;

        public ClientData( ParameterizedThreadStart ts, Socket clientScoket ) {
            this.socket = clientScoket;
            id = Guid.NewGuid().ToString();
            thread = new Thread( ts );
            thread.Start( this );
        }

        public void Disconnect() {
            thread.Abort();
            socket.Shutdown( SocketShutdown.Both );
            socket.Close();
        }

    }
}
