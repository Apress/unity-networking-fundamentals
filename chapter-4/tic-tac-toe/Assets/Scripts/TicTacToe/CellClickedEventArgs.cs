using System;

public class CellClickedEventArgs : EventArgs
{
    public int CellIndex { get; }

    public CellClickedEventArgs(int cellIndex)
    {
        CellIndex = cellIndex;
    }
}