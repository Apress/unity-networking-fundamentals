using System;

public class NetworkClientEventArgs : EventArgs
{
    public NetworkClient NetworkClient { get; }

    public NetworkClientEventArgs(NetworkClient networkClient)
    {
        NetworkClient = networkClient;
    }
}