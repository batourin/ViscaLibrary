using System;

namespace Visca
{

    public class ViscaAperture : ViscaModeCommand<UpDownMode>
    {
        public ViscaAperture(byte address, UpDownMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Aperture }, "Aperture", mode)
        { }
    }

    public class ViscaApertureValue : ViscaPositionCommand
    {
        public ViscaApertureValue(byte address, int position)
        : this(address, position, ViscaDefaults.ApertureLimits)
        { }

        public ViscaApertureValue(byte address, int position, IViscaRangeLimits<int> limits)
        : base(address, position, limits)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ApertureValue,
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Aperture.Value 0x{1:X2} ({1})", this.Destination, Position);
        }
    }

    public class ViscaApertureInquiry : ViscaPositionInquiry
    {
        public ViscaApertureInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ApertureValue
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Aperture.Inquiry", this.Destination);
        }
    }
}
