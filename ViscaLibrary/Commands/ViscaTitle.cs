using System;

namespace Visca
{
    public class ViscaTitle : ViscaModeCommand<OnOffMode>
    {
        public ViscaTitle(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Title }, "Title", mode)
        { }
    }

    public class ViscaTitleInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaTitleInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.Title }, "Title", action)
        { }
    }
}
