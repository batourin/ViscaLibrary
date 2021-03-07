using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visca
{
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
            public static class FocusAutoCommands
            {
                public const byte On = 0x02;
                public const byte Off = 0x03;
                public const byte Toggle = 0x10;
            }

            public const byte FocusPosition = 0x48;

            public const byte Memory = 0x3F;
            public static class MemoryCommands
            {
                public const byte Reset = 0x00;
                public const byte Set = 0x01;
                public const byte Recall = 0x02;
                public const byte Preset1 = 0x01;
            }

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
        }

    }
}