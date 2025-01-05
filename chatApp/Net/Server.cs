
using chatClient.Net.IO;
using System.Net.Sockets;


namespace chatClient.Net
{
    public class Server
    {
        TcpClient client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action userDisconnectEvent;
        public event Action newMessageEvent;
        public Server()
        {
            client = new TcpClient();  
        }
        public void ConnectToServer(string Username)
        {
            if (client.Connected)
            {
                return;
            }
            client.Connect("127.0.0.1", 666);
            PacketReader = new PacketReader(client.GetStream());

            if (!string.IsNullOrEmpty(Username)) 
            {
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(Username);
                client.Client.Send(connectPacket.GetPacketBytes());
            }
            Task.Run(() =>
            {
                ReadPackets();
            });
        }

        private void ReadPackets()
        {
            while (true)
            {
                var opcode = PacketReader.ReadByte();
                switch (opcode)
                {
                    case 1:
                        connectedEvent?.Invoke();
                        break;
                    case 5:
                        newMessageEvent?.Invoke(); 
                        break;
                    case 10:
                        userDisconnectEvent?.Invoke();
                        break;
                    default:
                        Console.WriteLine($"not covered opcode{opcode}");
                        break;
                }
            }
            
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(message);
            client.Client.Send(messagePacket.GetPacketBytes()); 
        }
    }
}
