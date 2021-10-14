using System;
using System.IO;
using System.Net.Sockets;

namespace SendAndReceiveConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("ns filename serverIpAddress port");
                return;
            }

            var file = args[0];
            var ip = args[1];
            var port = int.Parse(args[2]);

            TransferFile(file, ip, port);
        }

        private static void TransferFile(string file, string ip, int port)
        {
            var contents = File.ReadAllBytes(file);
            var offset = 0;
            var length = contents.Length;

            var socket = Create(ip, port);
            if (socket == null)
                return;

            socket.Send(BitConverter.GetBytes(contents.Length));

            while (length > 0)
            {
                var sent = socket.Send(contents, offset, length, SocketFlags.None);
                length -= sent;
                offset += sent;
                Console.WriteLine($"Sent {sent} byte(s)");
                socket.Send(contents, offset, length, SocketFlags.None);
            }

            Console.WriteLine("Finished!");
            socket.Close();
            socket.Dispose();
        }

        private static Socket Create(string ip, int port)
        {
            try
            {
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
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
