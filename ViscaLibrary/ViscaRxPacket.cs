using System;
using System.Text;

namespace Visca
{

    public class ViscaRxPacket: ViscaPacket
    {

        public ViscaRxPacket(byte[] data)
            : base(data)
        {
            if(data == null)
                throw new ArgumentNullException("data", "ViscaRxPacket data can not be null");
            PayLoad = new PayLoadIndexer(_bytes.Length - 2, getPayload);
        }

        public bool IsAck
        {
            get
            {
                return (!IsCompletionCommand && ((_bytes.Length == 3) && ((_bytes[1] & 0x40) == 0x40 ))) ? true : false;
            }
        }

        public bool IsCompletionCommand
        {
            get
            {
                return ((_bytes.Length == 3) && ((_bytes[1] & 0x50) == 0x50 )) ? true : false;
            }
        }

        public bool IsCompletionInquiry
        {
            get
            {
                return ((_bytes.Length > 3) && ((_bytes[1] & 0x50) == 0x50 )) ? true : false;
            }
        }

        public byte Socket
        {
            get { return (byte)(_bytes[1] & 0x01); }
        }

        public bool IsError
        {
            get { return ((_bytes[1] & 0x60) == 0x60) ? true : false; }
        }

        public ViscaError Error
        {
            get
            {
                if(IsError)
                {
                    return (ViscaError)_bytes[2];
                }
                else
                    return ViscaError.Ok;
            }
        }

#region Payload property
        public class PayLoadIndexer
        {
            private readonly Func<int, byte> _getPayLoadAction;
            private readonly int _length;
            public PayLoadIndexer(int length, Func<int, byte> getPayLoadAction)
            {
                _length = length;
                _getPayLoadAction = getPayLoadAction;
            }
            public byte this[int i]
            {
                get { return _getPayLoadAction(i); }
            }
        }

        private byte getPayload(int i)
        {
            return _bytes[i+2];
        }

        public readonly PayLoadIndexer PayLoad;

#endregion Payload property

        public override string ToString()
        {
            if(IsAck)
                return String.Format("Camera{0} on socket {1} ACK", Source, Socket);
            else if(IsCompletionCommand)
                return String.Format("Camera{0} on socket {1} Completion", Source, Socket);
            else if(IsCompletionInquiry)
                return String.Format("Camera{0} on socket {1} Inquiry Complition", Source, Socket);
            else if(IsError)
                return String.Format("Camera{0} Error: {0}", Source, Error.ToString());

            return String.Format("Camera{0} Unknown packet", Source);
        }
    }
}