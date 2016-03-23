using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ConsoleApplication1
{
    public class PopServer
    {
        private static Socket listener;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
       // public const int _buffersize = 50000;
        public const int _port = 50000;
        public static bool _isRunning = true;


        List<Account> listClient = null;
        List<Thread> threadClient;

  

        static void acceptCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            Socket listener = (Socket)ar.AsyncState;

            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);
                Account account;
                Console.WriteLine("Nouvelle connection /n");
              
                // Signal main thread to continue
                allDone.Set();
                account = new Account(handler);
                Thread oThread = new Thread(new ThreadStart(account.StartThread));
                oThread.Start();

                account


                /* TODO GESTION JOIN THREAD
                
                while (oThread.IsAlive) ;
                oThread.Join();
                */
                // Create state
            }
        }

        public void Start()
        {
            
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, _port);
            listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEP);

            while (_isRunning)
            {
                allDone.Reset();
                Console.WriteLine("SERVEUR POP: Start Listening /n");
                listener.Listen(10);
                listener.BeginAccept(new AsyncCallback(acceptCallback), listener);
                bool isRequest = allDone.WaitOne(new TimeSpan(12, 0, 0));  // Blocks for 12 hours

                if (!isRequest)
                {
                    allDone.Set();
                    // Do some work here every 12 hours
                }
            }
            listener.Close();
        }
    }
}
