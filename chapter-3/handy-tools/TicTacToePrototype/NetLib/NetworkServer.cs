﻿using System;
using System.Net;
using System.Net.Sockets;

namespace NetLib
{
    public abstract class NetworkServer<T>
    {
        private readonly NetworkClientCollection _clients = new NetworkClientCollection();
        private readonly TcpListener _listener;
        private readonly int _maxConnections;

        public event NetworkClientEventHandler ClientConnected;
        public event NetworkClientEventHandler ConnectionOverflow;
        public event PayloadEventHandler<T> PayloadReceived;
        public event EventHandler ClientListFull;

        public NetworkServer(int port, int maxConnections = 16)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _maxConnections = maxConnections;
            _clients.MessageReceived += Client_MessageReceived;
        }

        public void Start()
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(Listener_ClientConnected, null);
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Listener_ClientConnected(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                var client = _listener.EndAcceptTcpClient(ar);
                var networkClient = new NetworkClient(client);

                if (_clients.Count == _maxConnections)
                {
                    ConnectionOverflow?.Invoke(this, new NetworkClientEventArgs(networkClient));
                }
                else
                {
                    _clients.Add(networkClient);
                    OnClientConnected(networkClient);
                    _listener.BeginAcceptTcpClient(Listener_ClientConnected, null);

                    if (_clients.Count == _maxConnections)
                    {
                        ClientListFull?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var payload = CreatePayload(e.Data);
            PayloadReceived?.Invoke(sender, new PayloadEventArgs<T>(payload));
        }

        protected abstract T CreatePayload(byte[] message);

        protected void Broadcast(byte[] message)
        {
            foreach (var c in _clients)
            {
                c.Send(message);
            }
        }

        protected virtual void OnClientConnected(NetworkClient networkClient)
        {
            ClientConnected?.Invoke(this, new NetworkClientEventArgs(networkClient));
        }
    }
}
