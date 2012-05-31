using System;
using System.Collections.Generic;
using System.Linq;

namespace N2KDashboard.Configuration
{
    public partial class PGNSettings
    {
        public class PGNConfigurationRepresentation
        {
            public string name { private set; get;}
            public int pgn { private set; get; }
            public bool isComplete { private set; get; }
            private int currentBitOffset;
            private int currentOrder;
            private PGNField currentPGNField = null;

            public readonly List<PGNField> pgnFields = new List<PGNField>();

            internal PGNConfigurationRepresentation(int pgn, string name)
            {
                this.pgn = pgn;
                this.name = name;
            }

            public PGNConfigurationRepresentation IsComplete(bool isComplete)
            {
                this.isComplete = isComplete;
                return this;
            }

            public PGNConfigurationRepresentation FirstField(string fieldName, int bitLength)
            {
                return FirstField(fieldName, bitLength, x => {});
            }

            public PGNConfigurationRepresentation FirstField(string fieldName, int bitLength, Action<PGNField> pgnField)
            {
                currentOrder = 1;
                currentBitOffset = 0;

                PGNField field = new PGNField { Order = currentOrder, Name = fieldName, BitLength = bitLength, BitOffset = 0};

                pgnField(field);

                currentPGNField = field;
                pgnFields.Add(field);

                return this;
            }


            public PGNConfigurationRepresentation NextField(string fieldName, int bitLength)
            {
                return NextField(fieldName, bitLength, x => {});
            }


            public PGNConfigurationRepresentation NextField(string fieldName, int bitLength, Action<PGNField> pgnField)
            {
                currentOrder++;
                currentBitOffset = pgnFields.Sum(x => x.BitLength); // should manually increment...

                PGNField field = new PGNField { Name = fieldName, Order = currentOrder, BitOffset = currentBitOffset, BitLength = bitLength};

                pgnField(field);

                currentPGNField = field;
                pgnFields.Add(field);

                return this;
            }

            //public PGNConfigurationRepresentation SetBitStart(int bitStart)
            //{
            //    //guard currentPGNField isn't null
            //    currentPGNField.BitStart = bitStart;
            //    return this;
            //}

            public PGNConfigurationRepresentation IsInteger(/* optional accuracy? */)
            {
                //guard currentPGNField isn't null
                currentPGNField.FieldType = PGNFieldType.Integer;
                return this;
            }

            //public PGNConfigurationRepresentation IsLookup()
            //{
            //    //guard currentPGNField isn't null
            //    currentPGNField.FieldType = PGNFieldType.Lookup;
            //    return this;
            //}
        }
    }
}
