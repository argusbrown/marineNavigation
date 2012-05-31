using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace N2KDashboard.Configuration
{
    [DebuggerDisplay("Order={Order} Name={Name} Offset={BitOffset} Length={BitLength}")]
    public class PGNField
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public int BitLength { get; set; }
        public int BitOffset { get; set; }
        public int BitStart { get; set; }
        public PGNFieldType FieldType { get; set; }
        public string Description { get; set; }
        public bool IsSigned { get; set; }
        public Dictionary<int, string> Lookups { get; private set; }
        public AngleUnit Units { get; set; }
        public double Resolution { get; set; }
        
        public void SetLookups(Action<Dictionary<int, string>> dictionary)
        {
            FieldType = PGNFieldType.Lookup;

            Lookups = new Dictionary<int, string>();
            dictionary(Lookups);
        }

        public void SetAngle(AngleUnit units, double resolution)
        {
            FieldType = PGNFieldType.Angle;

            Units = units;
            Resolution = resolution;
        }
    }
}