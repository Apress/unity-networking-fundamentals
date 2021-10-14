using System;
namespace NetLib
{
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class MessageReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; }

        public MessageReceivedEventArgs(byte[] data, int length)
        {
            Data = new byte[length];
            Array.Copy(data, Data, length);
        }
    }
}