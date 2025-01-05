using chatServer;
using chatServer.Net.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static List<Client> users;
        static TcpListener listener;
        static void Main(string[] args)
        {
            users = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 666);
            listener.Start();

            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                users.Add(client);

                // broadcast connection to other users
                BroadcastConnection();
            }
        }
        static void BroadcastConnection()
        {
            foreach (var client in users)
            {
                foreach (var other in users)
                {
                    var broadCastPacket = new PacketBuilder();
                    broadCastPacket.WriteOpCode(1);
                    broadCastPacket.WriteString(other.Name);
                    broadCastPacket.WriteString(other.UID.ToString());
                    client.ClientSocket.Client.Send(broadCastPacket.GetPacketBytes());
                }
            }
        }
        public static void BroadcastMessage(string msg)
        {
            foreach (var client in users)
            {
               
                var broadcastMessage = new PacketBuilder();
                broadcastMessage.WriteOpCode(5);
                broadcastMessage.WriteString(msg);
                client.ClientSocket.Client.Send(broadcastMessage.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string UID)
        {
            Client DcUser = users.Where(client => client.UID.ToString() == UID).FirstOrDefault();
            users.Remove(DcUser);
            foreach (var client in users)
            {

                var broadcastDisconnect = new PacketBuilder();
                broadcastDisconnect.WriteOpCode(10);
                broadcastDisconnect.WriteString(UID);
                client.ClientSocket.Client.Send(broadcastDisconnect.GetPacketBytes());
            }
            BroadcastMessage($"{DcUser.Name} Disconnected");
        }
    }
}