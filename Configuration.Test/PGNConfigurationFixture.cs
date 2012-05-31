using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2KDashboard.Configuration;

namespace Configuration.Test
{
    /// <summary>
    /// Fluent configuration of packetlogger PGN's.
    /// </summary>
    /// <remarks>
    /// Updated to version 2010-10-06
    /// </remarks>
    [TestClass]
    public class PGNConfigurationFixture
    {
        
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void BasicConfiguration()
        {
            PGNSettings configuration = new PGNSettings();
            configuration.AddPGN(59392, "ISO Acknowledgement")
                .FirstField("Control", 8, f =>
                    f.SetLookups(d =>
                    {
                        d.Add(0, "ACK");
                        d.Add(1, "NAK");
                        d.Add(2, "Access Denied");
                        d.Add(3, "Adress Busy");
                    }))
                .NextField("Group Function", 8)
                .NextField("Reserved", 24, f => f.IsSigned = true)
                .NextField("PGN", 24, f =>
                {
                    f.Description = "Parameter Group Number of requested information";
                    f.FieldType = PGNFieldType.Integer;
                });

            configuration.AddPGN(59904, "ISO Request")
                .FirstField("PGN", 24).IsInteger();

            PGNConfiguration result59392 = configuration.GetById(59392);
            Assert.IsNotNull(result59392);
            Assert.AreEqual(59392, result59392.PGN);
            Assert.AreEqual("ISO Acknowledgement", result59392.Name);

            var fields = result59392.Fields;
            Assert.IsNotNull(fields);
            Assert.AreEqual(4, fields.Count);

            var pgn = fields.FirstOrDefault(f => f.Name == "PGN");
            Assert.IsNotNull(pgn);
            Assert.AreEqual("Parameter Group Number of requested information", pgn.Description);
            Assert.AreEqual(4, pgn.Order);

            var signed = fields.FirstOrDefault(f => f.IsSigned);
            Assert.IsNotNull(signed);
            Assert.AreEqual("Reserved", signed.Name);

            var result59904 = configuration.GetById(59904);
            Assert.IsNotNull(result59904);
            Assert.AreEqual(59904, result59904.PGN);
            Assert.AreEqual("ISO Request", result59904.Name);
        }

        [TestMethod]
        public void Heading()
        {
            PGNSettings configuration = new PGNSettings();
            configuration.AddPGN(127250, "Vessel Heading")
                .FirstField("SID", 8)
                .NextField("Heading", 16, f=> f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Deviation", 16, f => f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Variation", 16, f => f.SetAngle(AngleUnit.Radians, 0.0001d))
                .NextField("Reference", 2, f => f.SetLookups
                (d =>
                    {
                        d.Add(0, "True");
                        d.Add(1, "Magnetic");
                    }
                ));

            PGNConfiguration vesselHeading = configuration.GetById(127250);
            Assert.IsNotNull(vesselHeading);
            Assert.AreEqual(127250, vesselHeading.PGN);
            Assert.AreEqual("Vessel Heading", vesselHeading.Name);

            var fields = vesselHeading.Fields;
            Assert.IsNotNull(fields);
            Assert.AreEqual(5, fields.Count);

            var deviation = fields.FirstOrDefault(f => f.Name == "Deviation");
            Assert.IsNotNull(deviation);
            Assert.AreEqual(24, deviation.BitOffset);
            Assert.AreEqual(AngleUnit.Radians, deviation.Units);
            Assert.AreEqual(0.0001d, deviation.Resolution);
        }
    }
}
