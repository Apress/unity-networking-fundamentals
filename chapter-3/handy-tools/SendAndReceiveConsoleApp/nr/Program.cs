using System;

namespace SloanKelly.Networking.NetCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config(args);
            if (config.AskForHelp || string.IsNullOrEmpty(config.Filename))
            {
                Console.WriteLine("NetCopy - Sloan Kelly 2020");
                Console.WriteLine("Provides a simple peer to peer copy from one machine to another");
                Console.WriteLine("Usage");
                Console.WriteLine("\tSend\tncp [-ip serverIP] [-p port] filename");
                Console.WriteLine("\tReceive\tncp [-p port] filename");
            }
            else if (config.IsServer)
            {
                var server = new Receiver(config.ServerIP,
                                          config.Port,
                                          config.Filename,
                                          config.Debug);
                server.Run();
            }
            else
            {
                var sender = new Sender(config.ServerIP,
                                        config.Port,
                                        config.Filename,
                                        config.Debug);
                sender.Run();
            }
        }
    }
}
