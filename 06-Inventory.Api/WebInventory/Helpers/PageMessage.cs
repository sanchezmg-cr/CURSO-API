using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInventory.Helpers
{
    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Question,
        Danger
    };

    public class PageMessage
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

        public PageMessage(MessageType messageType, string message)
        {
            MessageType = messageType;
            Message = message;
        }
    }
}
