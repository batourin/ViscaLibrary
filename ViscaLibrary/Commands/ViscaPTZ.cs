using System;

namespace Visca
{
    public class ViscaPTZHome: ViscaCommand
    {
        public ViscaPTZHome(byte address)
        : base(address)
        {
            Append(new byte[]{
                Visca.Category.PanTilt,
                Visca.Commands.PanTiltHome
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Home", this.Destination) ;
        }
    }

    public class ViscaPanSpeed: ViscaVariableWithLimits
    {

        private const byte _defaultSpeed = 0x09;

        public ViscaPanSpeed()
            : this(_defaultSpeed, ViscaDefaults.PanSpeedLimits)
        {}

        public ViscaPanSpeed(byte value)
            : this(value, ViscaDefaults.PanSpeedLimits)
        {}

        public ViscaPanSpeed(IViscaRangeLimits<byte> limits)
            : this(_defaultSpeed, limits)
        {}

        public ViscaPanSpeed(byte value, IViscaRangeLimits<byte> limits)
            : base("PanSpeed", value, limits)
        {}
    }

    public class ViscaTiltSpeed: ViscaVariableWithLimits
    {

        private const byte _defaultSpeed = 0x07;

        public ViscaTiltSpeed()
            : this(_defaultSpeed, ViscaDefaults.TiltSpeedLimits)
            {}

        public ViscaTiltSpeed(byte value)
            : this(value, ViscaDefaults.TiltSpeedLimits)
            {}

        public ViscaTiltSpeed(IViscaRangeLimits<byte> limits)
            : this(_defaultSpeed, limits)
            {}

        public ViscaTiltSpeed(byte value, IViscaRangeLimits<byte> limits)
            : base("TiltSpeed", value, limits)
            {}
    }

    public abstract class ViscaPTZCommand: ViscaDynamicCommand
    {
        public ViscaPanSpeed PanSpeed { get; private set; }
        public ViscaTiltSpeed TiltSpeed { get; private set; }
 
        public ViscaPTZCommand(byte address)
            : this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZCommand(byte address, ViscaPanSpeed panSpeed, ViscaTiltSpeed tiltSpeed)
            : base(address)
        {
            this.PanSpeed = panSpeed;
            this.TiltSpeed = tiltSpeed;
            Append(new byte[]{
                Visca.Category.PanTilt,
                Visca.Commands.PanTilt,
            });
            Append(this.PanSpeed);
            Append(this.TiltSpeed);
        }
    }

    public class ViscaPTZStop: ViscaPTZCommand
    {
 
        public ViscaPTZStop(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZStop(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZStop(byte address, ViscaPanSpeed panSpeed, ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.HorizontalStop,
                Visca.Commands.PanTiltCommands.VerticalStop
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Stop with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }

     public class ViscaPTZUp: ViscaPTZCommand
    {
 
        public ViscaPTZUp(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZUp(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZUp(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.HorizontalStop,
                Visca.Commands.PanTiltCommands.Up
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Up with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
     public class ViscaPTZDown: ViscaPTZCommand
    {
 
        public ViscaPTZDown(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZDown(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZDown(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.HorizontalStop,
                Visca.Commands.PanTiltCommands.Down
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Down with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZLeft: ViscaPTZCommand
    {
 
        public ViscaPTZLeft(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZLeft(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZLeft(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Left,
                Visca.Commands.PanTiltCommands.VerticalStop
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Left with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZRight: ViscaPTZCommand
    {
 
        public ViscaPTZRight(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZRight(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZRight(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Right,
                Visca.Commands.PanTiltCommands.VerticalStop
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Right with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZUpLeft: ViscaPTZCommand
    {
 
        public ViscaPTZUpLeft(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZUpLeft(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZUpLeft(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Left,
                Visca.Commands.PanTiltCommands.Up
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.UpLeft with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZUpRight: ViscaPTZCommand
    {
 
        public ViscaPTZUpRight(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZUpRight(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZUpRight(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Right,
                Visca.Commands.PanTiltCommands.Up
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.UpRight with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZDownLeft: ViscaPTZCommand
    {
 
        public ViscaPTZDownLeft(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZDownLeft(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZDownLeft(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Left,
                Visca.Commands.PanTiltCommands.Down
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.DownLeft with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZDownRight: ViscaPTZCommand
    {
 
        public ViscaPTZDownRight(byte address)
            :this(address, new ViscaPanSpeed(), new ViscaTiltSpeed())
        {}

        public ViscaPTZDownRight(byte address, byte panSpeed, byte tiltSpeed)
            :this(address, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed))
        {}

        public ViscaPTZDownRight(byte address, ViscaPanSpeed panSpeed,  ViscaTiltSpeed tiltSpeed)
            : base(address, panSpeed, tiltSpeed)
        {
            Append(new byte[]{
                Visca.Commands.PanTiltCommands.Down,
                Visca.Commands.PanTiltCommands.Right
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.DownRight with PanSpeed:{1} TiltSpeed:{2}", this.Destination, PanSpeed.Value, TiltSpeed.Value);
        }
    }
   
     public class ViscaPTZPosition: ViscaDynamicCommand
    {
 
        private readonly bool _relative;
        public ViscaPanSpeed PanSpeed { get; private set; }
        public ViscaTiltSpeed TiltSpeed { get; private set; }
 
        private readonly ViscaVariable _panPositionByte1;
        private readonly ViscaVariable _panPositionByte2;
        private readonly ViscaVariable _panPositionByte3;
        private readonly ViscaVariable _panPositionByte4;
        private readonly ViscaVariable _tiltPositionByte1;
        private readonly ViscaVariable _tiltPositionByte2;
        private readonly ViscaVariable _tiltPositionByte3;
        private readonly ViscaVariable _tiltPositionByte4;

        public ViscaPTZPosition(byte address, bool relative, int panPosition, int tiltPosition)
            :this(address, relative, new ViscaPanSpeed(), new ViscaTiltSpeed(), panPosition, tiltPosition)
        {}

        public ViscaPTZPosition(byte address, bool relative, byte panSpeed, byte tiltSpeed, int panPosition, int tiltPosition)
            :this(address, relative, new ViscaPanSpeed(panSpeed), new ViscaTiltSpeed(tiltSpeed), panPosition, tiltPosition)
        {}

        public ViscaPTZPosition(byte address, bool relative, ViscaPanSpeed panSpeed, ViscaTiltSpeed tiltSpeed, int panPosition, int tiltPosition)
            : base(address)
        {
            _relative = relative;
            this.PanSpeed = panSpeed;
            this.TiltSpeed = tiltSpeed;

            _panPositionByte1 = new ViscaVariable("PanPositionByte1", 0);
            _panPositionByte2 = new ViscaVariable("PanPositionByte2", 0);
            _panPositionByte3 = new ViscaVariable("PanPositionByte3", 0);
            _panPositionByte4 = new ViscaVariable("PanPositionByte4", 0);
            _tiltPositionByte1 = new ViscaVariable("TiltPositionByte1", 0);
            _tiltPositionByte2 = new ViscaVariable("TiltPositionByte2", 0);
            _tiltPositionByte3 = new ViscaVariable("TiltPositionByte3", 0);
            _tiltPositionByte4 = new ViscaVariable("TiltPositionByte4", 0);

            PanPosition = panPosition;
            TiltPosition = tiltPosition;

            Append(new byte[]{
                Visca.Category.PanTilt,
                relative ? Visca.Commands.PanTiltRelative : Visca.Commands.PanTiltAbsolute,
            });
            Append(this.PanSpeed);
            Append(this.TiltSpeed);
            Append(_panPositionByte1);
            Append(_panPositionByte2);
            Append(_panPositionByte3);
            Append(_panPositionByte4);
            Append(_tiltPositionByte1);
            Append(_tiltPositionByte2);
            Append(_tiltPositionByte3);
            Append(_tiltPositionByte4);
        }

        public bool IsRelative { get { return _relative; } }
        public int PanPosition
        {
            get
            {
                return (_panPositionByte1.Value << 12)
                    +  (_panPositionByte2.Value << 8)
                    +  (_panPositionByte3.Value << 4)
                    +   _panPositionByte4.Value;
            }
            set 
            {
                _panPositionByte1.Value = (byte)((value & 0xF000) >> 12);
                _panPositionByte2.Value = (byte)((value & 0x0F00) >> 8);
                _panPositionByte3.Value = (byte)((value & 0x00F0) >> 4);
                _panPositionByte4.Value = (byte)((value & 0x000F) );
            }
        }

        public int TiltPosition
        {
            get
            {
                return (_tiltPositionByte1.Value << 12)
                    +  (_tiltPositionByte2.Value << 8)
                    +  (_tiltPositionByte3.Value << 4)
                    +   _tiltPositionByte4.Value;
            }
            set 
            {
                _tiltPositionByte1.Value = (byte)((value & 0xF000) >> 12);
                _tiltPositionByte2.Value = (byte)((value & 0x0F00) >> 8);
                _tiltPositionByte3.Value = (byte)((value & 0x00F0) >> 4);
                _tiltPositionByte4.Value = (byte)((value & 0x000F) );
            }
        }

        public ViscaPTZPosition SetPosition(int panPosition, int tiltPosition)
        {
            PanPosition = panPosition;
            TiltPosition = tiltPosition;

            return this;
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.{1} with PanSpeed:{2} TiltSpeed:{3} PanPosition:{4} TiltPosition:{5}", this.Destination, _relative ? "Relative" : "Absolute", PanSpeed.Value, TiltSpeed.Value, PanPosition, TiltPosition);
        }
    }

    /// <summary>
    /// PTZ Position Inquiry
    /// </summary>
    public class ViscaPTZPositionInquiry : Visca2DPositionInquiry
    {
        /// <summary>
        /// Constructor for PTZ Position Inquiry
        /// </summary>
        /// <param name="address">camera address</param>
        /// <param name="action"> action to be called with two int parameters: pan position and tilt position</param>
        public ViscaPTZPositionInquiry(byte address, Action<int, int> action)
        : base(address, action)
        {
            Append(new byte[]{
                Visca.Category.PanTilt,
                Visca.Commands.PanTiltInquiry
            });
        }

        public override string ToString()
        {
            return String.Format("Camera{0} PTZ.Position.Inquiry", this.Destination);
        }
    }

}