using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ICQInterface
{
    public interface IClient
    {
        int Port { get; }
        IPEndPoint IP { get; }
        IUser User { get; }

        bool Connect(IPEndPoint aServer, int aPort);
        bool Disconnect();

        bool SendMessage(IMessage aMessage);
        void ReceiveMessage(IMessage aMessage);
    }
}
