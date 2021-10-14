using System.Linq;
using System.Net;

namespace SloanKelly.Networking.NetCopy
{
    class Config
    {
        public string Filename { get; }

        public int Port { get; } = 9021;

        public bool IsServer { get; } = true;

        public bool Debug { get; }

        public IPAddress ServerIP { get; }

        public bool AskForHelp { get; }

        public Config(string[] args)
        {
            ServerIP = IPAddress.Any;

            if (args.Length ==0)
            {
                return;
            }

            int index = 0;
            while (index < args.Length)
            {
                if (IsMatch(args[index], "-ip", "/ip", "ip"))
                {
                    index++;
                    ServerIP = IPAddress.Parse(args[index]);
                    IsServer = false;
                } else if (IsMatch(args[index], "-p", "/p", "port"))
                {
                    index++;
                    Port = int.Parse(args[index]);
                }
                else if (IsMatch(args[index], "-h", "/h", "/help", "help"))
                {
                    AskForHelp = true;
                    return;
                }
                else if(IsMatch(args[index], "-d", "/d"))
                {
                    Debug = true;
                }
                else
                {
                    Filename = args[index];
                }

                index++;
            }
        }

        private bool IsMatch(string leftHand, params string[] rightHand)
        {
            var match = rightHand.FirstOrDefault(s => s == leftHand.ToLower());
            return !string.IsNullOrEmpty(match);
        }
    }
}
