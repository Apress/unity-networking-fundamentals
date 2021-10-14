using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// The TicTacToeClientBehaviour class is used to glue the network client
/// to the UI. It contains references to the AppController and BoardController.
/// This class is not directly created in the scene but is attached to a 
/// game object at run time when a client needs to be created.
/// 
/// It uses an internal queue to perform actions on the main thread. No changes
/// to scene objects can be made on any thread other than the main thread!
/// </summary>
public class TicTacToeClientBehaviour : MonoBehaviour
{
    private int _playerID;

    private NetworkClient _networkClient;
    private TicTacToeClient _client;

    public AppController _app;
    public BoardController _board;

    private Queue<Action> _actions;

    void Awake()
    {
        _actions = new Queue<Action>();
    }

    void Update()
    {
        lock(_actions)
        {
            while (_actions.Count > 0)
            {
                _actions.Dequeue().Invoke();
            }
        }
    }

    private void OnDestroy()
    {
        lock (_actions)
        {
            _actions.Clear();
        }

        _client.ShowPodium -= Client_ShowPodium;
        _client.StartGame -= Client_StartGame;
        _client.TogglePlayer -= Client_TogglePlayer;
        _client.ReturnToTitle -= Client_ReturnToTitle;

        _board.CellClicked -= BoardCell_Clicked;
        _board.PlayAgainClicked -= PlayAgain_Clicked;
        _board.ReturnToTitleClicked -= ReturnToTitle_Clicked;

        _client.Cleanup();
    }

    public void Connect(TcpClient tcpClient)
    {
        _networkClient = new NetworkClient(tcpClient);
        _client = new TicTacToeClient(_networkClient);
        _client.ShowPodium += Client_ShowPodium;
        _client.StartGame += Client_StartGame;
        _client.TogglePlayer += Client_TogglePlayer;
        _client.ReturnToTitle += Client_ReturnToTitle;

        _board.CellClicked += BoardCell_Clicked;
        _board.PlayAgainClicked += PlayAgain_Clicked;
        _board.ReturnToTitleClicked += ReturnToTitle_Clicked;
    }

    private void Client_ReturnToTitle(object sender, EventArgs e)
    {
        Action action = () => _app.StopServer();
        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }

    private void ReturnToTitle_Clicked(object sender, EventArgs e)
    {
        _client.ReturnToLobby();
    }

    private void PlayAgain_Clicked(object sender, EventArgs e)
    {
        _client.PlayAgain();
    }

    private void BoardCell_Clicked(object sender, CellClickedEventArgs e)
    {
        _client.MakeMove(e.CellIndex);
    }

    private void Client_TogglePlayer(object sender, GameMessageEventArgs e)
    {
        lock (_actions)
        {
            _actions.Enqueue(() =>
            {
                _board.ToggleCellButtons(_playerID == e.Message.playerId);
                _board.UpdateBoard(e.Message.boardState);
            });
        }
    }

    private void Client_StartGame(object sender, GameMessageEventArgs e)
    {
        Action action = () =>
        {
            _playerID = e.Message.playerId < 0 ? 2 : e.Message.playerId;
            _board.ResetBoard(e.Message.boardState);
            _board.ToggleCellButtons(_playerID == 1);
            _app._panels.ShowPanel(PanelType.Play);
        };

        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }

    private void Client_ShowPodium(object sender, GameMessageEventArgs e)
    {
        Action action = () =>
        {
            _board.UpdateBoard(e.Message.boardState);
            _board.ToggleCellButtons(false);
            _board.ToggleActionButtons(true);
            _board.BoardWinner(e.Message.playerId);
        };

        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }
}