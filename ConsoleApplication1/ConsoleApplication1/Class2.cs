using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ConsoleApplication1
{
   public class Commande
    {
       public String name;
       public String compl;
        public Func<String, int> Func;
        

       public Commande(String name, String comp, Func<String, int> func )
        {
            this.name = name;
            this.compl = comp;
            this.Func = func;
        }
    }
    public class Account
    {

        
        private Socket _sock;
        // private Thread _thread;
        public bool _connected;
        private List<String> listCommande;
        
        private List<Commande> command;

        private void sendMessage(String str)
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(str);
            _sock.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None
                    //       , new AsyncCallback(sendCallback), state);
        }
        int RequestHelper(String value,String desc)
        {
            int i = 0;
            switch (value)
            {
                case "QUIT":
                    Console.WriteLine("QUIT QUIT QUIT MOTHER FUCKER");
                  
                    i = quitCommande(desc); //because quitcommande return -1
                    _connected = false;
                  //   i = -1;
                     break;
                default:
                     break;    
            }
            return i;
        }
        public void init_command()
        {
            listCommande = new List<String>();
            listCommande.Add("QUIT");

        }
        public Account(Socket socket)
        {
            _sock = socket;
            _connected = true;
            command = new List<Commande>();
       
            //_sock.BeginReceive(sta)
            init_command();
        }
        public int quitCommande(String str)
        {
            _sock.Close();
            return -1;
        }
       
        public Socket getSock()
        {
            Socket sock;

            lock(_sock)
            {
                sock = _sock;
            }
            return sock;
        }
        public void StartThread()
        {

            StateObject state = new StateObject();
          //  lock (_sock)
           // {
            state.workSocket = _sock; 
            _sock.BeginReceive(state.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(readCallback), state);
            //}
            while (_connected)
            {
               
            }
        }

        public bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        public void sendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            handler.EndSend(ar);

            StateObject newstate = new StateObject();
            newstate.workSocket = handler;
            handler.BeginReceive(newstate.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(readCallback), newstate);
        }

       public void readCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            IPEndPoint remoteIpEndPoint = handler.RemoteEndPoint as IPEndPoint;

            String debug;
            Console.WriteLine("RECU");
            debug = "";
            if (remoteIpEndPoint != null)
            {
                // Using the RemoteEndPoint property.
                debug += "[" + remoteIpEndPoint.Address + "][" + remoteIpEndPoint.Port + "] : ";
            }
            if (!IsSocketConnected(handler))
            {
                handler.Close();
                debug = "Socket Deconnected \n";
               Console.WriteLine(debug);
                return;
            }

            int read = handler.EndReceive(ar);
            Console.WriteLine(debug);
            // Data was read from the client socket.
            if (read > 0)
            {
               
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, read));
                Console.WriteLine(state.sb.ToString());
                for (int i = 0;i < listCommande.Count; i++)
                {
                    if (state.sb.ToString().StartsWith(listCommande[i].ToString()))
                    {
                        RequestHelper(listCommande[i].ToString(), state.sb.ToString());
                        byte[] bytesToSend = Encoding.UTF8.GetBytes(toSend);
                             handler.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None
                               , new AsyncCallback(sendCallback), state);

                    }
                }
              /*  if (state.sb.ToString().StartsWith("QUIT"))
                {
                    debug += "[QUIT] \n";
                    _sock.Close();
                    _connected = false;
                    return;
     
                }
                else
                {

                }*/
              
                //    if (state.sb.ToString().Contains("<!--ENDSOCKET-->"))
                //   {
            
              //     
             //   }
               // else
                //{
                //    handler.BeginReceive(state.buffer, 0, StateObject.buffersize, 0
                 //           , new AsyncCallback(readCallback), state);
               // }
            }
            else
            {
                handler.Close();

                _connected = false;
              
            }
        }


    }
}
