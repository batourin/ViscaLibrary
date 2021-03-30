using System;

namespace Visca
{
    public class ViscaPower : ViscaModeCommand<OnOffMode>
    {
        public ViscaPower(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Power }, "Power", mode)
        { }
    }

    public class ViscaPowerInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaPowerInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Power }, "Power", action)
        { }
    }
}
