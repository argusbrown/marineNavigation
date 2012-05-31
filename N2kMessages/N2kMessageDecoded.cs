using System.Collections;
using System.Collections.Generic;
using N2KDashboard.Configuration;

namespace N2kMessages
{
    public abstract class N2kMessageDecoded
    {
        public byte Priority { get; set; }
        public int PGN { get; set; }
        public byte Destination { get; set; }
        public byte Source { get; set; }
        public int Milliseconds { get; set; }

        public abstract int[] AllowedPGNs { get;  }

        protected N2kMessageDecoded(N2kMessageEncoded encodedMessage)
        {
            Priority = encodedMessage.Priority;
            PGN = encodedMessage.PGN;
            Destination = encodedMessage.Destination;
            Source = encodedMessage.Source;
            Milliseconds = encodedMessage.Milliseconds;

            DecodeMessage(encodedMessage.Payload);   
        }

        private void DecodeMessage(byte[] payload)
        {
            #region this has to be totally refactored somehow and made more efficient for loading via singleton or whatever:
            PGNSettings configuration = new PGNSettings();
            configuration.AddPGN(127250, "Vessel Heading")
                .FirstField("SID", 8)
                .NextField("Heading", 16, f => f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Deviation", 16, f => f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Variation", 16, f => f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Reference", 2, f => f.SetLookups
                (d =>
                {
                    d.Add(0, "True");
                    d.Add(1, "Magnetic");
                }
                ));
            #endregion

            PGNConfiguration vesselHeadingConfiguration = configuration.GetById(PGN);

            var fieldInformation = new Dictionary<string, PGNFieldInformation>();
            BitArray payloadBitArray = new BitArray(payload);
            foreach (PGNField field in vesselHeadingConfiguration.Fields)
            {
                BitArray bitArray = new BitArray(field.BitLength);
                for (int i = 0; i < field.BitLength; i++)
                {
                    bitArray[i] = payloadBitArray[i + field.BitOffset];
                }

                PGNFieldInformation pgnFieldInformation = new PGNFieldInformation
                {
                    Configuration = field,
                    Data = bitArray
                };
                fieldInformation.Add(field.Name, pgnFieldInformation);
            }

            ProcessPayload(fieldInformation);
        }

        protected abstract void ProcessPayload(Dictionary<string, PGNFieldInformation> fieldInformation);
    }
}