using NetLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;
using TicTacToeNetworkGame;
using TicTacToeNetworkGame.Events;

namespace TicTacToePrototype
{
    public partial class ClientForm : Form
    {
        private TcpClient _tcpClient;
        private NetworkClient _nc;
        private TicTacToeClient _client;

        private readonly List<Action> _messages = new List<Action>();
        private readonly Timer _timer = new Timer();
        private readonly Button[] _buttons = new Button[9];

        public int Port { get; set; }

        public ClientForm()
        {
            InitializeComponent();
            _buttons[0] = button1;
            _buttons[1] = button2;
            _buttons[2] = button3;
            _buttons[3] = button4;
            _buttons[4] = button5;
            _buttons[5] = button6;
            _buttons[6] = button7;
            _buttons[7] = button8;
            _buttons[8] = button9;

            foreach (var button in _buttons)
            {
                button.Click += (o, e) =>
                {
                    var index = Array.IndexOf(_buttons, o);
                    _client.MakeMove(index);
                };
            }

            ClearButtons();
            ToggleButtons(false);

            _timer.Interval = 500;
            _timer.Tick += Timer_Tick;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            while (_messages.Count > 0)
            {
                var message = _messages[0];
                _messages.RemoveAt(0);
                BeginInvoke(message);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _tcpClient = new TcpClient(AddressFamily.InterNetwork);
            _tcpClient.Connect("127.0.0.1", Port);
            _nc = new NetworkClient(_tcpClient);

            _client = new TicTacToeClient(_nc);
            _client.ShowPodium += TSShowPodium;
            _client.StartGame += TSStartGame;
            _client.TogglePlayer += TSTogglePlayer;
            _client.CloseWindow += Client_CloseWindow;
        }

        private void Client_CloseWindow(object sender, EventArgs e)
        {
            _client.ShowPodium -= TSShowPodium;
            _client.StartGame -= TSStartGame;
            _client.TogglePlayer -= TSTogglePlayer;
            _client.CloseWindow -= Client_CloseWindow;

            _tcpClient.Close();
            _tcpClient.Dispose();
            Close();
        }

        private void TSTogglePlayer(object sender, GameMessageEventArgs e)
        {
            _messages.Add(() => TogglePlayer(sender, e));
        }

        private void TSStartGame(object sender, GameMessageEventArgs e)
        {
            _messages.Add(() => StartGame(sender, e));
        }

        private void TSShowPodium(object sender, GameMessageEventArgs e)
        {
            _messages.Add(() => ShowPodium(sender, e));
        }

        private void UpdateBoard(int[] boardState)
        {
            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] == 1)
                {
                    _buttons[i].Text = "X";
                    _buttons[i].Enabled = false;
                }
                else if (boardState[i] == 2)
                {
                    _buttons[i].Text = "O";
                    _buttons[i].Enabled = false;
                }
            }
        }

        private void TogglePlayer(object sender, GameMessageEventArgs e)
        {
            var enabled = e.Message.playerId > 0;
            ToggleButtons(enabled);
            UpdateBoard(e.Message.boardState);
        }

        private void StartGame(object sender, GameMessageEventArgs e)
        {
            ClearButtons();
            var enabled = e.Message.playerId > 0;
            ToggleButtons(enabled);
        }

        private void ShowPodium(object sender, GameMessageEventArgs e)
        {
            UpdateBoard(e.Message.boardState);

            if (e.Message.playerId == 0)
            {
                Podium.Text = "Cat's Game!";
            }
            else
            {
                Podium.Text = $"Congratulations! Player {e.Message.playerId} is the WINNER!";
            }

            ToggleButtons(false);

            PlayAgain.Enabled = true;
            ReturnToLobby.Enabled = true;
        }

        private void ClearButtons()
        {
            PlayAgain.Enabled = false;
            ReturnToLobby.Enabled = false;
            Podium.Text = "Play Till There is a Winner!";

            for (int i = 0; i < 9; i++)
            {
                _buttons[i].Text = (i + 1).ToString();
            }
        }

        private void ToggleButtons(bool enabled)
        {
            foreach (var b in _buttons)
            {
                b.Enabled = enabled;
            }
        }

        private void PlayAgain_Click(object sender, EventArgs e)
        {
            _client.PlayAgain();
        }

        private void ReturnToLobby_Click(object sender, EventArgs e)
        {
            _client.ReturnToLobby();
        }
    }
}
