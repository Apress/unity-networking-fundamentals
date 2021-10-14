using NetLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeNetworkGame
{
    public class TicTacToeGameController
    {
        private readonly int[] _cells;
        private readonly List<int[]> _matches = new List<int[]>();
        private readonly TicTacToeServer _server;
        private int _currentPlayer = 1;
        private TicTacToeGameState _state = TicTacToeGameState.Inactive;

        public event EventHandler ClientQuit;

        public TicTacToeGameController(TicTacToeServer server)
        {
            _server = server;
            _server.PayloadReceived += Server_PayloadReceived;
            _server.ClientListFull += StartTheRound;

            _cells = new int[9];
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

        private void StartTheRound(object sender, EventArgs e)
        {
            Array.Clear(_cells, 0, _cells.Length);
            _currentPlayer = 1;
            _state = TicTacToeGameState.Playing;
            _server.StartGame();
            _server.SetCurrentPlayer(_currentPlayer, _cells);
        }

        private void MakeMove(int[] boardState)
        {
            var cellIndex = boardState[0];
            if (_cells[cellIndex] == 0)
            {
                _cells[cellIndex] = _currentPlayer;

                if (IsWinner())
                {
                    _state = TicTacToeGameState.Podium;
                    _server.ShowPodium(_currentPlayer, _cells);
                }
                else
                {
                    if (!BoardHasEmptySlots())
                    {
                        _state = TicTacToeGameState.Podium;
                        _server.ShowPodium(0, _cells);
                    }
                    else
                    {
                        _currentPlayer = 1 + (_currentPlayer % 2);
                        _server.SetCurrentPlayer(_currentPlayer, _cells);
                    }
                }
            }
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
            return _cells[index1] == _currentPlayer &&
                   _cells[index2] == _currentPlayer &&
                   _cells[index3] == _currentPlayer;
        }

        private bool BoardHasEmptySlots()
        {
            return _cells.Count(c => c == 0) > 0;
        }

        private void Server_PayloadReceived(object sender, PayloadEventArgs<GameMessage> e)
        {
            var client = (NetworkClient)sender;

            switch (_state)
            {
                case TicTacToeGameState.Playing:
                    if (_server.IsCurrentPlayer(client, _currentPlayer))
                    {
                        if (e.Payload.messageType == MessageType.ClientMakeMove)
                        {
                            MakeMove(e.Payload.boardState);
                        }
                    }
                    break;
                case TicTacToeGameState.Podium:
                    if (e.Payload.messageType == MessageType.ClientPlayAgain)
                    {
                        StartTheRound(this, EventArgs.Empty);
                    }
                    else if (e.Payload.messageType == MessageType.ClientQuit)
                    {
                        _state = TicTacToeGameState.Inactive;
                        ClientQuit?.Invoke(this, EventArgs.Empty);
                    }
                    break;
            }
        }
    }
}
