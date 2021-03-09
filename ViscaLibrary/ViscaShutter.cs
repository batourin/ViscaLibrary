using System;

namespace Visca
{

    public class ViscaShutter : ViscaModeCommand<UpDownMode>
    {
        public ViscaShutter(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Shutter }, "Shutter", mode)
        { }
    }

    public class ViscaShutterValue : ViscaPositionCommand
    {
        public ViscaShutterValue(byte address, int position)
        : this(address, position, ViscaDefaults.ShutterLimits)
        { }

        public ViscaShutterValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ShutterValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Shutter.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaShutterInquiry : ViscaPositionInquiry
    {
        public ViscaShutterInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ShutterValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Shutter.Inquiry", this.Destination);
        }
    }

    public class ViscaShutterSlowMode : ViscaModeCommand<AutoManualMode>
    {
        public ViscaShutterSlowMode(byte address, AutoManualMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.ShutterSlow }, "ShutterSlow", mode)
        { }
    }
}
