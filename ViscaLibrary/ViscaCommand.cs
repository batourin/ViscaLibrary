using System;

namespace Visca
{
    /// <summary>
    /// Base class for all Visca command type messages
    /// It always start with 0x01 byte in the second position after address
    /// </summary>
    public class ViscaCommand: ViscaTxPacket
    {
        public ViscaCommand(byte address)
        : base(address, false)
        {
            Append(Visca.Command);
        }
    }

    /// <summary>
    /// Base class to handle Visca comand with position variable
    /// split into 4 low bytes
    /// </summary>
    public abstract class ViscaPositionCommand: ViscaCommand
    {
        private readonly ViscaVariable _positionByte1;
        private readonly ViscaVariable _positionByte2;
        private readonly ViscaVariable _positionByte3;
        private readonly ViscaVariable _positionByte4;

        private readonly IViscaRangeLimits<int> _limits;

        public ViscaPositionCommand(byte address, int position)
            : this(address, position, new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca 4x byte range is from 0x0000 to 0xffff"))
        { }

        public ViscaPositionCommand(byte address, int position, IViscaRangeLimits<int> limits)
            : base(address)
        {
            _positionByte1 = new ViscaVariable("PositionByte1");
            _positionByte2 = new ViscaVariable("PositionByte2");
            _positionByte3 = new ViscaVariable("PositionByte3");
            _positionByte4 = new ViscaVariable("PositionByte4");

            _limits = limits;
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
                if (_limits != null && ((value < _limits.Low) || (value > _limits.High)))
                    throw new ArgumentOutOfRangeException("Position", String.Format("Assigned value should be in range between 0x{0:x4} ({0}) and 0x{1:x4} ({1})", _limits.Low, _limits.High));
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

    /// <summary>
    /// Class representing Visca Command that normally set up one of the aspects of
    /// camera operations mode, i.e. White Balance modes, Automatic Exposure modes, etc 
    /// </summary>
    /// <typeparam name="T"><code>EnumBaseType<T></code> type pseudo enumeration
    /// representing possible command parameter</typeparam>
    /// <example>
    /// <code>
    /// ViscaMode<TestMode> testCmd = new ViscaMode<TestMode>(
    ///                1,
    ///                new byte[]{
    ///                Visca.Category.Camera1,
    ///                Visca.Commands.AE
    ///                },
    ///                "AETest",
    ///                TestMode.Manual);
    /// </code>
    /// </example>
    public class ViscaModeCommand<T> : ViscaCommand where T : EnumBaseType<T>
    {
        private readonly ViscaVariable _mode;
        private readonly string _commandName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">Camera address</param>
        /// <param name="commands">Command's Category and Command</param>
        /// <param name="commandName">String representation of the command</param>
        /// <param name="mode">Mode to set</param>
        public ViscaModeCommand(byte address, byte[] commands, string commandName, T mode)
            :base(address)
        {
            _commandName = commandName;
            _mode = new ViscaVariable(_commandName + " Mode", mode.Key);

            Append(commands);
            Append(_mode);
        }
        public T Mode
        {
            get { return EnumBaseType<T>.GetByKey(_mode.Value); }
            set { _mode.Value = value.Key; }
        }

        /// <summary>
        /// Passthrough setter of mode suitable to enqueue command in one operation
        /// </summary>
        /// <param name="mode">Mode to set</param>
        /// <returns>Same object</returns>
        public ViscaModeCommand<T> SetMode(T mode)
        {
            Mode = mode;
            return this;
        }

        public override string ToString()
        {
            return String.Format("Camera{0} {1} set to {2}", this.Destination, _commandName, EnumBaseType<T>.GetByKey(_mode.Value));
        }
    }
}