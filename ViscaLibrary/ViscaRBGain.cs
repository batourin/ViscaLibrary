using System;

namespace Visca
{

    public class ViscaRGain: ViscaModeCommand<UpDownMode>
    {
        public ViscaRGain(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.RGain }, "RGain", mode)
        { }
    }

    public class ViscaRGainValue : ViscaPositionCommand
    {
        public ViscaRGainValue(byte address, int position)
        : this(address, position, ViscaDefaults.RGainLimits)
        { }

        public ViscaRGainValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.RGainValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} RGain.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaRGainInquiry : ViscaPositionInquiry
    {
        public ViscaRGainInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.RGainValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} RGain.Inquiry", this.Destination);
        }
    }

    public class ViscaBGain : ViscaModeCommand<UpDownMode>
    {
        public ViscaBGain(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.BGain }, "BGain", mode)
        { }
    }

    public class ViscaBGainValue : ViscaPositionCommand
    {
        public ViscaBGainValue(byte address, int position)
        : this(address, position, ViscaDefaults.BGainLimits)
        { }

        public ViscaBGainValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.BGainValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} BGain.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaBGainInquiry : ViscaPositionInquiry
    {
        public ViscaBGainInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.BGainValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} BGain.Inquiry", this.Destination);
        }
    }
}
