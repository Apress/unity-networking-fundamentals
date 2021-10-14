using System;
using System.Linq;
using System.Text;

public static class GameSerialization
{
    public static byte[] CreateMove(int playerId, int cellIndex)
    {
        return CreateMessage(playerId, MessageType.ClientMakeMove, new int[1] { cellIndex });
    }

    public static byte[] CreatePlayAgain()
    {
        return CreateMessage(0, MessageType.ClientPlayAgain);
    }

    public static byte[] CreateClientQuit()
    {
        return CreateMessage(0, MessageType.ClientQuit);
    }

    public static byte[] CreatePodium(int currentPlayer, int[] boardState)
    {
        return CreateMessage(currentPlayer, MessageType.ServerShowPodium, boardState);
    }

    public static byte[] CreateMessage(int playerId, MessageType type, int[] boardState = null)
    {
        var state = boardState == null || boardState.Length == 0 ? new int[1] { 0 } : boardState;
        var message = new GameMessage
        {
            boardState = state,
            messageType = type,
            playerId = playerId
        };

        return ToBytes(message);
    }

    public static GameMessage FromBytes(byte[] message)
    {
        var str = Encoding.ASCII.GetString(message);
        var split = str.Split(":".ToCharArray());
        var messageType = (MessageType)Enum.Parse(typeof(MessageType), split[0]);
        var playerId = int.Parse(split[1]);
        var payload = split[2].Split(',').Select(int.Parse);

        return new GameMessage
        {
            boardState = payload.ToArray(),
            messageType = messageType,
            playerId = playerId
        };
    }

    public static byte[] ToBytes(GameMessage message)
    {
        var str = $"{message.messageType}:{message.playerId}:{string.Join(",", message.boardState)}";
        return Encoding.ASCII.GetBytes(str);
    }
}