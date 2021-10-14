using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

public class NetworkClient
{
    public const int DefaultBufferSize = 4096;

    private readonly byte[] _buffer;
    private TcpClient _client;

    private MessageBuffer _store;

    private bool ClientCanRead
    {
        get
        {
            return _client != null && _client.Connected && _client.GetStream().CanRead;
        }
    }

    private bool ClientCanWrite
    {
        get
        {
            return _client != null && _client.Connected && _client.GetStream().CanWrite;
        }
    }

    public event EventHandler<MessageReceivedEventArgs> MessageReceived;

    public NetworkClient(TcpClient client, int bufferSize = DefaultBufferSize)
    {
        _client = client;
        _buffer = new byte[bufferSize];
        _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Remote_ReceivedMessage, null);
    }

    public NetworkClient(int bufferSize = DefaultBufferSize)
    {
        _buffer = new byte[bufferSize];
        _client = new TcpClient(AddressFamily.InterNetwork);
        _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Remote_ReceivedMessage, null);
    }

    public void Close()
    {
        _client.Close();
        _client.Dispose();
        _client = null;
    }

    public void Send(byte[] message)
    {
        if (!ClientCanWrite)
        {
            return;
        }

        var header = $"Size: {message.Length}\n";
        var headerBytes = Encoding.ASCII.GetBytes(header);
        var fullMessage = new byte[message.Length + header.Length];

        Array.Copy(headerBytes, fullMessage, headerBytes.Length);
        Array.Copy(message, 0, fullMessage, headerBytes.Length, message.Length);

        _client.GetStream().BeginWrite(fullMessage, 0, fullMessage.Length, Write_Callback, null);
    }

    private void Remote_ReceivedMessage(IAsyncResult ar)
    {
        if (ar.IsCompleted && ClientCanRead)
        {
            var bytesReceived = _client.GetStream().EndRead(ar);
            if (bytesReceived > 0)
            {
                if (_store != null)
                {
                    AppendToStore(bytesReceived);
                }
                else
                {
                    ReadBuffer(bytesReceived);
                }

                Array.Clear(_buffer, 0, _buffer.Length);
                _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Remote_ReceivedMessage, null);
            }
        }
    }

    /// <summary>
    /// Read the contents of the buffer and determine the size of the payload.
    /// If the payload size matches the expected length, MessageReceived is fired.
    /// Otherwise, the payload is added to an internal buffer.
    /// </summary>
    /// <param name="bytesReceived">The number of bytes received.</param>
    private void ReadBuffer(int bytesReceived)
    {
        var text = Encoding.ASCII.GetString(_buffer, 0, bytesReceived);
        var sizeMatch = new Regex("^[S|s]ize:\\s");
        var match = sizeMatch.Match(text);
        if (match.Success)
        {
            var startOfLength = match.Index + match.Length;
            var endOfLine = text.IndexOf('\n', startOfLength);
            var lengthStr = text.Substring(startOfLength, endOfLine - startOfLength);
            var length = int.Parse(lengthStr);
            var payloadSoFar = text.Substring(endOfLine + 1, text.Length - (endOfLine + 1));

            var payload = Encoding.ASCII.GetBytes(payloadSoFar);
            if (payloadSoFar.Length == length)
            {
                var args = new MessageReceivedEventArgs(payload, payload.Length);
                MessageReceived?.Invoke(this, args);
            }
            else
            {
                _store = new MessageBuffer(length);
                _store.Append(payload);
            }
        }
    }

    /// <summary>
    /// Append the contents of the buffer (size 'count') to the internal store
    /// if there is more message to receive. If the store has received the entire
    /// message, MessageReceived is called.
    /// </summary>
    /// <param name="bytesReceived">The number of bytes to copy from the buffer</param>
    private void AppendToStore(int bytesReceived)
    {
        _store.Append(_buffer, bytesReceived);
        if (_store.IsComplete)
        {
            var args = new MessageReceivedEventArgs(_store.Buffer, _store.Length);
            MessageReceived?.Invoke(this, args);
            _store = null;
        }
    }

    private void Write_Callback(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            _client.GetStream().EndWrite(ar);
        }
    }
}
