using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{

    /// <summary>
    /// Klasa odpowiadająca za wysyłanie poleceń do użytkowników czatu.
    /// </summary>
    public class ChatMessenger
    {
        private UIAgent ui;
        public Dictionary<string, ChatUser> loggedChatUsers { get; set; }
        public Dictionary<string, ChatRoom> chatRooms { get; set; }
        ServerMessageGenerator generator = new ServerMessageGenerator();

        private readonly string SERVER_NICKNAME = "server";
        public ChatMessenger(UIAgent agent, Dictionary<string, ChatUser> loggedChatUsers,
            Dictionary<string, ChatRoom> chatRooms)
        {
            ui = agent;
            this.loggedChatUsers = loggedChatUsers;
            this.chatRooms = chatRooms;

        }
        public void sendData(Socket socket, string msg)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), socket);
            }
            catch (Exception)
            {
              
            }
            
            //serverSocket.BeginAccept(new AsyncCallback(onUserConnected), null);
        }

        public void sendBroadcastData(string msg)
        {
            foreach (var item in loggedChatUsers)
            {
                //sendMessageToTheUser(item.Value, msg);
                sendData(item.Value.socket, msg);
            }
            ui.log(msg);
        }

        private void sendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }



        #region Text Messages



        public void sendMessageToTheUser(string author, ChatUser user, string msg)
        {
            sendData(user.socket, generator.generateChatAuthorMsg(author, msg));
        }

        public void sendMessageToTheRoom(string roomName, string roomMsg, string author)
        {

            if (roomName != null && chatRooms[roomName] != null)
            {
                var members = chatRooms[roomName].members;
                foreach (string name in members)
                {
                    try
                    {
                        sendMessageToTheUser(author, loggedChatUsers[name], roomMsg);
                    }
                    catch (Exception)
                    {
                        ui.log("User " + name + " not found");
                    }
                }
            }
        }

        //server msgs: 

        public void sendServerMessageToTheUser(ChatUser user, string msg)
        {
            sendData(user.socket, generator.generateServerMsg(msg));
        }

        public void sendServerBroadcastMsg(string msg)
        {
            var str = generator.generateServerMsg(msg);
            sendBroadcastData(str);
        }

        public void sendServerMessageToTheRoom(string roomName, string roomMsg)
        {
            if (roomName != null && chatRooms[roomName] != null)
            {
                var members = chatRooms[roomName].members;
                foreach (string name in members)
                {
                    try
                    {
                        sendServerMessageToTheUser(loggedChatUsers[name], roomMsg);
                    }
                    catch (Exception)
                    {
                        ui.log("User " + name + " not found");
                    }
                }
            }

        }


#endregion


        #region Data Messages

        public void sendUserListMsg()
        {
            var str = generator.generateUserListMsg(loggedChatUsers.Values);
            sendBroadcastData(str);
        }
        public void sendRoomListMsg()
        {
            var str = generator.generateRoomListMsg(chatRooms.Values);
            sendBroadcastData(str);
        }

        public void sendChatTree()
        {
            var str = generator.generateChatTreeMsg(chatRooms.Values);
            sendBroadcastData(str);
        }
        public void sendUsernameNotAvailable(ChatUser user)
        {
            var str = "nameTaken";
            sendData(user.socket, str);
        }

        #endregion

    }
}
