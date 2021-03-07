using System;

namespace Visca
{
    public abstract class ViscaInquiry: ViscaTxPacket
    {
        public ViscaInquiry(byte address)
        : base(address, false)
        {
            Append(Visca.Inquiry);
        }

        public abstract void Process(ViscaRxPacket viscaRxPacket);
    }

    public abstract class ViscaOnOffInquiry: ViscaInquiry
    {
        private readonly Action<bool> _action;
        public ViscaOnOffInquiry(byte address, Action<bool> action)
        : base(address)
        {
            _action = action;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_action != null)
            {
                if (viscaRxPacket.PayLoad[0] == 0x02)
                    _action(true);
                else if (viscaRxPacket.PayLoad[0] == 0x03)
                    _action(false);
            }
        }
    }

    public abstract class ViscaPositionInquiry : ViscaInquiry
    {
        private readonly Action<int> _action;
        public ViscaPositionInquiry(byte address, Action<int> action)
        : base(address)
        {
            _action = action;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_action != null)
            {
                if (viscaRxPacket.PayLoad.Length == 4)
                {
                    _action( (viscaRxPacket.PayLoad[0] << 12) +
                             (viscaRxPacket.PayLoad[1] << 8) +
                             (viscaRxPacket.PayLoad[2] << 4) +
                              viscaRxPacket.PayLoad[3]
                     );
                }
                else
                    throw new ArgumentOutOfRangeException("viscaRxPacket", "Recieved packet is not Position Inquiry");
            }
        }
    }
    public abstract class Visca2DPositionInquiry : ViscaInquiry
    {
        private readonly Action<int, int> _action;
        public Visca2DPositionInquiry(byte address, Action<int, int> action)
        : base(address)
        {
            _action = action;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_action != null)
            {
                if (viscaRxPacket.PayLoad.Length == 8)
                {
                    _action(
                            (viscaRxPacket.PayLoad[0] << 12) +
                            (viscaRxPacket.PayLoad[1] << 8) +
                            (viscaRxPacket.PayLoad[2] << 4) +
                             viscaRxPacket.PayLoad[3],
                            (viscaRxPacket.PayLoad[4] << 12) +
                            (viscaRxPacket.PayLoad[5] << 8) +
                            (viscaRxPacket.PayLoad[6] << 4) +
                             viscaRxPacket.PayLoad[7]
                     );
                }
                else
                    throw new ArgumentOutOfRangeException("viscaRxPacket", "Recieved packet is not 2D Position Inquiry");
            }
        }
    }
}