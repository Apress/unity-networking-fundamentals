using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(TcpListenClientBehaviour))]
public class TcpClientAsyncBehaviour : MonoBehaviour
{
    private TcpClient _client;

    [Tooltip("The server's IP address")]
    public string _ipAddress = "127.0.0.1";

    [Tooltip("The port the service is running on")]
    public int _port = 9021;

    IEnumerator Start()
    {
        var listener = GetComponent<TcpListenClientBehaviour>();
        while (!listener._isReady)
        {
            yield return null;
        }

        _client = new TcpClient();
        _client.Connect(_ipAddress, _port);
        var msg = Encoding.ASCII.GetBytes("Hello, from TcpClient!");

        _client.GetStream()
               .BeginWrite(msg,
                           0,
                           msg.Length,
                           Send_Complete,
                           _client);
    }

    private void Send_Complete(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            var client = ar.AsyncState as TcpClient;
            client.GetStream()
                  .EndWrite(ar);
        }
    }
}
