using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{
    public delegate void ServerUserCallback(Socket socket);
    public delegate void ServerDataCallback(string data, Socket socket);

    /// <summary>
    /// Główna klasa zarządzająca czatem. Obsługuje polecenia które przychodzą do serwera, 
    /// przechowuje informacje dotyczące aktualnie zalogowanych użytkowników i utworzonych pokoi czatowych.
    /// </summary>
    class ChatManager
    {

        ClientMessageParser parser = new ClientMessageParser();
        private UIAgent ui;
        public ChatMessenger messenger;
        public Dictionary<string, ChatUser> loggedChatUsers { get; set; }
        public Dictionary<string, ChatUser> pendingChatUsers { get; set; }
        public Dictionary<string, ChatRoom> chatRooms { get; set; }

        public ChatServer server;

        private string waitingRoomName = "Poczekalnia";

        /// <summary>
        /// Publiczny konstruktor inicjalizujący komponenty . Utworzenie faktycznego serwera sieciowego,
        /// dopiero w metodzie <see cref="startServer"/>
        /// </summary>
        /// <param name="agent"></param>
        public ChatManager(UIAgent agent)
        {

            ui = agent;
            server = new ChatServer(agent, onUserConnected, onUserDisconnected, onDataReceived);


            loggedChatUsers = new Dictionary<string, ChatUser>();
            pendingChatUsers = new Dictionary<string, ChatUser>();
            chatRooms = new Dictionary<string, ChatRoom>();
            messenger = new ChatMessenger(ui, loggedChatUsers, chatRooms);

            var cr1 = new ChatRoom("Pokój #1");
            var cr2 = new ChatRoom("Pokój #2");
            var cr3 = new ChatRoom("Pokój #3");
            var cr4 = new ChatRoom(waitingRoomName);

            chatRooms.Add(cr4.name, cr4);
            chatRooms.Add(cr1.name, cr1);
            chatRooms.Add(cr2.name, cr2);
            chatRooms.Add(cr3.name, cr3);
        }


        public void startServer(string ip, int port){
            server.initServer(ip, port);
        }

        private void onUserDisconnected(Socket socket)
        {
            var user = findChatUserBySocket(socket);
            if(user != null)
                disconnectUser(user);
        }
        private void onUserConnected(Socket socket)
        {
            var user = new ChatUser(socket);
            addPendingUser(user);
        }



        private void onDataReceived(string data, Socket socket)
        {
            var user = findChatUserBySocket(socket);
            handleMessage(data, user);
        }

        public void addPendingUser(ChatUser user)
        {
            pendingChatUsers.Add(user.guid, user);
            ui.log("Liczba użytkowników: " + getChatUsersCount().ToString());
        }

        public void sendChatTree()
        {
            messenger.sendChatTree();
        }

        private void loginUser(ChatUser user, string name)
        {
            name = getAvailableUsername(name);
            user.logIn(name);
            loggedChatUsers.Add(name, user);
            pendingChatUsers.Remove(user.guid);
            joinRoom(user, waitingRoomName);
            ui.addUser(name);
            messenger.sendChatTree();
            //messenger.sendMessageToTheUser(user, "Rozmawiasz jako '" + name + "'");

            
        }
        public void disconnectUser(ChatUser user)
        {
            if (user.loggedIn)
            {
                leaveRoom(user);
                loggedChatUsers.Remove(user.name);
                ui.removeUser(user.name);
            }
            else
            {
                pendingChatUsers.Remove(user.guid);
            }
            messenger.sendChatTree();
            messenger.sendServerBroadcastMsg("Użytkownik " + user.name + " wyszedł.");
        }

        public int getChatUsersCount()
        {
            return pendingChatUsers.Count + loggedChatUsers.Count;
        }

        public ChatUser findChatUserBySocket(Socket userSocket)
        {
            try
            {
                foreach (var user in loggedChatUsers)
                {
                    if (user.Value.socket.RemoteEndPoint.ToString().Equals(userSocket.RemoteEndPoint.ToString()))
                    {
                        return user.Value;
                    }
                }
                foreach (var user in pendingChatUsers)
                {
                    if (user.Value.socket.RemoteEndPoint.ToString().Equals(userSocket.RemoteEndPoint.ToString()))
                    {
                        return user.Value;
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return null;
            
        }

        private void joinRoom(ChatUser user, string roomName)
        {
            try
            {
                leaveRoom(user);
                user.roomName = roomName;
                chatRooms[roomName].addMember(user.name);
                messenger.sendChatTree();
                ui.log(user.name + " dołączył do pokoju " + roomName);
                messenger.sendServerMessageToTheRoom(roomName, "'" + user.name + "' dołączył do pokoju \"" + roomName + "\"");
            }
            catch (Exception)
            {
                ui.log(user.name + " NIE dołączył do pokoju " + roomName);
            }
            
            //sendMessage(userGuid, "Rozmawiasz teraz w pokoju \"" + roomName + "\"");
        }

        private void leaveRoom(ChatUser user)
        {
            string roomName = user.roomName;
            if (roomName != "")
            {
                try
                {
                    chatRooms[roomName].removeMember(user.name);

                    messenger.sendServerMessageToTheRoom(roomName, "'" + user.name + "' opuścił pokój \"" + roomName + "\"");
                    user.roomName = "";
                }
                catch (Exception)
                {
                    ui.log("Nie znaleziono pokoju " + roomName);
                }
                
            }

        }

        private string createRoom(string name)
        {
            var roomname = getAvailableRoomName(name);
            var room = new ChatRoom(roomname);
            chatRooms.Add(roomname, room);
            messenger.sendChatTree();
            return roomname;
        }


        private string getAvailableUsername(string name)
        {
            if (name == "")
            {
                name = "User";
            }
            if (!isUserNameAvailable(name))
            {
                name = getAvailableUsername(name + "*");
            }
            return name;
        }

        private bool isUserNameAvailable(string name)
        {
            foreach (var item in loggedChatUsers)
            {
                if (item.Value.name == name)
                {
                    return false;
                }
            }
            return true;
        }
        private string getAvailableRoomName(string name)
        {
            if (name == "")
            {
                name = "Room";
            }
            if (!isRoomNameAvailable(name))
            {
                name = getAvailableRoomName(name + "*");
            }
            return name;
        }

        private bool isRoomNameAvailable(string name)
        {
            foreach (var item in chatRooms)
            {
                if (item.Value.name == name)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Metoda interpretujaca polecenia
        /// </summary>
        /// <param name="msg">Niesparsowane polecenie</param>
        /// <param name="user">Autor polecenia</param>
        private void handleMessage(string msg, ChatUser user)
        {
            switch (parser.getMsgType(msg))
            {
                case "login":
                    var t = parser.parseLoginMsg(msg);
                    loginUser(user, t);

                    break;
                case "join":
                    var r = parser.parseJoinMsg(msg);
                    joinRoom(user, r);
                    break;

                case "roomMsg":
                    var roomMsg = parser.parseRoomMsg(msg);
                    var rName = user.roomName;
                    messenger.sendMessageToTheRoom(rName, roomMsg, user.name);
                    break;

                case "createRoom":
                    var roomName = parser.parseCreateRoomMsg(msg);
                    var realRoomName = createRoom(roomName);
                    joinRoom(user, realRoomName);
                    break;
            }
        }

        public void sendBroadcastMsg(string msg)
        {
            messenger.sendServerBroadcastMsg("server: " + msg);
        }

    }
}
