using System;

namespace Communication
{
    public abstract class BaseDevice : IDevice
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        protected BaseDevice(ISerialPort serialPort)
        {
            serialPort.DataReceived += OnSerialPortDataReceived;
        }

        void OnSerialPortDataReceived(object sender, DeviceDataReceivedEventArgs e)
        {
            OnDeviceDataReceived(e.Data);
        }

        /// <summary>
        /// Data arriving on the port can be processed directly by inheriting classes.
        /// RaiseDataReceived(...) event needs to be raised by inheriting class once a message has been assembled.
        /// </summary>
        /// <param name="data"></param>
        protected abstract void OnDeviceDataReceived(byte[] data);

        /// <summary>
        /// Call this method
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="data"></param>
        protected void RaiseMessageReceived(MessageType messageType, byte[] data)
        {
            var handler = MessageReceived;
            if (handler != null)
            {
                handler(this, new MessageReceivedEventArgs(messageType, data));
            }
        }
    }
}