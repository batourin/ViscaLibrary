using System;
using System.Text;

namespace Visca
{
    public abstract class ViscaPacket
    {
        protected readonly byte[] _bytes;

        protected ViscaPacket(byte[] data)
        {
            if(data != null)
            {
                if ((data.Length < 3) || ((data[0] & 0x80) != 0x80) || (data[data.Length - 1] != Visca.Terminator))
#if SSHARP
                    throw new ArgumentOutOfRangeException("data", "Supplied byte array does not appear as Visca packet");
#else
                    throw new ArgumentOutOfRangeException(nameof(data), "Supplied byte array does not appear as Visca packet");
#endif
                _bytes = data;
            }
            else
                _bytes = new byte[32];
        }

        public byte Source
        {
            get
            {
                return (byte)((_bytes[0] & 0x70) >> 4); // 0b0111_0000 bits are source address
            }
        }
        public byte Destination
        {
            get
            {
                return (byte)(_bytes[0] & 0x07); // 0b0000_0111 bits are destination address
            }
        }
        public bool Broadcast
        {
            get
            {
                return ((_bytes[0] & 0x08) >> 3) == 1; // 0b0000_1000 bit is broadcast flag
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\tSource: {0}\r\n", Source);
            sb.AppendFormat("\tDestination: {0}\r\n", Destination);
            sb.AppendFormat("\tBroadcast: {0}", Broadcast);

            return sb.ToString();
        }
    }
}