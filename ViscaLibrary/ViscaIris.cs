using System;

namespace Visca
{

    public class ViscaIris : ViscaModeCommand<UpDownMode>
    {
        public ViscaIris(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Iris }, "Iris", mode)
        { }
    }

    public class ViscaIrisValue : ViscaPositionCommand
    {
        public ViscaIrisValue(byte address, int position)
        : this(address, position, ViscaDefaults.IrisLimits)
        { }

        public ViscaIrisValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.IrisValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Iris.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaIrisInquiry : ViscaPositionInquiry
    {
        public ViscaIrisInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.IrisValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Iris.Inquiry", this.Destination);
        }
    }
}
