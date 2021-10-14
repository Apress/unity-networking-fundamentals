using System.Net.Sockets;

public class StateObject
{
    public byte[] Buffer { get; }
    public Socket Socket { get; }

    public StateObject(Socket socket, int bufferSize = 1024)
    {
        Buffer = new byte[bufferSize];
        Socket = socket;
    }
}
