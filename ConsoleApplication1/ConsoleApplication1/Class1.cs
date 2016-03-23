using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class StateObject
    {
        public Socket workSocket = null;
        public const int buffersize = 10000;
        public byte[] buffer = new byte[buffersize];
        public StringBuilder sb = new StringBuilder();
    }
}
