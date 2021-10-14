using System;

/// <summary>
/// Message received event arguments.
/// </summary>
public class MessageReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Data received.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="data">Buffer containing message received</param>
    /// <param name="length">Length of the data</param>
    public MessageReceivedEventArgs(byte[] data, int length)
    {
        Data = new byte[length];
        Array.Copy(data, Data, length);
    }
}