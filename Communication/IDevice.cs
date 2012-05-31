namespace Communication
{
    public interface IDevice
    {
        event System.EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}