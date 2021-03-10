using System;

namespace Visca
{
    public class ViscaZoomStop: ViscaCommand
    {
        public ViscaZoomStop(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Zoom,
                Visca.Commands.ZoomCommands.Stop
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Stop", this.Destination) ;
        }
    }

    public class ViscaZoomTele: ViscaCommand
    {
        public ViscaZoomTele(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Zoom,
                Visca.Commands.ZoomCommands.Tele
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Tele", this.Destination) ;
        }
    }

    public class ViscaZoomWide: ViscaCommand
    {
        public ViscaZoomWide(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Zoom,
                Visca.Commands.ZoomCommands.Wide
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Wide", this.Destination) ;
        }
    }

    public class ViscaZoomSpeed: ViscaVariableWithLimits
    {

        private const byte _defaultSpeed = 0x04;

        public ViscaZoomSpeed()
            : this(_defaultSpeed, ViscaDefaults.ZoomSpeedLimits)
            {}
        public ViscaZoomSpeed(byte value)
            : this(value, ViscaDefaults.ZoomSpeedLimits)
            {}

        public ViscaZoomSpeed(IViscaRangeLimits<byte> limits)
            : this( _defaultSpeed, limits)
            {}

        public ViscaZoomSpeed(byte value, IViscaRangeLimits<byte> limits)
            : base("ZoomSpeed", value, limits)
        {
        }
    }

    public abstract class ViscaZoomSpeedCommand: ViscaCommand
    {
        public ViscaZoomSpeed ZoomSpeed { get; private set; }

        public ViscaZoomSpeedCommand(byte address, byte zoomOperation)
            : this(address, zoomOperation, new ViscaZoomSpeed())
        {}

        public ViscaZoomSpeedCommand(byte address, byte zoomOperation, ViscaZoomSpeed zoomSpeed)
            : base(address)
        {
            if (   zoomOperation != Visca.Commands.ZoomCommands.TeleWithSpeed 
                && zoomOperation != Visca.Commands.ZoomCommands.WideWithSpeed )
                    throw new ArgumentOutOfRangeException
                    (
                        "zoomOperation",
                        String.Format("Value should be ether 0x{0:X2} or 0x{1:X2}",
                                        Visca.Commands.ZoomCommands.TeleWithSpeed,
                                        Visca.Commands.ZoomCommands.WideWithSpeed
                        )
                    );

            this.ZoomSpeed = zoomSpeed;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Zoom
            });
            Append(zoomOperation, ZoomSpeed, Visca.HighByteMask);
        }

        public byte Speed { get { return ZoomSpeed.Value; } }
    }

    public class ViscaZoomTeleWithSpeed: ViscaZoomSpeedCommand
    {
 
        public ViscaZoomTeleWithSpeed(byte address)
            :this(address, new ViscaZoomSpeed())
        {}

        public ViscaZoomTeleWithSpeed(byte address, byte zoomSpeed)
            :this(address, new ViscaZoomSpeed(zoomSpeed))
        {}

        public ViscaZoomTeleWithSpeed(byte address, ViscaZoomSpeed zoomSpeed)
            : base(address, Visca.Commands.ZoomCommands.TeleWithSpeed, zoomSpeed)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Tele with ZoomSpeed:{1}", this.Destination, ZoomSpeed.Value);
        }
    }
   
    public class ViscaZoomWideWithSpeed: ViscaZoomSpeedCommand
    {
 
        public ViscaZoomWideWithSpeed(byte address)
            :this(address, new ViscaZoomSpeed())
        {}

        public ViscaZoomWideWithSpeed(byte address, byte zoomSpeed)
            :this(address, new ViscaZoomSpeed(zoomSpeed))
        {}

        public ViscaZoomWideWithSpeed(byte address, ViscaZoomSpeed zoomSpeed)
            : base(address, Visca.Commands.ZoomCommands.WideWithSpeed, zoomSpeed)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Wide with ZoomSpeed:{1}", this.Destination, ZoomSpeed.Value);
        }
    }

    public class ViscaZoomPosition: ViscaPositionCommand
    {

        public ViscaZoomPosition(byte address, int zoomPosition)
            : base(address, zoomPosition)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ZoomPosition
            });
            AppendPosition();
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.Position:{1}", this.Destination, Position);
        }
    }

    public class ViscaZoomPositionInquiry : ViscaPositionInquiry
    {
        public ViscaZoomPositionInquiry(byte address, Action<int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ZoomPosition
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Zoom.PositionInquiry", this.Destination);
        }
    }

}