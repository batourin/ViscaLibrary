using System;

namespace Visca
{
    public abstract class ViscaInquiry: ViscaTxPacket
    {
        public ViscaInquiry(byte address)
        : base(address)
        {
            Append(Visca.Inquiry);
        }

        public abstract void Process(ViscaRxPacket viscaRxPacket);
    }

    public abstract class ViscaValueInquiry : ViscaInquiry
    {
        private readonly Action<byte> _completionAction;
        public ViscaValueInquiry(byte address, Action<byte> completionAction)
        : base(address)
        {
            _completionAction = completionAction;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
#if SSHARP
            if (_completionAction != null)
                _completionAction(viscaRxPacket.PayLoad[0]);
#else
            _completionAction?.Invoke(viscaRxPacket.PayLoad[0]);
#endif
        }
    }

    public abstract class ViscaOnOffInquiry: ViscaInquiry
    {
        private readonly Action<bool> _completionAction;
        public ViscaOnOffInquiry(byte address, Action<bool> completionAction)
        : base(address)
        {
            _completionAction = completionAction;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
#if SSHARP
            if (_completionAction != null)
                _completionAction(viscaRxPacket.PayLoad[0] == Visca.On);
#else
            _completionAction?.Invoke(viscaRxPacket.PayLoad[0] == Visca.On);
#endif
        }
    }

    public abstract class ViscaPositionInquiry : ViscaInquiry
    {
        private readonly Action<int> _completionAction;
        public ViscaPositionInquiry(byte address, Action<int> completionAction)
        : base(address)
        {
            _completionAction = completionAction;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_completionAction != null)
            {
                if (viscaRxPacket.PayLoad.Length == 4)
                {
                    _completionAction( (viscaRxPacket.PayLoad[0] << 12) +
                             (viscaRxPacket.PayLoad[1] << 8) +
                             (viscaRxPacket.PayLoad[2] << 4) +
                              viscaRxPacket.PayLoad[3]
                     );
                }
                else
                    throw new ArgumentOutOfRangeException("viscaRxPacket", "Recieved packet is not Position Inquiry");
            }
        }
    }
    public abstract class Visca2DPositionInquiry : ViscaInquiry
    {
        private readonly Action<int, int> _completionAction;
        public Visca2DPositionInquiry(byte address, Action<int, int> completionAction)
        : base(address)
        {
            _completionAction = completionAction;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
            if (_completionAction != null)
            {
                if (viscaRxPacket.PayLoad.Length == 8)
                {
                    _completionAction(
                            (viscaRxPacket.PayLoad[0] << 12) +
                            (viscaRxPacket.PayLoad[1] << 8) +
                            (viscaRxPacket.PayLoad[2] << 4) +
                             viscaRxPacket.PayLoad[3],
                            (viscaRxPacket.PayLoad[4] << 12) +
                            (viscaRxPacket.PayLoad[5] << 8) +
                            (viscaRxPacket.PayLoad[6] << 4) +
                             viscaRxPacket.PayLoad[7]
                     );
                }
                else
                    throw new ArgumentOutOfRangeException("viscaRxPacket", "Recieved packet is not 2D Position Inquiry");
            }
        }
    }

    /// <summary>
    /// Class representing Visca Inquiry that normally set up one of the aspects of
    /// camera operations mode, i.e. White Balance modes, Automatic Exposure modes, etc 
    /// </summary>
    /// <typeparam name="T"><code>EnumBaseType<T></code> type pseudo enumeration
    /// representing possible command parameter</typeparam>
    /// <example>
    /// <code>
    /// ViscaModeInquiry<TestMode> testCmd = new ViscaModeInquiry<TestMode>(
    ///                1,
    ///                new byte[]{
    ///                Visca.Category.Camera1,
    ///                Visca.Commands.AE
    ///                },
    ///                "AETest",
    ///                new Action<TestMode>( mode => { if (mode == TestMode.Test) Console.PrintLine("TestMode.Test"); }));
    /// </code>
    /// </example>

    public class ViscaModeInquiry<T> : ViscaInquiry where T : EnumBaseType<T>
    {
        private readonly Action<T> _completionAction;
        private readonly string _commandName;

        public ViscaModeInquiry(byte address, byte[] commands, string commandName, Action<T> completionAction)
        : base(address)
        {
            Append(commands);
            _commandName = commandName;
            _completionAction = completionAction;
        }

        public override void Process(ViscaRxPacket viscaRxPacket)
        {
#if SSHARP
            if (_completionAction != null)
                _completionAction(EnumBaseType<T>.GetByKey(viscaRxPacket.PayLoad[0]));
#else
            _completionAction?.Invoke(EnumBaseType<T>.GetByKey(viscaRxPacket.PayLoad[0]));
#endif
        }

        public override string ToString()
        {
            return String.Format("Camera{0} {1}.Inquiry", this.Destination, _commandName);
        }
    }
}