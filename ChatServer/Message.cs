using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChatServer
{
    public class Message
    {
        public Message(Client message, string sender, int uid, DateTime time)
        {
            this.message = message;
            this.sender = sender;
            this.time = time;
            this.uid = uid;
        }

        public string sender { set; get; }
        public string message { set; get; }
        public int uid { set; get; }
        public DateTime time { set; get; }
    }
}
