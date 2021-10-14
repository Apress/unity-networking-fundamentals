using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Networking version of the Hello World program using UDP.
/// </summary>
public class MarshalHelloWorld : MonoBehaviour
{
    /// <summary>
    /// The port number the UDP server is listening in on.
    /// </summary>
    public int _port = 9022;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the server.
        var udpClient = new UdpClient(_port);

        // Wait for the client to connect to the server
        udpClient.BeginReceive((ar) =>
        {
            // If the message has been fully received...
            if (ar.IsCompleted)
            {
                // End receiption. This will return the bytes received and get information
                // about the remote device that sent the message
                var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var arr = udpClient.EndReceive(ar, ref remoteEndPoint);

                // From the byte array, reconstruct the message
                var m = CreateMessage(arr);

                // Display the received message to the console
                print($"{m.Text} with version {m.Version} received from {remoteEndPoint.Address} on {remoteEndPoint.Port}");

                // Close the client. Always important to do this :)
                udpClient.Close();
            }
        }, null);

        // For the client it's a simple matter of setting up a socket and giving it an end-point
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // This next line appears to be required on Mac (I didn't have a problem on PC)
        socket.EnableBroadcast = true;

        var remote = new IPEndPoint(GetBroadcastAddress(), _port);

        // Create the structure to send
        var msg = new Message
        {
            Text = "Hello, Udp!",
            Version = 1
        };

        // The 'SendTo' method can be used to send the message to the remote server
        // The CreateArray method converts the structure to a byte array
        socket.SendTo(CreateArray(msg), remote);

        // Always close the socket when you're done using it
        socket.Close();
    }

    /// <summary>
    /// Get the broadcast IP address for the computer's default network card. Broadcast IP address
    /// ends in 255.
    /// </summary>
    /// <returns>The broadcast IP address</returns>
    private IPAddress GetBroadcastAddress()
    {
        // Find the first IPv4 address
        var ipAddress = Dns.GetHostEntry(Dns.GetHostName())
                           .AddressList
                           .FirstOrDefault((e) => e.AddressFamily == AddressFamily.InterNetwork);

        ipAddress = ipAddress != null ? ipAddress : IPAddress.Loopback;
        var bytes = ipAddress.GetAddressBytes();

        // The last byte needs to change to 255 because this isn't a host address, it's the
        // broadcast address.
        bytes[3] = 255;
        return new IPAddress(bytes);
    }

    /// <summary>
    /// Create a serialized array from the structure.
    /// </summary>
    /// <param name="message">Message to serialize</param>
    /// <returns>Serialized structure</returns>
    private byte[] CreateArray(Message message)
    {
        // Create a pointer in memory and allocate the size of the structure
        var size = Marshal.SizeOf(message);
        byte[] buf = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        // Copy the structure to the newly created memory space and then
        // copy it to the byte buffer
        Marshal.StructureToPtr(message, ptr, true);
        Marshal.Copy(ptr, buf, 0, size);

        // Always free your pointers!
        Marshal.FreeHGlobal(ptr);

        return buf;
    }

    /// <summary>
    /// Deserialize the array to the given structure.
    /// </summary>
    /// <param name="array">Array of bytes</param>
    /// <returns>The reconstructed structure</returns>
    private Message CreateMessage(byte[] array)
    {
        Message msg = new Message();

        // Create an area of memory to store the byte array and then copy
        // it to memory
        int size = Marshal.SizeOf(msg);
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(array, 0, ptr, size);

        // Using the PtrToStructure method, copy the bytes out into the
        // Message structure
        msg = (Message)Marshal.PtrToStructure(ptr, msg.GetType());
        Marshal.FreeHGlobal(ptr);
        return msg;
    }
}
