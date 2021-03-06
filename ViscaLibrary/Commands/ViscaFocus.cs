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
            : this(_defaultSpeed, ViscaDefaults.FocusSpeedLimits)
         {}
        public ViscaFocusSpeed(byte value)
            : this(value, ViscaDefaults.FocusSpeedLimits)
         {}

        public ViscaFocusSpeed(IViscaRangeLimits<byte> limits)
            : this( _defaultSpeed, limits)
         {}

        public ViscaFocusSpeed(byte value, IViscaRangeLimits<byte> limits)
            : base("FocusSpeed", value, limits)
        { }
    }

    public abstract class ViscaFocusSpeedCommand: ViscaDynamicCommand
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

    public class ViscaFocusNearLimit : ViscaPositionCommand
    {

        public ViscaFocusNearLimit(byte address, int focusPosition)
            : base(address, focusPosition)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusNearLimit
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.NearLimit:{1}", this.Destination, Position);
        }
    }

    public class ViscaFocusAuto : ViscaModeCommand<OnOffMode>
    {
        public ViscaFocusAuto(byte address, OnOffMode mode)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.FocusAuto }, "Focus.Auto", mode)
        { }
    }

    public class ViscaFocusAutoToggle : ViscaCommand
    {
        public ViscaFocusAutoToggle(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusAuto,
                Visca.Commands.FocusAutoMode.Toggle
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Auto.Toggle", this.Destination);
        }
    }

    public class ViscaFocusAutoInquiry : ViscaModeInquiry<OnOffMode>
    {
        public ViscaFocusAutoInquiry(byte address, Action<OnOffMode> action)
            : base(address, new byte[] { Visca.Category.Camera1, Visca.Commands.FocusAuto }, "Focus.Auto", action)
        { }
    }

    public class ViscaFocusPosition : ViscaPositionCommand
    {

        public ViscaFocusPosition(byte address, int focusPosition)
            : base(address, focusPosition)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusPosition
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Position:{1}", this.Destination, Position);
        }
    }

    public class ViscaFocusPositionInquiry : ViscaPositionInquiry
    {
        public ViscaFocusPositionInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.FocusPosition
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Focus.Position.Inquiry", this.Destination);
        }
    }

}