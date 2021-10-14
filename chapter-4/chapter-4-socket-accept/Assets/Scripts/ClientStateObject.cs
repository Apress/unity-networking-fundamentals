using System.Net.Sockets;

public class ClientStateObject
{
    public TcpClient Client { get; }

    public NetworkStream Stream => Client.GetStream();

    public byte[] Buffer { get; }

    public ClientStateObject(TcpClient client, int bufferSize = 1024)
    {
        Client = client;
        Buffer = new byte[bufferSize];
    }
}