using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
namespace ConsoleApplication1
{
    class Program
    {

       
        static void Main(string[] args)
        {
            PopServer server;
            server = new PopServer();
            Thread oThread = new Thread(new ThreadStart(server.Start));
            oThread.Start();
            while (oThread.IsAlive);
            oThread.Join();
        }
    }
}
