using System;

namespace Visca
{
    public class ViscaBackLight : ViscaModeCommand<OnOffMode>
    {
        public ViscaBackLight(byte address, OnOffMode mode)
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
