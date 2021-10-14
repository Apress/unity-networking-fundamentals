using System.Collections;
using System.Collections.Generic;

namespace NetLib
{
    public class NetworkClientCollection : IEnumerable<NetworkClient>
    {
        private readonly List<NetworkClient> _clients = new List<NetworkClient>();

        public int Count => _clients.Count;

        public event MessageReceivedEventHandler MessageReceived;

        public void Add(NetworkClient client)
        {
            _clients.Add(client);
            client.MessageReceived += Client_MessageReceived;
        }

        public IEnumerator<NetworkClient> GetEnumerator()
        {
            return _clients.GetEnumerator();
        }

        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(sender, e);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
