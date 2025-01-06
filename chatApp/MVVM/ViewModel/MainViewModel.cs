using chatClient.MVVM.Core;
using chatClient.MVVM.Model;
using chatClient.Net;
using System.Collections.ObjectModel;
using System.Windows;

namespace chatClient.MVVM.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<UserModel> users { get; set; }
        public ObservableCollection<string> messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        private Server server;
        public string Username { get; set; }
        public string Message { get; set; }
        public string IP  { get; set; }
        public MainViewModel()
        {
            users = new ObservableCollection<UserModel>();
            messages = new ObservableCollection<string>();
            server = new Server();
            server.connectedEvent += userConnected;
            server.newMessageEvent += messageRecieved;
            server.userDisconnectEvent += userDisconnected;
            ConnectToServerCommand = new RelayCommand(o=>server.ConnectToServer(Username, IP), o=> !string.IsNullOrEmpty(Username)&& !string.IsNullOrEmpty(IP));
            SendMessageCommand = new RelayCommand(o=>server.SendMessageToServer(Message), o=> !string.IsNullOrEmpty(Message));
        }

        private void userDisconnected()
        {
            var msg = server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => { 
                var DcUser = users.Where(user => user.UID == msg).FirstOrDefault(); 
                if (DcUser == null)
                {
                    throw new Exception($"Did not find user UUID: {msg} ");
                }
                users.Remove(DcUser);
            });
        }

        private void messageRecieved()
        {
            var msg = server.PacketReader.ReadMessage();

            Application.Current.Dispatcher.Invoke(() => messages.Add(msg));
        }

        private void userConnected()
        {
            var user = new UserModel
            {
                Username = server.PacketReader.ReadMessage(),
                UID = server.PacketReader.ReadMessage()
            };
            if (!users.Any(x=>x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() =>  users.Add(user) );
            }
        }
    }
}
