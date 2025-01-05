
using System.Text;

namespace chatServer.Net.IO
{
    public class PacketBuilder
    {
        public MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            ms.WriteByte(opcode);
        }
        public void WriteString(string str)
        {
            int ln = str.Length;
            ms.Write(BitConverter.GetBytes(ln));
            ms.Write(Encoding.ASCII.GetBytes(str));
        }
        public byte[] GetPacketBytes()
        {
            return ms.ToArray();
        }
    }
}
