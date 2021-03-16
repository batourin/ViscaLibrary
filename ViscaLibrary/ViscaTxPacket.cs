using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Visca
{

    public abstract class ViscaTxPacket: ViscaPacket
    {
        private int _hash;
        
        // we start writing at byte 1, the first byte will be filled by the
        // packet constructor function. This function will also append a terminator.
        private byte _length = 1;

        public ViscaTxPacket(byte address, bool broadcast)
            : base(null)
        {
            if (broadcast)
                _bytes[0] = (byte)(0x80 + (0x01 << 3));
            else
                _bytes[0] = (byte)(0x80 + address);
        }

        public bool IsCommand { get { return _bytes[1] == Visca.Command; } }

        public bool IsInquiry { get { return _bytes[1] == Visca.Inquiry; } }

        public byte Length
        {
            get { return _length; } 
            set
            {
                _length = value;
                recalculateHash();
            }
        }

        public void Append(byte data)
        {
            _bytes[Length] = data;
            Length++;
        }

        public void Append(byte[] data)
        {
            Array.Copy(data, 0, _bytes, Length, data.Length);
            Length += (byte) data.Length;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        /// <summary>
        /// To byte[]
        /// </summary>
        /// <returns>The current raw Visca packet as byte array</returns>
        public static implicit operator byte[] (ViscaTxPacket packet)
        {
            byte[] data = new byte[packet.Length+1];
            Array.Copy(packet._bytes, 0, data, 0, packet.Length);
            data[data.Length-1] = 0xFF;
            return data;
        }

        public static bool operator == (ViscaTxPacket a, ViscaTxPacket b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(ViscaTxPacket a, ViscaTxPacket b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return false;
            if (ReferenceEquals(a, null))
                return true;
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            ViscaTxPacket second = obj as ViscaTxPacket;

            return second is object && _hash == second._hash;
        }

        protected void recalculateHash()
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < Length; i++)
                    hash = (hash ^ _bytes[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                _hash = hash;
            }
        }
    }
}