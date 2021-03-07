using System;
using System.Collections.ObjectModel;

namespace Visca
{
    public class WB : EnumBaseType<WB>
    {
        public static readonly WB Auto = new WB(Visca.Commands.WBCommands.Auto, "Auto");
        public static readonly WB Indoor = new WB(Visca.Commands.WBCommands.Indoor, "Indoor");
        public static readonly WB Outdoor = new WB(Visca.Commands.WBCommands.Outdoor, "Outdoor");
        public static readonly WB OnePush = new WB(Visca.Commands.WBCommands.OnePush, "OnePush");
        public static readonly WB ATW = new WB(Visca.Commands.WBCommands.ATW, "ATW");
        public static readonly WB Manual = new WB(Visca.Commands.WBCommands.Manual, "Manual");

        public WB(int key, string value) : base(key, value)
        {
        }

        public static ReadOnlyCollection<WB> GetValues()
        {
            return GetBaseValues();
        }

        public static WB GetByKey(int key)
        {
            return GetBaseByKey(key);
        }
    }

    public class ViscaWBAuto : ViscaCommand
    {
        public ViscaWBAuto(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.Auto
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.Auto", this.Destination);
        }
    }

    public class ViscaWBIndoor : ViscaCommand
    {
        public ViscaWBIndoor(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.Indoor
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.Indoor", this.Destination);
        }
    }

    public class ViscaWBOutdoor : ViscaCommand
    {
        public ViscaWBOutdoor(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.Outdoor
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.Outdoor", this.Destination);
        }
    }

    public class ViscaWBOnePush : ViscaCommand
    {
        public ViscaWBOnePush(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.OnePush
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.OnePush", this.Destination);
        }
    }

    public class ViscaWBAtw : ViscaCommand
    {
        public ViscaWBAtw(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.ATW
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.ATW", this.Destination);
        }
    }

    public class ViscaWBManual : ViscaCommand
    {
        public ViscaWBManual(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.WB,
                Visca.Commands.WBCommands.Manual
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} WB.Manual", this.Destination);
        }
    }

    public class ViscaWBInquiry: ViscaValueInquiry
    {
        public ViscaWBInquiry(byte address, Action<WB> action)
            :base(address, new Action<byte>(b => { WB wb = WB.GetByKey(b); action(wb); }))
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
