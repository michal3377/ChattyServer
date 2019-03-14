using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{

    /// <summary>
    /// Tworzy tekstowe polecenia, które są później parsowane i interpretowane po stronie klienta.
    /// </summary>
    public class ServerMessageGenerator
    {
        public static readonly char MESSAGE_END = '~';
        public static readonly char STRING_SEPARATOR = '|';
        public static readonly char ROOM_SEPARATOR = '}';
        public static readonly char USER_SEPARATOR = '{';

        private string closeMsg(string msg)
        {
            return msg += MESSAGE_END;
        }

        public string generateUserListMsg(Dictionary<string, ChatUser>.ValueCollection users)
        {
            string output = "userList" + STRING_SEPARATOR;
            foreach (var item in users)
            {
                output += item.name + STRING_SEPARATOR;
            }
            output = output.Remove(output.Length - 1);

            return closeMsg(output);
        }
        public string generateRoomListMsg(Dictionary<string, ChatRoom>.ValueCollection rooms)
        {
            string output = "roomList" + STRING_SEPARATOR;
            foreach (var item in rooms)
            {
                output += item.name + STRING_SEPARATOR;
            }
            output = output.Remove(output.Length - 1);
            return closeMsg(output);
        }

        public string generateChatTreeMsg(Dictionary<string, ChatRoom>.ValueCollection rooms)
        {
            string output = "chatTree" + STRING_SEPARATOR;
            foreach (var room in rooms)
            {
                output += room.name;
                foreach (var username in room.members)
                {
                    output += USER_SEPARATOR + username;
                }
                output += ROOM_SEPARATOR;
            }

            output = output.Remove(output.Length - 1);
            return closeMsg(output);
        }

        public string generateChatMsg(string msg)
        {
            var output = "msg" + STRING_SEPARATOR + msg;
            return closeMsg(output);
        }

        public string generateServerMsg(string msg)
        {
            var output = "msgServer" + STRING_SEPARATOR + msg;
            return closeMsg(output);
        }
        public string generateChatAuthorMsg(string author, string msg)
        {
            var output = "msgAuthor" + STRING_SEPARATOR + author + STRING_SEPARATOR + msg;
            return closeMsg(output);
        }
    }

    /// <summary>
    /// Parsuje polecenia i zwraca je w odpowiedniej formie.
    /// </summary>
    public class ClientMessageParser
    {
        private char STRING_SEPARATOR = ServerMessageGenerator.STRING_SEPARATOR;
        private char MESSAGE_END = ServerMessageGenerator.MESSAGE_END;

        //todo mechanizm
        public string[] parseCommands(string msg)
        {
            msg.Remove(msg.Length - 1);
            return msg.Split(MESSAGE_END);
        }

        public string getMsgType(string msg)
        {
            return msg.Split(STRING_SEPARATOR)[0];
        }

        public string parseLoginMsg(string msg)
        {
            return msg.Split(STRING_SEPARATOR)[1];
        }
        public string parseJoinMsg(string msg)
        {
            return msg.Split(STRING_SEPARATOR)[1];
        }
        public string parseRoomMsg(string msg)
        {
            return msg.Split(STRING_SEPARATOR)[1];
        }

        public string parseCreateRoomMsg(string msg)
        {
            return msg.Split(STRING_SEPARATOR)[1];
        }
    }
}
