using System;

namespace Communication
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }
        public MessageType MessageType { get; set; }

        public MessageReceivedEventArgs(MessageType messageType, byte[] data)
        {
            MessageType = messageType;
            Data = data;
        }
    }
}