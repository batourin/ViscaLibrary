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

    public class ViscaPower: ViscaCommand
    {
        private readonly bool _power;
        public ViscaPower(byte address, bool power)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Power,
                power ? Visca.Commands.PowerCommands.On : Visca.Commands.PowerCommands.Off 
            });
            _power = power;
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Power{1}", this.Destination, _power ? "ON" : "OFF") ;
        }
    }
}