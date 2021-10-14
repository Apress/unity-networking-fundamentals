using System;

public class GameMessageEventArgs : EventArgs
{
    public GameMessage Message { get; }

    public GameMessageEventArgs(GameMessage message)
    {
        Message = message;
    }
}