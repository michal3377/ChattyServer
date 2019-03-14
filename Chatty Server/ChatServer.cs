using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{
    /// <summary>
    /// Klasa serwera, odpowiadająca za przyjmowanie połączeń oraz ich obsługę, oparta na implementacji klasy Net.Sockets.
    /// Klasa na zewnątrz wysyła tylko callbacki trzech typów (user podłączony, rozłączony, odebrano dane),
    /// dzięki czemu osiągnięto elastyczność i abstrakcję, gdyż niskopoziomowa obsługa sieci została oddzielona od wysokopoziomowej obsługi danych.
    /// </summary>
    class ChatServer
    {

        private UIAgent ui;
        private byte[] buffer = new byte[1024];     // standardowo, bufor 1KB
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private ServerUserCallback userConnectedCallback;
        private ServerUserCallback userDisconnectedCallback;
        private ServerDataCallback dataReceivedCallback;

        public ChatServer(UIAgent agent, ServerUserCallback userConnectedCallback,
            ServerUserCallback userDisconnectedCallback, ServerDataCallback dataReceivedCallback)
        {
            this.userConnectedCallback = userConnectedCallback;
            this.userDisconnectedCallback = userDisconnectedCallback;
            this.dataReceivedCallback = dataReceivedCallback;
            ui = agent;
        }

        /// <summary>
        /// Metoda inicjalizująca pracę serwera
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void initServer(string ip, int port)
        {

            try
            {
                ui.log("Uruchamianie serwera...");
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                serverSocket.Listen(1);
                serverSocket.BeginAccept(new AsyncCallback(onUserConnected), null);
                ui.log("Serwer został uruchomiony!");
            }
            catch (FormatException)
            {
                ui.log("Podano nieprawidłową kombinację IP:Port");
            }
            catch (SocketException)
            {
                ui.log("Nie można uruchomić serwera na tym adresie");
            }
            catch (Exception)
            {
                ui.log("Błąd startu serwera");                
            }
            
        }
        private void onUserConnected(IAsyncResult ar)
        {
            Socket socket = serverSocket.EndAccept(ar);
            userConnectedCallback(socket);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(onDataReceived), socket);
            serverSocket.BeginAccept(new AsyncCallback(onUserConnected), null);
        }

        private void onDataReceived(IAsyncResult ar)
        {

            Socket socket = (Socket)ar.AsyncState;
            if (socket.Connected)
            {
                int received;
                try
                {
                    received = socket.EndReceive(ar);
                }
                catch (Exception)
                {
                    userDisconnectedCallback(socket);
                    return;
                }
                if (received != 0)
                {
                    byte[] dataBuf = new byte[received];
                    Array.Copy(buffer, dataBuf, received);
                    string text = Encoding.UTF8.GetString(dataBuf);

                    dataReceivedCallback(text, socket);
                }
                else
                {
                    userDisconnectedCallback(socket);
                }
            }
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(onDataReceived), socket);
            }
            catch (Exception)
            {
                userDisconnectedCallback(socket);
            }
        }

    }
}
