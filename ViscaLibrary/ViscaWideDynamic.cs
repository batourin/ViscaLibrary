using System;

namespace Visca
{
    public class ViscaWideDynamicMode : ViscaModeCommand<OnOffMode>
    {
        public ViscaWideDynamicMode(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.WideDynamic }, "WideDynamic", mode)
        { }

    }

    public class ViscaWideDynamicInquiry : ViscaOnOffInquiry
    {
        public ViscaWideDynamicInquiry(byte address, Action<bool> action)
            : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WideDynamic
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WideDynamic.Inquiry", this.Destination);
        }
    }
}
