using System;

namespace Visca
{
    public class ViscaBackLightMode : ViscaModeCommand<OnOffMode>
    {
        public ViscaBackLightMode(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.BackLight }, "BackLight", mode)
        { }

    }

    public class ViscaBackLightInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaBackLightInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.BackLight }, "BackLight", action)
        { }
    }
}
