using System;
using System.IO.Ports;

namespace Communication
{
    public sealed class SerialPort : ISerialPort
    {
        private readonly System.IO.Ports.SerialPort serialPort;
        private readonly object dataReadLock = new object();

        public SerialPort()
        {
            serialPort = new System.IO.Ports.SerialPort();
            serialPort.DataReceived += OnSerialPortDataReceived;
        }

        void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                byte[] readBuffer;

                lock (dataReadLock)
                {
                    int numBytesToRead = serialPort.BytesToRead;
                    readBuffer = new byte[numBytesToRead];
                    serialPort.Read(readBuffer, 0, numBytesToRead);
                }

                var handler = DataReceived;
                if (handler != null)
                {
                    handler(this, new DeviceDataReceivedEventArgs(readBuffer));
                }
            }
        }

        #region properties
        public string PortName
        {
            get { return serialPort.PortName; }
            set { serialPort.PortName = value; }
        }

        public int BaudRate
        {
            get { return serialPort.BaudRate; }
            set { serialPort.BaudRate = value; }
        }

        public bool DtrEnable
        {
            get { return serialPort.DtrEnable; }
            set { serialPort.DtrEnable = value; }
        }

        public bool RtsEnable
        {
            get { return serialPort.RtsEnable; }
            set { serialPort.RtsEnable = value; }
        }
        #endregion

        public event EventHandler<DeviceDataReceivedEventArgs> DataReceived;

        public int Read(byte[] buffer, int offset, int count)
        {
            return serialPort.Read(buffer, offset, count);
        }

        public void Open()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
        }

        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}