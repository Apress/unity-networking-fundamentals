using System;

public class TicTacToeClient
{
    private readonly NetworkClient _client;

    public event EventHandler<GameMessageEventArgs> StartGame;
    public event EventHandler<GameMessageEventArgs> TogglePlayer;
    public event EventHandler<GameMessageEventArgs> ShowPodium;
    public event EventHandler ReturnToTitle;

    public TicTacToeClient(NetworkClient client)
    {
        _client = client;
        _client.MessageReceived += MessageReceived;
    }

    public void PlayAgain()
    {
        _client.Send(GameSerialization.CreatePlayAgain());
    }

    public void ReturnToLobby()
    {
        _client.Send(GameSerialization.CreateClientQuit());
    }

    public void MakeMove(int index)
    {
        _client.Send(GameSerialization.CreateMove(0, index));
    }

    public void Cleanup()
    {
        _client.MessageReceived -= MessageReceived;
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        var data = new byte[e.Data.Length];
        Array.Copy(e.Data, data, e.Data.Length);

        var message = GameSerialization.FromBytes(data);
        switch (message.messageType)
        {
            case MessageType.ServerStartGame:
                StartGame?.Invoke(this, new GameMessageEventArgs(message));
                break;
            case MessageType.ServerTogglePlayer:
                TogglePlayer?.Invoke(this, new GameMessageEventArgs(message));
                break;
            case MessageType.ServerShowPodium:
                ShowPodium?.Invoke(this, new GameMessageEventArgs(message));
                break;
            case MessageType.ClientQuit:
                ReturnToTitle?.Invoke(this, EventArgs.Empty);
                break;
        }
    }
}
