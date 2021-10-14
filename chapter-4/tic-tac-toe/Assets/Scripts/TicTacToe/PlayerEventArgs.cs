using System;

public class PlayerEventArgs : EventArgs
{
    public int PlayerIndex { get; }

    public PlayerEventArgs(int playerIndex)
    {
        PlayerIndex = playerIndex;
    }
}