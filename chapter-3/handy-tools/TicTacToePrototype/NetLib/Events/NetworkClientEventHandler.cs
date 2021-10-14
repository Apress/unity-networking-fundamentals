using System;

namespace NetLib
{
    public delegate void NetworkClientEventHandler(object sender, NetworkClientEventArgs e);

    public class NetworkClientEventArgs : EventArgs
    {
        public NetworkClient NetworkClient { get; }

        public NetworkClientEventArgs(NetworkClient networkClient)
        {
            NetworkClient = networkClient;
        }
    }
}