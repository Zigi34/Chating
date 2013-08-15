using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICQInterface
{
    public interface IServer
    {
        int Port { get; }
        
        bool Start();
        bool Stop();

        bool AcceptClient(IClient aClient);
        bool LogOutClient(IClient aClient);
    }
}
