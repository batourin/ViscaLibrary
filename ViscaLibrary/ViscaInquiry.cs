using System;

namespace Visca
{
    internal abstract class ViscaInquiry: ViscaTxPacket
    {
        public ViscaInquiry(byte address)
        : base(address, false)
        {
            Append(Visca.Inquiry);
        }

        public abstract void Process(ViscaRxPacket viscaRxPacket);
    }

    internal class ViscaPowerInquiry: ViscaInquiry
    {
        private readonly Action<bool> _action;
        public ViscaPowerInquiry(byte address, Action<bool> action)
        : base(address)
        {
            _action = action;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Power
            });
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if(_action != null)
            {
                if (viscaRxPacket.PayLoad[0] == 0x02)
                    _action(true);
                else if (viscaRxPacket.PayLoad[0] == 0x03)
                    _action(false);
            }
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PowerInquiry", this.Destination) ;
        }
    }
}