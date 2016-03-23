using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
namespace ConsoleApplication1
{
   public class Account
    {
        private Socket _sock;
        // private Thread _thread;
        private bool _connected;

       public Account(Socket socket)
        {
            _sock = socket;
            _connected = true;
            //_sock.BeginReceive(sta)
        }
        public void StartThread()
        {

            StateObject state = new StateObject();
            state.workSocket = _sock;
            _sock.BeginReceive(state.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(readCallback), state);

            while (_connected)
            {
                _connected = IsSocketConnected(_sock);
            }
        }

        static bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        static void sendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            handler.EndSend(ar);

            StateObject newstate = new StateObject();
            newstate.workSocket = handler;
            handler.BeginReceive(newstate.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(readCallback), newstate);
        }

        static void readCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            if (!IsSocketConnected(handler))
            {
                handler.Close();
                return;
            }

            int read = handler.EndReceive(ar);

            // Data was read from the client socket.
            if (read > 0)
            {
                Console.WriteLine(state.sb.ToString());
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, read));
                if (state.sb.ToString().StartsWith("APOP"))
                {

                }
            //    if (state.sb.ToString().Contains("<!--ENDSOCKET-->"))
             //   {
                    string toSend = "";
                    string cmd = "";

                    cmd = state.sb.ToString();

                /*switch (cmd)
                {
                    case "Hi!":
                        toSend = "How are you?";
                        break;
                    case "Milky Way?":
                        toSend = "No I am not.";
                        break;
                }*/
                Console.WriteLine(cmd);
                    toSend = cmd;
                    toSend = "<!--RESPONSE-->" + toSend + "<!--ENDRESPONSE-->";

                    byte[] bytesToSend = Encoding.UTF8.GetBytes(toSend);
                    handler.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None
                        , new AsyncCallback(sendCallback), state);
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
            }
        }


    }
}
