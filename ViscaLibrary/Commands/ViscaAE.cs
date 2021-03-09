using System;

namespace Visca
{
    public partial class AEMode : EnumBaseType<AEMode>
    {
        public static readonly AEMode FullAuto = new AEMode(Visca.Commands.AEMode.FullAuto, "FullAuto");
        public static readonly AEMode Manual = new AEMode(Visca.Commands.AEMode.Manual, "Manual");
        public static readonly AEMode ShutterPriority = new AEMode(Visca.Commands.AEMode.ShutterPriority, "ShutterPriority");
        public static readonly AEMode IrisPriority = new AEMode(Visca.Commands.AEMode.IrisPriority, "IrisPriority");
        public static readonly AEMode GainPriority = new AEMode(Visca.Commands.AEMode.GainPriority, "GainPriority");
        public static readonly AEMode Bright = new AEMode(Visca.Commands.AEMode.Bright, "Bright");
        public static readonly AEMode ShutterAuto = new AEMode(Visca.Commands.AEMode.ShutterAuto, "ShutterAuto");
        public static readonly AEMode IrisAuto = new AEMode(Visca.Commands.AEMode.IrisAuto, "IrisAuto");
        public static readonly AEMode GainAuto = new AEMode(Visca.Commands.AEMode.GainAuto, "GainAuto");

        public AEMode(byte key, string value) : base(key, value)
        { }
    }

    public class ViscaAEMode : ViscaModeCommand<AEMode>
    {
        public ViscaAEMode(byte address, AEMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.AE }, "AE", mode)
        { }

    }

    public class ViscaAEInquiry : ViscaValueInquiry
    {
        public ViscaAEInquiry(byte address, Action<AEMode> action)
            : base(address, new Action<byte>(b => { AEMode aeMode = AEMode.GetByKey(b); action(aeMode); }))
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.AE
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} AE.Inquiry", this.Destination);
        }
    }
}
