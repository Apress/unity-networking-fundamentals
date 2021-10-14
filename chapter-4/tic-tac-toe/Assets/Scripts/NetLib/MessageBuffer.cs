using System;

/// <summary>
/// Buffer used to receive data if the size is too large to take in one
/// piece.
/// </summary>
public class MessageBuffer
{
    /// <summary>
    /// The current offset inside the buffer.
    /// </summary>
    private int _currentOffset;

    /// <summary>
    /// The buffer used to store data.
    /// </summary>
    public byte[] Buffer { get; }

    /// <summary>
    /// Returns true if the buffer is full.
    /// </summary>
    public bool IsComplete => _currentOffset == Buffer.Length;

    /// <summary>
    /// The size of the buffer.
    /// </summary>
    public int Length => Buffer.Length;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="size">Size of the buffer</param>
    public MessageBuffer(int size)
    {
        Buffer = new byte[size];
    }

    /// <summary>
    /// Append data to the buffer.
    /// </summary>
    /// <param name="source">Data to append</param>
    /// <param name="length">The number of bytes to copy from the source</param>
    public void Append(byte[] source, int length = -1)
    {
        var len = length > 0 ? length : source.Length;
        Array.Copy(source, 0, Buffer, _currentOffset, len);
        _currentOffset += len;
    }
}