using System;

namespace Communication
{
    public class DeviceDataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        public DeviceDataReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}