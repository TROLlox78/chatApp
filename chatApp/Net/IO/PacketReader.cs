using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace chatClient.Net.IO
{
    public class PacketReader : BinaryReader
    {

        private NetworkStream ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            this.ns = ns;
        }
        public string ReadMessage()
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            ns.Read(msgBuffer, 0, length);
            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
