using System.Collections.Generic;

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

    public void QuitToTitle()
    {
        _players.ForEach((p) => p.Send(GameSerialization.CreateClientQuit()));
    }

    public void SetCurrentPlayer(int currentPlayer, int[] boardState, MessageType type = MessageType.ServerTogglePlayer)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            var player = (i + 1 == currentPlayer) ? currentPlayer : -1;
            var message = GameSerialization
                            .CreateMessage(player, type, boardState);

            _players[i].Send(message);
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
