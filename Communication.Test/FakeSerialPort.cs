using System;

namespace Communication.Test
{
    /// <summary>
    /// that this can be replace later with rhino mocks and container
    /// </summary>
    public class FakeSerialPort : ISerialPort
    {
        #region Implementation of ISerialPort

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }
        public event EventHandler<DeviceDataReceivedEventArgs> DataReceived;

        public void Open(){}
        public void Close(){}

        public void SendDataFromDevice(byte[] data)
        {
            var handler = DataReceived;
            if (handler != null)
            {
                handler(this, new DeviceDataReceivedEventArgs(data));
            }
        }
        #endregion
    }
}