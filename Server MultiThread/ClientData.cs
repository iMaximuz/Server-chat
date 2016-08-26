using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ServerData;


namespace Server {
    class ClientData {

        public Socket socket;
        public Thread thread;
        public string id;

        public ClientData( ServerManager sm, Socket clientScoket ) {
            this.socket = clientScoket;
            id = Guid.NewGuid().ToString();
            thread = new Thread( sm.ReadMessageThread );
            thread.Start( this );
            //TODO: Manejar este mensaje en otra parte?
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket() {
            Packet p = new Packet( PacketType.Server_Registration, id );
            socket.Send( p.ToBytes() );
        }

        public void Disconnect() {
            thread.Abort();
            socket.Shutdown( SocketShutdown.Both );
            socket.Close();
        }

    }
}
