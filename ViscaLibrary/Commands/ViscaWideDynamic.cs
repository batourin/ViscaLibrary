using System;

namespace Visca
{
    public class ViscaWideDynamicMode : ViscaModeCommand<OnOffMode>
    {
        public ViscaWideDynamicMode(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.WideDynamic }, "WideDynamic", mode)
        { }

    }

    public class ViscaWideDynamicInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaWideDynamicInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.WideDynamic }, "WideDynamic", action)
        { }
    }
}
