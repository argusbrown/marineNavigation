using System;
using N2kMessages;

namespace Parser
{
    public class N2kMessageEventArgs : EventArgs
    {
        public N2kMessageEncoded Message { get; set; }

        public N2kMessageEventArgs(N2kMessageEncoded message)
        {
            Message = message;   
        }
    }
}