using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// Hello World program using TCP. This creates a client/server application
/// in a single script. 
/// </summary>
public class TcpHelloWorld : MonoBehaviour
{
    /// <summary>
    /// The server component of the client / server application.
    /// </summary>
    private TcpListener _server;

    /// <summary>
    /// The client component of the client / server application.
    /// </summary>
    private TcpClient _client;

    /// <summary>
    /// The listening port of the server. Change this if you are having
    /// problems connecting to the server. There may be another service
    /// running on this port on your machine.
    /// </summary>
    [Tooltip("The listening port of the server")]
    public int _port = 54000;

    /// <summary>
    /// Start the server and then the client.
    /// </summary>
    void Start()
    {
        // Create an instance of TcpListener and start it. Before you use
        // functions like BeginAcceptTcpClient() you must start the server
        _server = new TcpListener(IPAddress.Any, _port);
        _server.Start();

        // Start accepting new connections. This is performed asyncronously
        // so it needs an AsyncCallback method passed to it
        _server.BeginAcceptTcpClient(Server_AcceptConnection, null);

        // Create the client and connect to the server that happens to be
        // running on the localhost. Notice that the port number is the same
        // because we want to connect to the service running on that port which
        // is our "Hello" server.
        _client = new TcpClient();
        _client.Connect("127.0.0.1", _port);

        // Set up the return message receiver from the server. A buffer is
        // created to store the received message and an AsyncCallback is created
        // to print the message to the console.
        var buffer = new byte[256];
        AsyncCallback messageReceived = (ar) =>
        {
            if (ar.IsCompleted)
            {
                // End the read async operation and this will return the
                // number of bytes received.
                var bytes = _client.GetStream().EndRead(ar);

                // Convert the bytes received to a string and print the
                // contents of the message to the console.
                var msg = Encoding.ASCII.GetString(buffer, 0, bytes);
                print(msg);
            }
        };

        // Now that the buffer and async callback are created we can tell the
        // client to listen for incoming messages
        _client.GetStream().BeginRead(buffer, 0, buffer.Length, messageReceived, null);

        // The last thing to do is to send a message to the server to let it
        // know that we're friendly.
        var message = Encoding.ASCII.GetBytes("Hello, Server!");
        _client.GetStream().Write(message, 0, message.Length);
    }

    /// <summary>
    /// Server side. Accept incoming connections from a remote client.
    /// </summary>
    /// <param name="ar">Async result</param>
    private void Server_AcceptConnection(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            // End the async accept client operation and this returns an
            // instance of TcpClient. We can use this instance to talk to
            // the connected client.
            var client = _server.EndAcceptTcpClient(ar);

            // Before we can do that, let's set up a buffer to hold any
            // received messages and an async callback when a message is
            // received.
            var buffer = new byte[256];
            AsyncCallback acceptMessage = (a) =>
            {
                if (a.IsCompleted)
                {
                    // As with the client, end the async read and this will
                    // return the number of bytes received.
                    var bytes = client.GetStream().EndRead(a);

                    // Convert the buffer to a string and display it in the
                    // console window.
                    var str = Encoding.ASCII.GetString(buffer, 0, bytes);
                    print(str);

                    // Finally, send a message back to the connected client
                    // with a friendly greeting.
                    var returnMessage = Encoding.ASCII.GetBytes("Hello, Client!");
                    client.GetStream().Write(returnMessage, 0, returnMessage.Length);

                    // In this example, the connection is terminated at this point.
                    client.Close();
                }
            };

            // Now that the buffer and async callback are set up, start waiting
            // for a message to be sent from the client. 
            client.GetStream().BeginRead(buffer, 0, buffer.Length, acceptMessage, null);
        }
    }
}
