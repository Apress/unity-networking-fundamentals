using System;
using System.Windows.Forms;

namespace TicTacToePrototype
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Server());

            var c1 = new ClientForm();
            c1.Port = int.Parse("9021");
            c1.Text = "Player Two";
            c1.Show();
            Application.Run(c1);



        }
    }
}
