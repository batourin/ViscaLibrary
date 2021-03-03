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
        public ViscaFocusSpeed FocusSpeed { get; private set; }

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

            this.FocusSpeed = focusSpeed;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Focus
            });
            Append(focusOperation, this.FocusSpeed, Visca.HighByteMask);
        }

        public byte Speed { get { return FocusSpeed.Value; } }
    }

    public class ViscaFocusFarWithSpeed : ViscaFocusSpeedCommand
    {

        public ViscaFocusFarWithSpeed(byte address)
            : this(address, new ViscaFocusSpeed())
        { }

        public ViscaFocusFarWithSpeed(byte address, byte focusSpeed)
            : this(address, new ViscaFocusSpeed(focusSpeed))
        { }

        public ViscaFocusFarWithSpeed(byte address, ViscaFocusSpeed focusSpeed)
            : base(address, Visca.Commands.FocusCommands.FarWithSpeed, focusSpeed)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Far with FocusSpeed:{1}", this.Destination, FocusSpeed.Value);
        }
    }

    public class ViscaFocusNearWithSpeed : ViscaFocusSpeedCommand
    {

        public ViscaFocusNearWithSpeed(byte address)
            : this(address, new ViscaFocusSpeed())
        { }

        public ViscaFocusNearWithSpeed(byte address, byte focusSpeed)
            : this(address, new ViscaFocusSpeed(focusSpeed))
        { }

        public ViscaFocusNearWithSpeed(byte address, ViscaFocusSpeed focusSpeed)
            : base(address, Visca.Commands.FocusCommands.NearWithSpeed, focusSpeed)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Wide with FocusSpeed:{1}", this.Destination, FocusSpeed.Value);
        }
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

    public class ViscaFocusAutoOn : ViscaCommand
    {
        public ViscaFocusAutoOn(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusAuto,
                Visca.Commands.FocusAutoCommands.On
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Auto.On", this.Destination);
        }
    }

    public class ViscaFocusAutoOff : ViscaCommand
    {
        public ViscaFocusAutoOff(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusAuto,
                Visca.Commands.FocusAutoCommands.Off
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Auto.Off", this.Destination);
        }
    }

    public class ViscaFocusAutoToggle : ViscaCommand
    {
        public ViscaFocusAutoToggle(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusAuto,
                Visca.Commands.FocusAutoCommands.Toggle
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Auto.Toggle", this.Destination);
        }
    }

    internal class ViscaFocusAutoInquiry : ViscaInquiry
    {
        private readonly Action<bool> _action;
        public ViscaFocusAutoInquiry(byte address, Action<bool> action)
        : base(address)
        {
            _action = action;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusAuto
            });
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_action != null)
            {
                if (viscaRxPacket.PayLoad[0] == 0x02)
                    _action(true);
                else if (viscaRxPacket.PayLoad[0] == 0x03)
                    _action(false);
            }
        }

        public override string ToString()
        {
            return String.Format("Camera{0} FocusAutoInquiry", this.Destination);
        }
    }

    internal class ViscaFocusPositionInquiry : ViscaInquiry
    {
        private readonly Action<int> _action;
        public ViscaFocusPositionInquiry(byte address, Action<int> action)
        : base(address)
        {
            _action = action;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusPosition
            });
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_action != null)
            {
                if (viscaRxPacket.PayLoad.Length == 4)
                {
                    _action( (viscaRxPacket.PayLoad[0] << 12) +
                             (viscaRxPacket.PayLoad[1] << 8) +
                             (viscaRxPacket.PayLoad[1] << 4) +
                              viscaRxPacket.PayLoad[1]
                     );
                }
                else
                    throw new ArgumentOutOfRangeException("viscaRxPacket", "Recieved packet is not Focus Position Inquiry");
            }
        }

        public override string ToString()
        {
            return String.Format("Camera{0} FocusPositionInquiry", this.Destination);
        }
    }

}