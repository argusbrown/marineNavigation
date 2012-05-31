using System;

namespace Communication
{
    public interface ISerialPort
    {
        string PortName { get; set; }
        int BaudRate { get; set; }
        bool DtrEnable { get; set; }
        bool RtsEnable { get; set; }

        event EventHandler<DeviceDataReceivedEventArgs> DataReceived;

        void Open();
        void Close();
    }
}