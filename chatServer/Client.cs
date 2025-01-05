
using chatServer.Net.IO;
using ChatServer;
using System.Net.Sockets;

namespace chatServer
{
    public class Client
    {
        public string Name { get; set; }    
        public Guid UID { get; set; }
        public TcpClient  ClientSocket { get; set; }

        private PacketReader packetReader;
        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            packetReader = new PacketReader(ClientSocket.GetStream());
            
            var opcode = packetReader.ReadByte();
            Name = packetReader.ReadMessage();

            Console.WriteLine($"{DateTime.Now} {Name} has connected");

            Task.Run(()=> Process() );
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = packetReader.ReadMessage();
                            var msgTosend = $"{DateTime.Now} {Name}: {msg}";
                            Console.WriteLine(msgTosend);
                            Program.BroadcastMessage( msgTosend);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{UID} {Name} Disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
