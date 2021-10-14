using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Tic_Tac_Toe_Test_Client
{
    public partial class TicTacToeTestForm : Form
    {
        byte[] _buffer = new byte[1024];
        TcpClient _client;

        public TicTacToeTestForm()
        {
            InitializeComponent();
        }

        public void Connect(object sender, EventArgs e)
        {
            _client = new TcpClient(AddressFamily.InterNetwork);
            try
            {
                var address = txtIpAddress.Text;
                var port = int.Parse(txtPort.Text);

                _client.Connect(address, port);
                _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Message_Received, null);
                txtOutput.AppendText("Connected!\r\n");
            }
            catch (SocketException ex)
            {
                txtOutput.AppendText($"{ex}\r\n");
            }
        }

        public void Disconnect(object sender, EventArgs e)
        {
            _client.Close();
            _client.Dispose();
        }

        public void Cell_Clicked(object sender, EventArgs e)
        {
            var cellIndex = int.Parse((sender as Button).Tag.ToString());
            SendMessage($"{GameCommand.MakeMove}:2:{cellIndex}"); // Hard coded to always be the second player
        }

        private void SendMessage(string message)
        {
            var bytes = Encoding.ASCII.GetBytes(message);
            _client.GetStream().Write(bytes, 0, bytes.Length);
        }

        private void Message_Received(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                var recv = _client.GetStream().EndRead(ar);
                if (recv == 0)
                {
                    BeginInvoke((Action)(() => AppendText("SERVER DISCONNECTED!")));
                }
                else
                {
                    var msg = Encoding.ASCII.GetString(_buffer, 0, recv);
                    BeginInvoke((Action)(() => AppendText(msg)));
                    Array.Clear(_buffer, 0, _buffer.Length);
                    _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Message_Received, null);
                }
            }
        }

        private void AppendText(string text)
        {
            txtOutput.AppendText($"{text}\r\n");
        }
    }
}
