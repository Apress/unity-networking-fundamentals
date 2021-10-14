using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TcpListenClientBehaviour : MonoBehaviour
{
    private TcpListener _listener;

    [HideInInspector]
    public bool _isReady;

    [Tooltip("The port the service is running on")]
    public int _port = 9021;

    void Start()
    {
        _listener = new TcpListener(IPAddress.Any,
                                    _port);
        _listener.Start();
        _listener.BeginAcceptTcpClient(Socket_Connected,
                                       _listener);
        _isReady = true;
    }

    private void OnDestroy()
    {
        _listener?.Stop();
        _listener = null;
    }

    private void Socket_Connected(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            var client = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
            var state = new ClientStateObject(client);

            client.GetStream()
                    .BeginRead(state.Buffer,
                                0,
                                state.Buffer.Length,
                                Client_Received,
                                state);
        }
    }

    private void Client_Received(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            var state = ar.AsyncState as ClientStateObject;
            var bytesIn = state.Stream.EndRead(ar);

            if (bytesIn > 0)
            {
                var msg = Encoding.ASCII
                                    .GetString(state.Buffer,
                                                0,
                                                bytesIn);
                print($"From client: {msg}");
            }

            var newState = new ClientStateObject(state.Client);
            state.Stream
                    .BeginRead(state.Buffer,
                            0,
                            state.Buffer.Length,
                            Client_Received,
                            newState);
        }
    }
}
