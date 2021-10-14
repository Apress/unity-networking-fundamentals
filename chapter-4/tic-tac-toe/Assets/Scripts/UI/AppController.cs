using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public PanelController _panels;

    private TicTacToeGameServer _gameController;
    private TicTacToeServer _server;

    private List<TicTacToeClientBehaviour> _clients;

    [Tooltip("The port number the Tic Tac Toe server is running")]
    public int _port = 9021;

    void Start()
    {
        _clients = new List<TicTacToeClientBehaviour>();

        _panels.GetPanel<MainTitlePanel>().StartServerClicked += StartServer;
        _panels.GetPanel<MainTitlePanel>().JoinServerClicked += JoinServer;
        _panels.GetPanel<StartServerPanel>().CancelClicked += CancelServer;
    }

    private void OnDestroy()
    {
        _panels.GetPanel<MainTitlePanel>().StartServerClicked -= StartServer;
        _panels.GetPanel<MainTitlePanel>().JoinServerClicked -= JoinServer;
    }

    private void ShowError(string error)
    {
        _panels.GetPanel<ErrorPanel>()._text.text = error;
        StartCoroutine(ShowErrorPanel());
    }
    
    private void JoinServer(object sender, EventArgs e)
    {
        CreateClient();
    }

    private void CancelServer(object sender, EventArgs e)
    {
        StopServer();
    }

    private void StartServer(object sender, EventArgs e)
    {
        if (_server == null)
        {
            _server = new TicTacToeServer(_port);
            _gameController = new TicTacToeGameServer(_server);
            _server.Start();
        }

        CreateClient(true);
        _panels.ShowPanel(PanelType.StartServer);
    }

    public void StopServer()
    {
        _server?.Stop();
        _gameController = null;
        _server = null;

        _panels.ShowPanel(PanelType.Title);
        ClearClients();
    }

    private void ClearClients()
    {
        foreach (var c in _clients)
        {
            Destroy(c.gameObject);
        }
        _clients.Clear();
    }

    private void CreateClient(bool local = false)
    {
        var go = new GameObject("Client");
        var client = go.AddComponent<TicTacToeClientBehaviour>();
        client._app = this;
        client._board = FindObjectOfType<BoardController>();

        var address = IPAddress.Parse("127.0.0.1");

        if (!local)
        {
            var userEnteredAddress = _panels.GetPanel<MainTitlePanel>().ServerAddress;
            address = GetAddress(userEnteredAddress);
        }

        if (address == IPAddress.None)
        {
            ShowError("Invalid IP Address!");
        }
        else
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(address, _port);
            client.Connect(tcpClient);
        }

        _clients.Add(client);
    }

    public IPAddress GetAddress(string ipAddress)
    {
        try
        {
            var address = IPAddress.Parse(ipAddress);
            return address;
        }
        catch (FormatException)
        {
            return IPAddress.None;
        }
    }

    private IEnumerator ShowErrorPanel(float duration = 3)
    {
        _panels.ShowPanel(PanelType.Error);
        yield return new WaitForSeconds(duration);
        _panels.ShowPanel(PanelType.Title);
    }
}
