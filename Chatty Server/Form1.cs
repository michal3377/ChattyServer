using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Chatty_Server
{
   
    /// <summary>
    /// Projekt serwera i klienta TCP, oparty asynchronicznych socketach.
    /// Projekt ma na zadanie przedstawić aplikację czatu w ujęciu obiektowym,
    /// gdzie funkcjonalność jest oddzielona od interfejsu graficznego, a kod
    /// napisany jest w wystarczającej abstrakcji umożliwiającej zarówno własną implementację
    /// oraz edycję poszczególnych działań i zachowań jak i przyszły ich rozwój w elastyczny sposób.
    /// 
    /// Michał Czopek, Krzysztof Szymański, 3ic1
    /// </summary>

    public partial class Form1 : Form
    {
       
        private ChatManager chatManager;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UIAgent agent = new UIAgent(tbServerLog, lbUsers);

            chatManager = new ChatManager(agent);
            
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            chatManager.sendBroadcastMsg(tbMsg.Text);
        }


        private void btStartServer_Click(object sender, EventArgs e)
        {
            chatManager.startServer(tbIp.Text, Decimal.ToInt32(nudPort.Value));

        }
    }

    /// <summary>
    /// Klasa pośrednicząca między mechanizmem serwera a interfejsem użytkownika.
    /// Wystarczy jej niewielka edycja, by przerobić serwer na wersję konsolową.
    /// </summary>
    public class UIAgent
    {
        private CheckedListBox lbUsers;
        private TextBox tbLog;
        public UIAgent(TextBox tb, CheckedListBox lb)
        {
            tbLog = tb;
            lbUsers = lb;
        }

        public void log(string msg)
        {
            tbLog.Invoke((MethodInvoker)(() => tbLog.AppendText(msg + Environment.NewLine)));
         
        }

        public void addUser(string name)
        {
            lbUsers.Invoke((MethodInvoker)(() => lbUsers.Items.Add(name)));
        }

        public void removeUser(string name)
        {
            lbUsers.Invoke((MethodInvoker)(() => lbUsers.Items.Remove(name)));
        }



    }


}
