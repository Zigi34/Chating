using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MyChatServer
{
    public class Server
    {
        public delegate void SendMessage(Object sender, ServerMessageArgs args);
        public event SendMessage newMessageEvent;

        public List<Message> messages;
        public List<Socket> clients;
        public Socket serverSocket;
        public int last_uid;
        public int port;
        public IPAddress ip;
        public IPEndPoint ep;

        public Server(int port)
        {
            ip = IPAddress.Parse("127.0.0.1");
            ep = new IPEndPoint(ip, port);
            this.messages = new List<Message>();
            this.clients = new List<Socket>();
            this.last_uid = 0;
            Console.WriteLine("Server created.");
        }

        public void start()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ep);
                serverSocket.Listen(10);

                Console.WriteLine("Server started.");

                while (true)
                {
                    Thread t = new Thread(new ParameterizedThreadStart(Communicate));
                    t.IsBackground = true;
                    Socket cli = serverSocket.Accept();
                    this.clients.Add(cli);
                    t.Start(cli);
                    Console.WriteLine("New connection: {0}.", cli.RemoteEndPoint);
                }
            }
            catch
            {
                Console.WriteLine("Error: Something wrong!");
            }
            finally
            {
                if (serverSocket != null) serverSocket.Close();
            }
        }

        public void newMessage(string message, string login)
        {
            lock (this)
            {
                Message newMessage = new Message(message, login, ++this.last_uid, DateTime.Now);
                this.messages.Add(newMessage);
                if (this.newMessageEvent != null)
                {
                    this.newMessageEvent(this, new ServerMessageArgs(newMessage));
                }
            }
        }

        public void Communicate(Object socket)
        {
            Socket s = (Socket)socket;

            NetworkStream networkStream = new NetworkStream(s);
            StreamWriter output = new StreamWriter(networkStream);
            StreamReader input = new StreamReader(networkStream);

            try
            {
                string login = input.ReadLine();
                Console.WriteLine("user: {0}.", login);
                while (true)
                {
                    string recv = input.ReadLine();
                    Console.WriteLine("Received: |{0}|.", recv);
                    if (recv == ";UPDATE") // update
                    {
                        Console.WriteLine("Call for update!");
                        string toSend = "";
                        foreach (Message m in this.messages)
                        {
                            toSend += m.time.Hour + ":" + m.time.Minute + ":" + m.time.Second + " <" + m.sender + ">: " + m.message + "|";
                        }
                        output.WriteLine(toSend);
                        output.Flush();
                        Console.WriteLine("Update: {0}.", toSend);
                    }
                    else if (recv == ";EXIT")
                    {
                        s.Shutdown(SocketShutdown.Both);
                        break;
                    }
                    else
                    {
                        this.newMessage(recv, login);
                    }
                }
            }
            finally
            {
                if (s != null) s.Close();
            }
        }
    }
}
