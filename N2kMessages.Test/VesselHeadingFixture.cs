using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2KDashboard.Configuration;

namespace N2kMessages.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class VesselHeadingFixture
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
        public void VesselHeadingMessageLoadsProperties()
        {
            N2kMessageEncoded encodedMessage = new N2kMessageEncoded
            {
                PGN = 127250,
                Priority = 2,
                Destination = 255,
                Source = 58,
                Milliseconds = 45,
                Payload = new byte[] {0xFF, 0x00, 0x00, 0xFF, 0x7F, 0xFF, 0x7F, 0xFD}
            };

            VesselHeading vesselHeading = new VesselHeading(encodedMessage);

            Assert.AreEqual(255, vesselHeading.SID);
            Assert.AreEqual(0, vesselHeading.Heading);
            Assert.AreEqual(187.74108073051684, vesselHeading.Deviation, double.Epsilon);
            Assert.AreEqual(187.74108073051684, vesselHeading.Variation, double.Epsilon);
            Assert.AreEqual(HeadingType.True, vesselHeading.HeadingReference);
        }
    }
}
