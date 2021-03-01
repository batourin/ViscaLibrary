using System;

namespace Visca
{
    public class ViscaFocusStop: ViscaCommand
    {
        public ViscaFocusStop(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Focus,
                Visca.Commands.FocusCommands.Stop
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Stop", this.Destination) ;
        }
    }

    public class ViscaFocusFar: ViscaCommand
    {
        public ViscaFocusFar(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Focus,
                Visca.Commands.FocusCommands.Far
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Far", this.Destination) ;
        }
    }

    public class ViscaFocusNear: ViscaCommand
    {
        public ViscaFocusNear(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Focus,
                Visca.Commands.FocusCommands.Near
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Near", this.Destination) ;
        }
    }

    public class ViscaFocusSpeed: ViscaVariableWithLimits
    {

        private const byte _defaultSpeed = 0x04;

        public ViscaFocusSpeed()
            : this(_defaultSpeed, new ViscaDefaultFocusSpeedLimits())
            {}
        public ViscaFocusSpeed(byte value)
            : this(value, new ViscaDefaultFocusSpeedLimits())
            {}

        public ViscaFocusSpeed(ViscaRangeLimits<byte> limits)
            : this( _defaultSpeed, limits)
            {}

        public ViscaFocusSpeed(byte value, ViscaRangeLimits<byte> limits)
            : base("FocusSpeed", value, limits)
        {
        }
    }

    public abstract class ViscaFocusSpeedCommand: ViscaCommand
    {
        public ViscaFocusSpeed _focusSpeed { get; private set; }

        public ViscaFocusSpeedCommand(byte address, byte focusOperation)
            : this(address, focusOperation, new ViscaFocusSpeed())
        {}

        public ViscaFocusSpeedCommand(byte address, byte focusOperation, ViscaFocusSpeed focusSpeed)
            : base(address)
        {
            if (   focusOperation != Visca.Commands.FocusCommands.FarWithSpeed 
                && focusOperation != Visca.Commands.FocusCommands.NearWithSpeed )
                    throw new ArgumentOutOfRangeException
                    (
                        "focusOperation",
                        String.Format("Value should be ether 0x{0:X2} or 0x{1:X2}",
                                        Visca.Commands.FocusCommands.FarWithSpeed,
                                        Visca.Commands.FocusCommands.NearWithSpeed
                        )
                    );

            _focusSpeed = focusSpeed;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Focus
            });
            Append(focusOperation, _focusSpeed, Visca.HighByteMask);
        }

        public byte Speed { get { return _focusSpeed.Value; } }
    }

    public class ViscaFocusPosition: ViscaCommand
    {
 
        private readonly ViscaVariable _focusPositionByte1;
        private readonly ViscaVariable _focusPositionByte2;
        private readonly ViscaVariable _focusPositionByte3;
        private readonly ViscaVariable _focusPositionByte4;

        public ViscaFocusPosition(byte address, int focusPosition)
            : base(address)
        {
            _focusPositionByte1 = new ViscaVariable("FocusPositionByte1", 0);
            _focusPositionByte2 = new ViscaVariable("FocusPositionByte2", 0);
            _focusPositionByte3 = new ViscaVariable("FocusPositionByte3", 0);
            _focusPositionByte4 = new ViscaVariable("FocusPositionByte4", 0);

            FocusPosition = focusPosition;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusPosition
            });
            Append(_focusPositionByte1);
            Append(_focusPositionByte2);
            Append(_focusPositionByte3);
            Append(_focusPositionByte4);
        }

        public int FocusPosition
        {
            get
            {
                return (_focusPositionByte1.Value << 12)
                    +  (_focusPositionByte2.Value << 8)
                    +  (_focusPositionByte3.Value << 4)
                    +   _focusPositionByte4.Value;
            }
            set 
            {
                _focusPositionByte1.Value = (byte)((value & 0xF000) >> 12);
                _focusPositionByte2.Value = (byte)((value & 0x0F00) >> 8);
                _focusPositionByte3.Value = (byte)((value & 0x00F0) >> 4);
                _focusPositionByte4.Value = (byte)((value & 0x000F) );
            }
        }

        public ViscaFocusPosition SetPosition(int focusPosition)
        {
            FocusPosition = focusPosition;

            return this;
        }

        public override string ToString()
        {
            return String.Format("Camera{0} FocusPosition:{1}", this.Destination, FocusPosition);
        }
    }

    public class ViscaFocusTrigger: ViscaCommand
    {
        public ViscaFocusTrigger(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusOnePush,
                Visca.Commands.FocusOnePushCommands.Trigger
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Trigger", this.Destination) ;
        }
    }
    
    public class ViscaFocusInfinity: ViscaCommand
    {
        public ViscaFocusInfinity(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusOnePush,
                Visca.Commands.FocusOnePushCommands.Infinity
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Infinity", this.Destination) ;
        }
    }


}