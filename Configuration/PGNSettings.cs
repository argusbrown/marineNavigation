using System.Collections.Generic;
using System.Linq;

namespace N2KDashboard.Configuration
{
    public partial class PGNSettings
    {
        readonly List<PGNConfigurationRepresentation> pgnConfigurations = new List<PGNConfigurationRepresentation>();

        public PGNConfigurationRepresentation AddPGN(int pgn, string name)
        {
            var newConfiguration = new PGNConfigurationRepresentation(pgn, name);
            pgnConfigurations.Add(newConfiguration);

            return newConfiguration;
        }

        // move to unit test this function.
        // remove the pgnconfiguration class to another area that loads and uses a factory to load
        // the fields properly.
        public PGNConfiguration GetById(int pgn)
        {
            var representation = pgnConfigurations.FirstOrDefault(c => c.pgn == pgn);

            // here is where the factory will come into play!
            // this is equivalent to kees file handler
            return representation == null ?
                null :
                new PGNConfiguration
                {
                    PGN = representation.pgn,
                    Name = representation.name,
                    Fields = representation.pgnFields
                };
        }
    }
}