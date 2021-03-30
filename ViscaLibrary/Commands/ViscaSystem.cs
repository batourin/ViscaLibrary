using System;

namespace Visca
{
    public class ViscaClear : ViscaCommand
    {
        public ViscaClear()
            : base()
        {
            Append(new byte[] { 0x00, 0x01 });
        }

        public ViscaClear(byte address)
            : base(address)
        {
            Append(new byte[] { 0x00, 0x01});
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Clear", this.Destination);
        }
    }
}
