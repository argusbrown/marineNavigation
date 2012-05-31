namespace N2kMessages
{
    public class N2kMessageEncoded
    {
        public byte Priority { get; set; }
        public int PGN { get; set; }
        public byte Destination { get; set; }
        public byte Source { get; set; }
        public int Milliseconds { get; set; }
        public int PayloadLength
        {
            get { return Payload.Length; }
        }

        /// <summary>
        /// The payload of the message is still encoded.
        /// Specific message types will have more message information.
        /// </summary>
        public byte[] Payload { get; set; }
    }
}