using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICQInterface
{
    public interface IMessage
    {
        string Message { get; }
        IUser FromUser { get; }
        IUser ToUser { get; }
    }
}
