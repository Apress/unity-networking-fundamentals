using NetLib;
using System.Collections.Generic;

namespace TicTacToeNetworkGame
{
    public class TicTacToeServer : NetworkServer<GameMessage>
    {
        private readonly List<NetworkClient> _players = new List<NetworkClient>();

        public TicTacToeServer(int port)
            : base(port, 2)
        {

        }

        public void StartGame()
        {
            SetCurrentPlayer(1, new int[9], MessageType.ServerStartGame);
        }

        public void SetCurrentPlayer(int currentPlayer, int[] boardState, MessageType type = MessageType.ServerTogglePlayer)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (i + 1 == currentPlayer)
                {
                    var activeMessage = GameSerialization
                                        .CreateMessage(currentPlayer, type, boardState);
                    _players[i].Send(activeMessage);
                }
                else
                {
                    var inactiveMessage = GameSerialization
                                          .CreateMessage(-1, type, boardState);
                    _players[i].Send(inactiveMessage);
                }
            }
        }

        public bool IsCurrentPlayer(NetworkClient client, int currentPlayer)
        {
            return (_players.IndexOf(client) + 1) == currentPlayer;
        }

        public void ShowPodium(int currentPlayer, int[] cells)
        {
            Broadcast(GameSerialization.CreatePodium(currentPlayer, cells));
        }

        protected override GameMessage CreatePayload(byte[] message)
        {
            return GameSerialization.FromBytes(message);
        }

        protected override void OnClientConnected(NetworkClient networkClient)
        {
            base.OnClientConnected(networkClient);
            _players.Add(networkClient);
        }
    }
}
