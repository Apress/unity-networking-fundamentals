using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TcpListenSocketBehaviour : MonoBehaviour
{
    private TcpListener _listener;

    [HideInInspector]
    public bool _isReady;

    [Tooltip("The port the service is running on")]
    public int _port = 9021;

    void Start()
    {
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        _listener.BeginAcceptSocket(Socket_Connected, _listener);

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
            var socket = (ar.AsyncState as TcpListener).EndAcceptSocket(ar);
            var state = new StateObject(socket);

            socket.BeginReceive(state.Buffer,
                                0,
                                state.Buffer.Length,
                                SocketFlags.None,
                                Socket_Received,
                                state);
        }
    }

    private void Socket_Received(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            var state = ar.AsyncState as StateObject;
            var bytesIn = state.Socket.EndReceive(ar);

            if (bytesIn > 0)
            {
                var msg = Encoding.ASCII.GetString(state.Buffer,
                                                   0,
                                                   bytesIn);
                print($"From client: {msg}");
            }

            var newState = new StateObject(state.Socket);
            state.Socket.BeginReceive(state.Buffer,
                                      0,
                                      state.Buffer.Length,
                                      SocketFlags.None,
                                      Socket_Received,
                                      newState);
        }
    }
}
