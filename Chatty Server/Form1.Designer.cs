namespace Chatty_Server
{
    partial class Form1
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
            this.tbServerLog = new System.Windows.Forms.TextBox();
            this.lbUsers = new System.Windows.Forms.CheckedListBox();
            this.btSend = new System.Windows.Forms.Button();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btStartServer = new System.Windows.Forms.Button();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tbServerLog
            // 
            this.tbServerLog.Location = new System.Drawing.Point(12, 93);
            this.tbServerLog.Multiline = true;
            this.tbServerLog.Name = "tbServerLog";
            this.tbServerLog.ReadOnly = true;
            this.tbServerLog.Size = new System.Drawing.Size(339, 440);
            this.tbServerLog.TabIndex = 0;
            this.tbServerLog.WordWrap = false;
            // 
            // lbUsers
            // 
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.Location = new System.Drawing.Point(357, 3);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(176, 289);
            this.lbUsers.TabIndex = 2;
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(276, 558);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 23);
            this.btSend.TabIndex = 3;
            this.btSend.Text = "Send";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(12, 558);
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(258, 20);
            this.tbMsg.TabIndex = 4;
            // 
            // btStartServer
            // 
            this.btStartServer.Location = new System.Drawing.Point(12, 64);
            this.btStartServer.Name = "btStartServer";
            this.btStartServer.Size = new System.Drawing.Size(151, 23);
            this.btStartServer.TabIndex = 8;
            this.btStartServer.Text = "Create server";
            this.btStartServer.UseVisualStyleBackColor = true;
            this.btStartServer.Click += new System.EventHandler(this.btStartServer_Click);
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(47, 38);
            this.nudPort.Maximum = new decimal(new int[] {
            65565,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(116, 20);
            this.nudPort.TabIndex = 9;
            this.nudPort.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // tbIp
            // 
            this.tbIp.Location = new System.Drawing.Point(47, 12);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(116, 20);
            this.tbIp.TabIndex = 10;
            this.tbIp.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 542);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Global message:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 582);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIp);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.btStartServer);
            this.Controls.Add(this.tbMsg);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.lbUsers);
            this.Controls.Add(this.tbServerLog);
            this.Name = "Form1";
            this.Text = "Chatty Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServerLog;
        private System.Windows.Forms.CheckedListBox lbUsers;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btStartServer;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

