using System;
using System.Collections.Generic;
using System.Linq;

public class TicTacToeGameEngine
{
    private readonly List<int[]> _matches = new List<int[]>();

    public int CurrentPlayer { get; private set; } = 1;

    public int Winner { get; private set; } = 0;

    public TicTacToeGameState State { get; private set;} = TicTacToeGameState.Inactive;

    public int[] Cells { get; private set; }

    public TicTacToeGameEngine()
    {
        Cells = new int[9];
        _matches = new List<int[]> {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 },
                new int[] { 0, 3, 6 },
                new int[] { 1, 4, 7 },
                new int[] { 2, 5, 8 },
                new int[] { 0, 4, 8 },
                new int[] { 2, 4, 6 }
            };
    }

    public void Reset()
    {
        Array.Clear(Cells, 0, Cells.Length);
        CurrentPlayer = 1;
        Winner = 0;
        State = TicTacToeGameState.Playing;
    }

    public TicTacToeGameState MakeMove(int cellIndex)
    {
        if (Cells[cellIndex] == 0)
        {
            Cells[cellIndex] = CurrentPlayer;

            if (IsWinner())
            {
                State = TicTacToeGameState.Podium;
                Winner = CurrentPlayer;
            }
            else
            {
                if (!BoardHasEmptySlots())
                {
                    State = TicTacToeGameState.Podium;
                    Winner = 0;
                }
                else
                {
                    CurrentPlayer = 1 + (CurrentPlayer % 2);
                }
            }
        }

        return State;
    }

    private bool IsWinner()
    {
        foreach (var idxList in _matches)
        {
            if (IsSame(idxList[0], idxList[1], idxList[2]))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSame(int index1, int index2, int index3)
    {
        return Cells[index1] == CurrentPlayer &&
               Cells[index2] == CurrentPlayer &&
               Cells[index3] == CurrentPlayer;
    }

    private bool BoardHasEmptySlots()
    {
        return Cells.Count(c => c == 0) > 0;
    }
}