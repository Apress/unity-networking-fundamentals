using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToeNetworkGame;

namespace TicTacToePrototype
{


    public partial class Server : Form
    {
        private TicTacToeServer _server;
        private TicTacToeGameController _controller;

        public Server()
        {
            InitializeComponent();
        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            _server = new TicTacToeServer(int.Parse(txtPort.Text));
            _controller = new TicTacToeGameController(_server);

            _server.Start();

            var c1 = new ClientForm();
            var c2 = new ClientForm();

            c1.Port = int.Parse(txtPort.Text);
            c2.Port = int.Parse(txtPort.Text);

            c1.Text = "Player One";
            c2.Text = "Player Two";

            c1.Show();
            c2.Show();
        }

        private void btnClientOnly_Click(object sender, EventArgs e)
        {
            var c1 = new ClientForm();
            c1.Port = int.Parse(txtPort.Text);
            c1.Text = "Player Two";
            c1.Show();
        }
    }
}
