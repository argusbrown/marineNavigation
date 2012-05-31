using System;
using System.Collections.Generic;
using N2KDashboard.Configuration;

namespace N2kMessages
{
    public class VesselHeading : N2kMessageDecoded
    {
        public byte SID { get; set; }
        public double Heading { get; set; }
        public double Deviation { get; set; }
        public double Variation { get; set; }
        public HeadingType HeadingReference { get; set; }

        public VesselHeading(N2kMessageEncoded encodedMessage) : base(encodedMessage)
        {
        }

        #region Overrides of N2kMessageDecoded
        public override int[] AllowedPGNs
        {
            get { return new []{127250}; }
        }

        protected override void ProcessPayload(Dictionary<string, PGNFieldInformation> fieldInformation)
        {
            // the fields collection could possibly be further pre-processed to make this easy.
            // perhaps additional information in the configuration can be used to indicate how to unpack data.
            SID = fieldInformation["SID"].Data.ToByte();
            
            var headingField = fieldInformation["Heading"];
            Heading = GetAngle(headingField.Data.ToUnsignedShort(), headingField.Configuration.Resolution,
                               headingField.Configuration.Units);

            var deviationField = fieldInformation["Deviation"];
            Deviation = GetAngle(deviationField.Data.ToUnsignedShort(), deviationField.Configuration.Resolution,
                                 deviationField.Configuration.Units);

            var variationField = fieldInformation["Variation"];
            Variation = GetAngle(variationField.Data.ToUnsignedShort(), deviationField.Configuration.Resolution,
                                 deviationField.Configuration.Units);

            HeadingReference = fieldInformation["Reference"].Data.Get(1) ?
                HeadingType.Magnetic :
                HeadingType.True;
        }

        public double GetAngle(ushort angle, double precision, AngleUnit units)
        {
            double result = angle * precision;

            if (units == AngleUnit.Radians)
            {
                result = RadianToDegree(result);
            }
            return result;
        }

        public double RadianToDegree(double radians)
        {
            return radians * (180 / Math.PI);
        }
        #endregion

    }
}