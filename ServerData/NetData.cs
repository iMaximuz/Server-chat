using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;



namespace ServerData {
    public static class NetData {
        public static int PORT = 4040;
        public static IPAddress localhost = IPAddress.Parse("127.0.0.1");
        public static string GetIP4Address() {

            IPAddress[] ips = Dns.GetHostAddresses( Dns.GetHostName() );

            foreach (IPAddress i in ips) {

                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return i.ToString();
            }

            return "127.0.0.1";
        }

    }

}
