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

    public abstract class ViscaPositionCommand: ViscaCommand
    {

        private readonly ViscaVariable _positionByte1;
        private readonly ViscaVariable _positionByte2;
        private readonly ViscaVariable _positionByte3;
        private readonly ViscaVariable _positionByte4;

        public ViscaPositionCommand(byte address, int position)
            : base(address)
        {
            _positionByte1 = new ViscaVariable("PositionByte1", 0);
            _positionByte2 = new ViscaVariable("PositionByte2", 0);
            _positionByte3 = new ViscaVariable("PositionByte3", 0);
            _positionByte4 = new ViscaVariable("PositionByte4", 0);

            Position = position;
        }

        protected void AppendPosition()
        {
            Append(_positionByte1);
            Append(_positionByte2);
            Append(_positionByte3);
            Append(_positionByte4);
        }

        public int Position
        {
            get
            {
                return (_positionByte1.Value << 12)
                    + (_positionByte2.Value << 8)
                    + (_positionByte3.Value << 4)
                    + _positionByte4.Value;
            }
            set
            {
                _positionByte1.Value = (byte)((value & 0xF000) >> 12);
                _positionByte2.Value = (byte)((value & 0x0F00) >> 8);
                _positionByte3.Value = (byte)((value & 0x00F0) >> 4);
                _positionByte4.Value = (byte)((value & 0x000F));
            }
        }

        public ViscaPositionCommand SetPosition(int position)
        {
            Position = position;
            return this;
        }
    }

}