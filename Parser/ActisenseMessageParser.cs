using System;
using Communication;
using N2kMessages;

namespace Parser
{
    public class ActisenseMessageParser
    {
        private IDevice device;

        public event EventHandler<N2kMessageEventArgs> N2kMessageParsed;

        private enum ActisenseN2kMessageFormat
        {
            Priority = 0,
            PGNLSB = 1,
            PGNMiddle = 2,
            PGNMSB = 3,
            Destination = 4,
            Source = 5,
            TimeStampLSB = 6,
            TimeStamp2 = 7,
            TimeStamp3 = 8,
            TimeStampMSB = 9,
            PayloadLength = 10,
            PayloadStart = 11
        }

        public ActisenseMessageParser(IDevice device)
        {
            this.device = device;
            // TODO: implement IDisposable and release event handler
            this.device.MessageReceived += OnDeviceMessageReceived;
        }

        void OnDeviceMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // The message has already been converted from Can Id to ISO-11783 by the Actisense device.
            if (e.MessageType == MessageType.NMEA2000)
            {
                var n2kMessage = new N2kMessageEncoded
                {
                    // leave parenthesis to prevent unwanted bitshifting
                    Priority = e.Data[(int) ActisenseN2kMessageFormat.Priority],
                    PGN =   (e.Data[(int) ActisenseN2kMessageFormat.PGNLSB] +
                            (e.Data[(int) ActisenseN2kMessageFormat.PGNMiddle] << 8) +
                            (e.Data[(int) ActisenseN2kMessageFormat.PGNMSB] << 16)),
                    Destination = e.Data[(int) ActisenseN2kMessageFormat.Destination],
                    Source = e.Data[(int) ActisenseN2kMessageFormat.Source],
                    Milliseconds =
                        (e.Data[(int) ActisenseN2kMessageFormat.TimeStampLSB] +
                        (e.Data[(int) ActisenseN2kMessageFormat.TimeStamp2] << 8) +
                        (e.Data[(int) ActisenseN2kMessageFormat.TimeStamp3] << 16) +
                        (e.Data[(int) ActisenseN2kMessageFormat.TimeStampMSB] << 32))
                };

                int payloadLength = e.Data[(int) ActisenseN2kMessageFormat.PayloadLength];
                byte[] payload = new byte[payloadLength];

                // could check there are enough bytes...
                if (e.Data.Length - (int)ActisenseN2kMessageFormat.PayloadStart < payloadLength) payloadLength = e.Data.Length - (int)ActisenseN2kMessageFormat.PayloadStart;
                Array.Copy(e.Data, (int)ActisenseN2kMessageFormat.PayloadStart, payload, 0, payloadLength);

                n2kMessage.Payload = payload;

                var handler = N2kMessageParsed;
                if (handler != null)
                {
                    handler(this, new N2kMessageEventArgs(n2kMessage));
                }
            }
        }
    }
}