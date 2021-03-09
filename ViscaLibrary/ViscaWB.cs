using System;
using System.Collections.ObjectModel;

namespace Visca
{
    public class WBMode : EnumBaseType<WBMode>
    {
        public static readonly WBMode Auto = new WBMode(Visca.Commands.WBMode.Auto, "Auto");
        public static readonly WBMode Indoor = new WBMode(Visca.Commands.WBMode.Indoor, "Indoor");
        public static readonly WBMode Outdoor = new WBMode(Visca.Commands.WBMode.Outdoor, "Outdoor");
        public static readonly WBMode OnePush = new WBMode(Visca.Commands.WBMode.OnePush, "OnePush");
        public static readonly WBMode ATW = new WBMode(Visca.Commands.WBMode.ATW, "ATW");
        public static readonly WBMode Manual = new WBMode(Visca.Commands.WBMode.Manual, "Manual");

        public WBMode(byte key, string value) : base(key, value)
        { }
    }

    public class ViscaWBMode : ViscaModeCommand<WBMode>
    {
        public ViscaWBMode(byte address, WBMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.WB }, "WB", mode)
        { }

    }

    public class ViscaWBInquiry: ViscaValueInquiry
    {
        public ViscaWBInquiry(byte address, Action<WBMode> action)
            :base(address, new Action<byte>(b => { WBMode wbMode = WBMode.GetByKey(b); action(wbMode); }))
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.Inquiry", this.Destination);
        }
    }
}
