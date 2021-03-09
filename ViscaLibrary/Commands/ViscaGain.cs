using System;

namespace Visca
{

    public class ViscaGain : ViscaModeCommand<UpDownMode>
    {
        public ViscaGain(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Gain }, "Gain", mode)
        { }
    }

    public class ViscaGainValue : ViscaPositionCommand
    {
        public ViscaGainValue(byte address, int position)
        : this(address, position, ViscaDefaults.GainLimits)
        { }

        public ViscaGainValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.GainValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Gain.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaGainInquiry : ViscaPositionInquiry
    {
        public ViscaGainInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.GainValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Gain.Inquiry", this.Destination);
        }
    }
}
