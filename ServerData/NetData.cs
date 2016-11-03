using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server_Utilities {
    public static class NetData {
        public static int TCP_PORT = 4040;
        public static int UDP_PORT = 4041;
        public static int UDP_CLIENT_PORT = 4042;
        public static IPAddress localhost = IPAddress.Parse("127.0.0.1");
        public static IPAddress remotehost = IPAddress.Parse("192.168.0.109");
        public static string GetIP4Address() {

            IPAddress[] ips = Dns.GetHostAddresses( Dns.GetHostName() );

            foreach (IPAddress i in ips) {

                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return i.ToString();
            }

            return "127.0.0.1";
        }

    }

    public class UdpState {
        UdpClient client;
        IPEndPoint endPoint;
    }

}
