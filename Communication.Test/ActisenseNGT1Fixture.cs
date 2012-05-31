using System.Linq;
using System.Collections.Generic;
using Communication.Actisense;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Communication.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ActisenseNGT1Fixture
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
        public void BasicMessageHandling()
        {
            FakeSerialPort serialPort = new FakeSerialPort();
            var ngt = new ActisenseNGT1(serialPort);

            var messagesReceived = new List<MessageReceivedEventArgs>();

            ngt.MessageReceived += ((sender, e) => messagesReceived.Add(e));

            Assert.AreEqual(0, messagesReceived.Count);

            byte[] fakeData = new byte[]
            {
                // header
                0x10, 0x02, 0x93, 0x13,
                // message body
                0x02, 0x12, 0xF1, 0x01, 0xFF, 0x3A, 0x2D, 0x00, 0x00, 0x00, 0x08, 0xFF, 0x00, 0x00, 0xFF, 0x7F, 0xFF, 0x7F, 0xFD,
                // checksum + C0 control codes
                0xEE, 0x10, 0x03
            };

            byte[] expectedMessageBody = new byte[]
            {
                // copy of message body above
                0x02, 0x12, 0xF1, 0x01, 0xFF, 0x3A, 0x2D, 0x00, 0x00, 0x00, 0x08, 0xFF, 0x00, 0x00, 0xFF, 0x7F, 0xFF, 0x7F, 0xFD,
            };

            serialPort.SendDataFromDevice(fakeData);
       
            Assert.AreEqual(1, messagesReceived.Count);
            var receivedMessage = messagesReceived[0];
            Assert.AreEqual(MessageType.NMEA2000, receivedMessage.MessageType);
            Assert.IsTrue(expectedMessageBody.SequenceEqual(receivedMessage.Data), "arrays should be identical");
        }
    }
}
