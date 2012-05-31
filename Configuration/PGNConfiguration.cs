using System.Collections.Generic;

namespace N2KDashboard.Configuration
{
    public class PGNConfiguration
    {
        public string Name { get; set; }
        public int PGN { get; set; }
        public uint ByteLength { get; set; }
        public List<PGNField> Fields { get; set; }
    }
}