using System;

namespace Visca
{
    public class ViscaBackLightMode : ViscaModeCommand<OnOffMode>
    {
        public ViscaBackLightMode(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.BackLight }, "BackLight", mode)
        { }

    }

    public class ViscaBackLightInquiry : ViscaOnOffInquiry
    {
        public ViscaBackLightInquiry(byte address, Action<bool> action)
            : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.BackLight
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} BackLight.Inquiry", this.Destination);
        }
    }
}
