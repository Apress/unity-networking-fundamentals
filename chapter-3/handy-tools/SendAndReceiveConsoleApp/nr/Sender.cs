using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SloanKelly.Networking.NetCopy
{
    class Sender
    {
        private IPAddress _serverIP;
        private int _port;
        private string _filename;
        private bool _debug;

        public Sender(IPAddress serverIP, int port, string filename, bool debug)
        {
            _serverIP = serverIP;
            _port = port;
            _filename = filename;
            _debug = debug;
        }

        public void Run()
        {
            var contents = File.ReadAllBytes(_filename);
            var offset = 0;
            var length = contents.Length;

            var socket = Create(_serverIP, _port);
            if (socket == null)
                return;

            socket.Send(BitConverter.GetBytes(contents.Length));

            while (length > 0)
            {
                var sent = socket.Send(contents,
                                       offset,
                                       length,
                                       SocketFlags.None);
                length -= sent;
                offset += sent;
                Console.WriteLine($"Sent {sent} byte(s)");
                socket.Send(contents,
                            offset,
                            length,
                            SocketFlags.None);
            }

            Console.WriteLine("Finished!");
            socket.Close();
            socket.Dispose();
        }

        private Socket Create(IPAddress ip, int port)
        {
            try
            {
                var socket = new Socket(SocketType.Stream,
                                        ProtocolType.Tcp);
                socket.Connect(ip, port);
                return socket;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
