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

        /// <summary>
        /// Action to be excuted when command fail
        /// </summary>
        public Action<ViscaError> ErrorAction;

        /// <summary>
        /// Construcot for broadcast command
        /// </summary>
        public ViscaTxPacket()
            : base(null)
        {
            _bytes[0] = (byte)(0x80 + (0x01 << 3));
        }

        /// <summary>
        /// Constructor for adddressed non-broadcast commands
        /// </summary>
        /// <param name="address">camera id - 0-7</param>
        public ViscaTxPacket(byte address)
            : this(address, null)
        { }

        /// <summary>
        /// Constructor for addressed command and setting error action to execute if error occurs
        /// </summary>
        /// <param name="address">camera id - 0 - 7</param>
        /// <param name="errorAction">ViscaProtocolProcessor will execute this action with ViscaError parameter if error recieved</param>
        public ViscaTxPacket(byte address, Action<ViscaError> errorAction)
            : base(null)
        {
            _bytes[0] = (byte)(0x80 + address);
            ErrorAction = errorAction;
        }

        public ViscaTxPacket OnError(Action<ViscaError> errorAction) { ErrorAction = errorAction; return this; }

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