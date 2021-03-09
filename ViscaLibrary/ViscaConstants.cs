using System;
using System.Collections.Generic;


namespace Visca
{
    using System.Collections.ObjectModel;
    /// <summary>
    /// Base psedo Enumeration type, supports exapndability by derived classes.
    /// </summary>
    /// <remarks>
    /// Base psedo Enumeration type, supports exapndability by derived classes.
    /// Useful when vendor expands possible values in the list of Visca
    /// pre-defined ones. I.e. Polycom adds 0x06 value for WB options
    /// 
    /// Inspired and copied from the code of Cassio Mosqueira
    /// https://www.codeproject.com/Articles/20805/Enhancing-C-Enums
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public abstract class EnumBaseType<T> where T : EnumBaseType<T>
    {
        protected static List<T> enumValues = new List<T>();

        public readonly byte Key;
        public readonly string Value;

        public EnumBaseType(byte key, string value)
        {
            Key = key;
            Value = value;
            enumValues.Add((T)this);
        }

        protected static ReadOnlyCollection<T> GetBaseValues()
        {
            return enumValues.AsReadOnly();
        }

        protected static T GetBaseByKey(byte key)
        {
            foreach (T t in enumValues)
            {
                if (t.Key == key) return t;
            }
            return null;
        }

        public static T GetByKey(byte key)
        {
            return GetBaseByKey(key);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class OnOffMode : EnumBaseType<OnOffMode>
    {
        public static readonly UpDownMode On = new UpDownMode(Visca.On, "On");
        public static readonly UpDownMode Off = new UpDownMode(Visca.Off, "Off");

        public OnOffMode(byte key, string value) : base(key, value)
        { }
    }

    public class AutoManualMode : EnumBaseType<AutoManualMode>
    {
        public static readonly UpDownMode Auto = new UpDownMode(Visca.Auto, "Reset");
        public static readonly UpDownMode Manual = new UpDownMode(Visca.Manual, "Manual");

        public AutoManualMode(byte key, string value) : base(key, value)
        { }
    }

    public class UpDownMode : EnumBaseType<UpDownMode>
    {
        public static readonly UpDownMode Reset = new UpDownMode(Visca.Reset, "Reset");
        public static readonly UpDownMode Up = new UpDownMode(Visca.Up, "Up");
        public static readonly UpDownMode Down = new UpDownMode(Visca.Down, "Down");

        public UpDownMode(byte key, string value) : base(key, value)
        { }
    }

    public enum ViscaCameraId : byte
    {
        Camera1 = 0x01,
        Camera2 = 0x02,
        Camera3 = 0x03,
        Camera4 = 0x04,
        Camera5 = 0x05,
        Camera6 = 0x06,
        Camera7 = 0x07,
    }


    public enum ViscaError: byte
    {
        Ok = 0x00,
        Length = 0x01,
        Syntax = 0x02,
        Full = 0x03,
        Canceled = 0x04,
        NoSocket = 0x05,
        NotExecutable = 0x41
    }

    public static partial class Visca
    {
        public const byte Command = 0x01;
        public const byte Inquiry = 0x09;
        public const byte Terminator = 0xFF;

        public const byte FullByteMask = 0x00;
        public const byte LowByteMask = 0x0F;
        public const byte HighByteMask = 0xF0;

        public const byte Reset = 0x00;
        public const byte On = 0x02;
        public const byte Off = 0x03;
        public const byte Auto = 0x02;
        public const byte Manual = 0x03;
        public const byte Up = 0x02;
        public const byte Down = 0x03;

        /// <summary>
        /// Timeout in milliseconds to ether acknoweledge command 
        /// (ACK, Completion) or response to Inquiry
        /// </summary>
        /// <remarks>
        /// Per Visca specifications 16.7 (20msec*PAL) of 1 Vertical
        /// cycle ACK/Completion has to be returned.
        /// </remarks>
        public const int ResponseTimeout = 34;

        public static class Category
        {
            public const byte Interface = 0x00;
            public const byte Camera1 = 0x04;
            public const byte PanTilt = 0x06;
            public const byte Camera2 = 0x07;
        }

        public static class Vendor
        {
            public const ushort Sony = 0x0020;
        }

        public static class Models
        {
            public const ushort IX47X = 0x0401;
        }

        public static partial class Commands
        {
            public const byte Power = 0x00;
            public static class PowerCommands
            {
                public const byte On = 0x02;
                public const byte Off = 0x03;
            }

            #region Aperture 0x02, 0x42

            public const byte Aperture = 0x02;
            public const byte ApertureValue = 0x42;

            #endregion Aperture

            #region RGain 0x03, 0x43

            public const byte RGain = 0x03;
            public const byte RGainValue = 0x43;

            #endregion RGain

            #region BGain 0x04, 0x44

            public const byte BGain = 0x04;
            public const byte BGainValue = 0x44;

            #endregion BGain

            #region Zooom 0x07, 0x47

            public const byte Zoom = 0x07;
            public static class ZoomCommands
            {
                public const byte Stop = 0x00;
                public const byte Tele = 0x02;
                public const byte Wide = 0x03;
                public const byte TeleWithSpeed = 0x20;
                public const byte WideWithSpeed = 0x30;
            }

            public const byte ZoomPosition = 0x47;

            #endregion Zoom

            #region Focus 0x08, 0x18, 0x28, 0x38, 0x48

            public const byte Focus = 0x08;
            public static class FocusCommands
            {
                public const byte Stop = 0x00;
                public const byte Far = 0x02;
                public const byte Near = 0x03;
                public const byte FarWithSpeed = 0x20;
                public const byte NearWithSpeed = 0x30;
            }
            public const byte FocusOnePush = 0x18;
            public static class FocusOnePushCommands
            {
                public const byte Trigger = 0x01;
                public const byte Infinity = 0x02;
            }
            public const byte FocusNearLimit = 0x28;
            public const byte FocusAuto = 0x38;
            public static class FocusAutoMode
            {
                public const byte On = 0x02;
                public const byte Off = 0x03;
                public const byte Toggle = 0x10;
            }
            public const byte FocusPosition = 0x48;

            #endregion Focus

            #region Shutter 0x0A, 0x4A, 0x5A

            public const byte Shutter = 0x0A;
            public const byte ShutterValue = 0x4A;
            public const byte ShutterSlow = 0x5A;
            public static class ShutterSlowMode
            {
                public const byte Auto = 0x02;
                public const byte Manual = 0x03;
            }

            #endregion Shutter

            #region Iris 0x0B, 0x4B

            public const byte Iris = 0x0B;
            public const byte IrisValue = 0x4B;

            #endregion Iris

            #region Gain 0x0C, 0x4C

            public const byte Gain = 0x0C;
            public const byte GainValue = 0x4C;

            #endregion Gain

            #region ExpComp 0x0C, 0x4C

            public const byte ExpComp = 0x0C;
            public const byte ExpCompValue = 0x4C;
            public const byte ExpCompPower = 0x5C;

            #endregion ExpComp

            #region WB 0x35

            public const byte WB = 0x35;
            public static class WBMode
            {
                public const byte Auto = 0x00;
                public const byte Indoor = 0x01;
                public const byte Outdoor = 0x02;
                public const byte OnePush = 0x03;
                public const byte ATW = 0x04;
                public const byte Manual = 0x05;
            }

            #endregion WB

            #region AE 0x39

            public const byte AE = 0x39;
            public static class AEMode
            {
                public const byte FullAuto = 0x00;
                public const byte Manual = 0x03;
                public const byte ShutterPriority = 0x0A;
                public const byte IrisPriority = 0x0B;
                public const byte GainPriority = 0x0C;
                public const byte Bright = 0x0D;
                public const byte ShutterAuto = 0x1A;
                public const byte IrisAuto = 0x1B;
                public const byte GainAuto = 0x1C;
            }

            #endregion AE

            #region Memory 0x3f

            public const byte Memory = 0x3F;
            public static class MemoryCommands
            {
                public const byte Reset = 0x00;
                public const byte Set = 0x01;
                public const byte Recall = 0x02;
                public const byte Preset1 = 0x01;
            }

            #endregion Memory

            #region PTZ

            public const byte PanTilt = 0x01;
            public static class PanTiltCommands
            {
                public const byte Up = 0x01;
                public const byte Down = 0x02;
                public const byte VerticalStop = 0x03;
                public const byte Left = 0x01;
                public const byte Right = 0x02;
                public const byte HorizontalStop = 0x03;
            }

            public const byte PanTiltAbsolute = 0x02;
            public const byte PanTiltRelative = 0x03;
            public const byte PanTiltHome = 0x04;
            public const byte PanTiltInquiry = 0x12;

            #endregion PTZ
        }

    }
}