using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(TcpListenSocketBehaviour))]
public class TcpSocketAsyncBehaviour : MonoBehaviour
{
    private Socket _socket;

    [Tooltip("The port the service is running on")]
    public int _port = 9021;

    IEnumerator Start()
    {
        var listener = GetComponent<TcpListenSocketBehaviour>();
        while (!listener._isReady)
        {
            yield return null;
        }

        _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(IPAddress.Parse("127.0.0.1"), _port);
        var msg = Encoding.ASCII.GetBytes("Hello, from Client!");
        _socket.BeginSend(msg,
                          0,
                          msg.Length,
                          SocketFlags.None,
                          Send_Complete,
                          _socket);
    }    

    private void Send_Complete(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            var socket = ar.AsyncState as Socket;
            var bytesSent = socket.EndSend(ar);
            print($"{bytesSent} bytes sent");
        }
    }
}