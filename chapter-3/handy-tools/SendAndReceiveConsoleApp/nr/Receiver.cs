using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SloanKelly.Networking.NetCopy
{
    class Receiver
    {
        private IPAddress _serverIP;
        private int _port;
        private string _fileName;
        private bool _debug;

        public Receiver(IPAddress serverIP, int port, string fileName, bool debug)
        {
            _serverIP = serverIP;
            _port = port;
            _fileName = fileName;
            _debug = debug;
        }

        public void Run()
        {
            Console.WriteLine("Listening for connection");
            var contents = ReceiveContents();

            if (contents == null || contents.Length == 0)
            {
                return;
            }
            else
            {
                File.WriteAllBytes(_fileName, contents);
            }
        }

        private byte[] ReceiveContents()
        {
            byte[] superBuffer;

            var listener = new TcpListener(_serverIP, _port);
            listener.Start();
            var socket = listener.AcceptSocket();

            var buffer = new byte[1024];

            var recv = socket.Receive(buffer);
            recv -= 4;
            int length = BitConverter.ToInt32(buffer, 0);
            superBuffer = new byte[length];

            Console.WriteLine($"Size of file received is {length} byte(s)");

            if (_debug)
                Console.WriteLine($"Received {recv} byte(s)");

            int sbOffset = recv;
            int bytesRemaining = length - recv;
            Array.Copy(buffer, 4, superBuffer, 0, sbOffset);
            while (bytesRemaining > 0)
            {
                Array.Clear(buffer, 0, buffer.Length);
                recv = socket.Receive(buffer);
                bytesRemaining -= recv;
                if (_debug)
                    Console.WriteLine($"Received {recv} byte(s). {bytesRemaining} left.");
                Array.Copy(buffer,
                           0,
                           superBuffer,
                           sbOffset,
                           recv);
                sbOffset += recv;
            }

            socket.Close();
            socket.Dispose();
            listener.Stop();

            return superBuffer;
        }
    }
}
