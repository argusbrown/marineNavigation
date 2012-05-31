using System;
using Communication;

namespace Parser.Test
{
    internal class FakeDevice : IDevice
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void RaiseMessageReceivedEvent(byte[] data)
        {
            var handler = MessageReceived;
            if (handler != null)
            {
                handler(this, new MessageReceivedEventArgs(MessageType.NMEA2000, data));
            }
        }
    }
}