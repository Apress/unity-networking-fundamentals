namespace TicTacToePrototype
{
    partial class Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartServer = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnClientOnly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartServer
            // 
            this.StartServer.Location = new System.Drawing.Point(12, 12);
            this.StartServer.Name = "StartServer";
            this.StartServer.Size = new System.Drawing.Size(104, 23);
            this.StartServer.TabIndex = 0;
            this.StartServer.Text = "Start Server On";
            this.StartServer.UseVisualStyleBackColor = true;
            this.StartServer.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(122, 14);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "9021";
            // 
            // btnClientOnly
            // 
            this.btnClientOnly.Location = new System.Drawing.Point(12, 59);
            this.btnClientOnly.Name = "btnClientOnly";
            this.btnClientOnly.Size = new System.Drawing.Size(104, 23);
            this.btnClientOnly.TabIndex = 2;
            this.btnClientOnly.Text = "Start Client";
            this.btnClientOnly.UseVisualStyleBackColor = true;
            this.btnClientOnly.Click += new System.EventHandler(this.btnClientOnly_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClientOnly);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.StartServer);
            this.Name = "Server";
            this.Text = "Tic-Tac-Toe Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnClientOnly;
    }
}

