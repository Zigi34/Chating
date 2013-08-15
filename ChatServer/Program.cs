using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace MyChatServer
{
    class MyServer
    {
        private static int mPort = 8888;
        private static Server mServer = null;

        static void Main(string[] args)
        {
            if (args.Length > 0)
                mPort = int.Parse(args[0]);

            mServer = new Server(mPort);
            mServer.Start();
        }
    }
}
