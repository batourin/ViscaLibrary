using System;

namespace Visca
{
    public class ViscaCommand: ViscaTxPacket
    {
        public ViscaCommand(byte address)
        : base(address, false)
        {
            Append(Visca.Command);
        }
    }
}