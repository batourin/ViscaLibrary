using System;

namespace Visca
{

    public class ViscaExpComp : ViscaModeCommand<UpDownMode>
    {
        public ViscaExpComp(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.ExpComp }, "ExpComp", mode)
        { }
    }

    public class ViscaExpCompValue : ViscaPositionCommand
    {
        public ViscaExpCompValue(byte address, int position)
        : this(address, position, ViscaDefaults.ExpCompLimits)
        { }

        public ViscaExpCompValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ExpCompValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} ExpComp.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaExpCompInquiry : ViscaPositionInquiry
    {
        public ViscaExpCompInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ExpCompValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} ExpComp.Inquiry", this.Destination);
        }
    }
}
