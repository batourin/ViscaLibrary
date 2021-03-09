using System;

namespace Visca
{
    /// <summary>
    /// Pan Speed values for VISCA are in range 0x01 to 0x18
    /// </summary>
    public class ViscaDefaultPanSpeedLimits : ViscaRangeLimits<byte>
    {
        public ViscaDefaultPanSpeedLimits()
            :base(0x01, 0x018, "Pan Speed should be in range 0x01 to 0x18")
        {
        }
    }

    /// <summary>
    /// Tilt Speed values for VISCA are in range 0x01 to 0x14
    /// </summary>
    public class ViscaDefaultTiltSpeedLimits : ViscaRangeLimits<byte>
    {
        public ViscaDefaultTiltSpeedLimits()
            :base(0x01, 0x14, "Tilt Speed should be in range 0x01 to 0x14")
        {
        }
    }

    /// <summary>
    /// Zoom Speed values for VISCA are in range 0x00 to 0x07
    /// </summary>
    public class ViscaDefaultZoomSpeedLimits : ViscaRangeLimits<byte>
    {
        public ViscaDefaultZoomSpeedLimits()
            :base(0x00, 0x07, "Zoom Speed should be in range 0x00 to 0x07")
        {
        }
    }

    /// <summary>
    /// Focus Speed values for VISCA are in range 0x00 to 0x07
    /// </summary>
    public class ViscaDefaultFocusSpeedLimits : ViscaRangeLimits<byte>
    {
        public ViscaDefaultFocusSpeedLimits()
            :base(0x00, 0x07, "Focus Speed should be in range 0x00 to 0x07")
        {
        }
    }

    public static class ViscaDefaults
    {
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
