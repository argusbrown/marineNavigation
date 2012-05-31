using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2kMessages;

namespace Parser.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ActisenseMessageParserFixture
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
        public void BasicMessageParse()
        {
            byte[] fakeData = new byte[]
            {
                // header information
                0x02, 0x12, 0xF1, 0x01, 0xFF, 0x3A, 0x2D, 0x00, 0x00, 0x00, 0x08,
                // payload
                0xFF, 0x00, 0x00, 0xFF, 0x7F, 0xFF, 0x7F, 0xFD
            };

            byte[] expectedPayload = new byte[] { 0xFF, 0x00, 0x00, 0xFF, 0x7F, 0xFF, 0x7F, 0xFD };

            FakeDevice fakeDevice = new FakeDevice();
            ActisenseMessageParser parser = new ActisenseMessageParser(fakeDevice);

            var messagesParsed = new List<N2kMessageEventArgs>();
            parser.N2kMessageParsed += ((sender, e) => messagesParsed.Add(e));


            Assert.AreEqual(0, messagesParsed.Count);

            fakeDevice.RaiseMessageReceivedEvent(fakeData);

            Assert.AreEqual(1, messagesParsed.Count);

            N2kMessageEncoded result = messagesParsed[0].Message;
            Assert.AreEqual(2, result.Priority);
            Assert.AreEqual(127250, result.PGN);
            Assert.AreEqual(255, result.Destination);
            Assert.AreEqual(58, result.Source);
            Assert.AreEqual(45, result.Milliseconds);
            Assert.AreEqual(8, result.PayloadLength);
            Assert.IsTrue(expectedPayload.SequenceEqual(result.Payload), "arrays should be identical");

        }
    }
}
