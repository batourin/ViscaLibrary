using System;

namespace Visca
{
    public static class ViscaDefaults
    {
        /// <summary>
        /// Pan Speed values for VISCA are in range 0x01 to 0x18
        /// </summary>
        public static IViscaRangeLimits<byte> PanSpeedLimits = new ViscaRangeLimits<byte>(0x1, 0x18, "Pan Speed should be in range from 0x1 to 0x18");

        /// <summary>
        /// Tilt Speed values for VISCA are in range 0x01 to 0x14
        /// </summary>
        public static IViscaRangeLimits<byte> TiltSpeedLimits = new ViscaRangeLimits<byte>(0x1, 0x14, "Tilt Speed should be in range from 0x1 to 0x14");

        /// <summary>
        /// Zoom Speed values for VISCA are in range 0x00 to 0x07
        /// </summary>
        public static IViscaRangeLimits<byte> ZoomSpeedLimits = new ViscaRangeLimits<byte>(0x0, 0x7, "Zoom Speed should be in range from 0 to 7");

        /// <summary>
        /// Focus Speed values for VISCA are in range 0x00 to 0x07
        /// </summary>
        public static IViscaRangeLimits<byte> FocusSpeedLimits = new ViscaRangeLimits<byte>(0x0, 0x7, "Focus Speed should be in range from 0 to 7");

        /// <summary>
        /// Visca Aperture value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> ApertureLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca RGain value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> RGainLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca BGain value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> BGainLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca Shutter value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> ShutterLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca Shutter value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> IrisLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca Shutter value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> GainLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca Shutter value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> ExpCompLimits = new ViscaRangeLimits<int>(0x0000, 0xffff, "Visca does not define limits");

        /// <summary>
        /// Visca defaults to 6 presets
        /// </summary>
        public static IViscaRangeLimits<byte> ViscaDefaultPresetLimits = new ViscaRangeLimits<byte>(0x00, 0x05, "Preset should be in range 0 to 5");

    }

}
