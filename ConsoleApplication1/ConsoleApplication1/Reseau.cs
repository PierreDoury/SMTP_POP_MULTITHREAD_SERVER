using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ConsoleApplication1
{


    class Reseau
    {
        Socket _socket;
        String _ip;
        int _port;
        bool init(String ip, int port)
        {
            _ip = ip;
            _port = port;
            return true;
        }
        bool init()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ip = Dns.GetHostName();
            _port = 80;
            return true;
        }
    }
}
