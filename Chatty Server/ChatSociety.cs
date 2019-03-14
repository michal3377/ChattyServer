using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{
    /// <summary>
    /// Klasa reprezentująca model pokoju czatowego
    /// </summary>
    public class ChatRoom
    {
        public string name { get; set; }
        public List<string> members { get; set; }

        public ChatRoom(string name)
        {
            this.name = name;
            members = new List<string>();
        }
        public void addMember(string userName)
        {
            members.Add(userName);
        }

        public void removeMember(string userName)
        {
            members.Remove(userName);
        }

    }

    /// <summary>
    /// Klasa reprezentująca model użytkownika czatu
    /// </summary>
    public class ChatUser
    {
        public string guid { get; set; }
        public Socket socket { get; set; }
        public string name { get; set; }

        public bool loggedIn { get; set; }
        public string roomName { get; set; }
        public ChatUser(Socket socket)
        {
            this.socket = socket;
            guid = Util.generateGuid();
            loggedIn = false;
            roomName = "";
        }

        public void logIn(string nickname)
        {
            name = nickname;
            loggedIn = true;
        }
    }
}
