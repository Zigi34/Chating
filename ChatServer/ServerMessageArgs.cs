using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChatServer
{
    public class ServerMessageArgs : EventArgs
    {
        public Message message { set; get; }

        public ServerMessageArgs(Message m)
        {
            this.message = m;
        }
    }
}
