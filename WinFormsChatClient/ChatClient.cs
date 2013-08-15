using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace WinFormsChatClient
{
        public class ChatClient
        {
            public Socket server;
            public IPAddress ip;
            public IPEndPoint ep;
            public int port;
            public StreamWriter output;
            public StreamReader input;
            public NetworkStream networkStream;

            public ChatClient(string ipaddress, int newport)
            {
                ip = IPAddress.Parse(ipaddress);
                Console.WriteLine("IP: {0}", ip);
                port = newport;
                ep = new IPEndPoint(ip, port);
                Console.WriteLine("End point: {0}", ep);
            }

            public void connect()
            {
                try
                {
                    this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Console.WriteLine("Creating socket.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: creating socket -> " + e);
                }
                try
                {
                    server.Connect(ep);
                    this.networkStream = new NetworkStream(this.server);
                    this.output = new StreamWriter(this.networkStream);
                    this.input = new StreamReader(this.networkStream);
                    Console.WriteLine("Connected to {0}: {1}", ep, server.Connected);
                }
                catch (Exception e)
                {
                    if (server != null) server.Close();
                    Console.WriteLine("Error: connect -> {0}", e);
                }
            }

            public void login(string nick)
            {
                try
                {
                    output.WriteLine(nick);
                    output.Flush();
                    Console.WriteLine("Logged in as {0}.", nick);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: login -> {0}", e);
                }
            }

            public void sendMessage(string message)
            {
                try
                {
                    this.output.WriteLine(message);
                    this.output.Flush();
                    Console.WriteLine("Sending: {0}", message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: send message -> {0}", e);
                }
            }

            public void disconnect()
            {
                try
                {
                    if (this.server != null)
                    {
                        this.output.WriteLine(";EXIT");
                        this.output.Flush();
                        Console.WriteLine("Disconnecting.");
                        this.server.Shutdown(SocketShutdown.Both);
                        this.server.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: disconnect -> {0}" + e);
                }
            }

            public string[] update()
            {
                try
                {
                    this.output.WriteLine(";UPDATE");
                    this.output.Flush();
                    Console.WriteLine("Calling for update.");
                    string recv = this.input.ReadLine();
                    char[] delimiters = { '|' };
                    string[] ret = recv.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    return ret;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: update -> {0}", e);
                }
                return new string[0];
            }
        }
}
