using System;

namespace Visca
{
    public class ViscaMute : ViscaModeCommand<OnOffMode>
    {
        public ViscaMute(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Mute }, "Mute", mode)
        { }

    }

    public class ViscaMuteInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaMuteInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Mute }, "Mute", action)
        { }
    }
}
