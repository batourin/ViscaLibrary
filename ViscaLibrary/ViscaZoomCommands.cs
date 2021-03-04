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
        private ViscaZoomSpeedCommand _zoomSpeedCommand;

        public ViscaZoomSpeed()
            : this(_defaultSpeed, new ViscaDefaultZoomSpeedLimits())
            {}
        public ViscaZoomSpeed(byte value)
            : this(value, new ViscaDefaultZoomSpeedLimits())
            {}

        public ViscaZoomSpeed(ViscaRangeLimits<byte> limits)
            : this( _defaultSpeed, limits)
            {}

        public ViscaZoomSpeed(byte value, ViscaRangeLimits<byte> limits)
            : base("ZoomSpeed", value, limits)
        {
        }

        public ViscaZoomSpeed AttachCommand(ViscaZoomSpeedCommand zoomSpeedCommand)
        {
            _zoomSpeedCommand = zoomSpeedCommand;
            return this;
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

            ZoomSpeed = zoomSpeed.AttachCommand(this);

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

    public class ViscaZoomPosition: ViscaCommand
    {
 
        private readonly ViscaVariable _zoomPositionByte1;
        private readonly ViscaVariable _zoomPositionByte2;
        private readonly ViscaVariable _zoomPositionByte3;
        private readonly ViscaVariable _zoomPositionByte4;

        public ViscaZoomPosition(byte address, int zoomPosition)
            : base(address)
        {
            _zoomPositionByte1 = new ViscaVariable("ZoomPositionByte1", 0);
            _zoomPositionByte2 = new ViscaVariable("ZoomPositionByte2", 0);
            _zoomPositionByte3 = new ViscaVariable("ZoomPositionByte3", 0);
            _zoomPositionByte4 = new ViscaVariable("ZoomPositionByte4", 0);

            ZoomPosition = zoomPosition;

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.ZoomPosition
            });
            Append(_zoomPositionByte1);
            Append(_zoomPositionByte2);
            Append(_zoomPositionByte3);
            Append(_zoomPositionByte4);
        }

        public int ZoomPosition
        {
            get
            {
                return (_zoomPositionByte1.Value << 12)
                    +  (_zoomPositionByte2.Value << 8)
                    +  (_zoomPositionByte3.Value << 4)
                    +   _zoomPositionByte4.Value;
            }
            set 
            {
                _zoomPositionByte1.Value = (byte)((value & 0xF000) >> 12);
                _zoomPositionByte2.Value = (byte)((value & 0x0F00) >> 8);
                _zoomPositionByte3.Value = (byte)((value & 0x00F0) >> 4);
                _zoomPositionByte4.Value = (byte)((value & 0x000F) );
            }
        }

        public ViscaZoomPosition SetPosition(int zoomPosition)
        {
            ZoomPosition = zoomPosition;

            return this;
        }

        public override string ToString()
        {
            return String.Format("Camera{0} ZoomPosition:{1}", this.Destination, ZoomPosition);
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
            return String.Format("Camera{0} ZoomPositionInquiry", this.Destination);
        }
    }

}