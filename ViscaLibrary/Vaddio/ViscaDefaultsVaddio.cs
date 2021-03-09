using System;

namespace Visca
{
    public static class ViscaDefaultsVaddio
    {
        /// <summary>
        /// Visca Aperture value limits are not defined
        /// </summary>
        public static IViscaRangeLimits<int> ApertureLimits = new ViscaRangeLimits<int>(0x0, 0xf, "Vaddio Aperture range is from 0 to 15");

        /// <summary>
        /// Vaddio RGain range is from 0x00 to 0xff
        /// </summary>
        public static IViscaRangeLimits<int> RGainLimits = new ViscaRangeLimits<int>(0x00, 0xff, "Vaddio RGain range is from 0 to 255");

        /// <summary>
        /// Vaddio RGain range is from 0x00 to 0xff
        /// </summary>
        public static IViscaRangeLimits<int> BGainLimits = new ViscaRangeLimits<int>(0x00, 0xff, "Vaddio BGain range is from 0 to 255");

        /// <summary>
        /// Vaddio Shutter value limits are 0 to 15
        /// </summary>
        public static IViscaRangeLimits<int> ShutterLimits = new ViscaRangeLimits<int>(0x0, 0xf, "Vaddio Shutter range is from 0 to 15");

        /// <summary>
        /// Vaddio Iris value limits are 0, 0x5 to 0x11
        /// </summary>
        public static IViscaRangeLimits<int> IrisLimits = new ViscaRangeLimits<int>(0x0, 0xB, "Vaddio Iris range is from 0 to 11");

        /// <summary>
        /// Vaddio Gain value limits are 0 to 15
        /// </summary>
        public static IViscaRangeLimits<int> GainLimits = new ViscaRangeLimits<int>(0x0, 0xf, "Vaddio Gain range is from 0 to 15");

        /// <summary>
        /// Vaddio preset range is from 0 to 15
        /// </summary>
        public static IViscaRangeLimits<byte> PresetLimits = new ViscaRangeLimits<byte>(0x00, 0x0f, "Vaddio Preset should be in range 0 to 15");

        /// <summary>
        /// Vaddio Tri-Sync speed rabge is from 0x01 to 0x18
        /// </summary>
        public static IViscaRangeLimits<byte> TriSyncSpeedLimits = new ViscaRangeLimits<byte>(0x01, 0x16, "Vaddio TriSync speed should be in range 0x01 to 0x18");
        public static byte DefaultTriSyncSpeed = 0x09;

    }

}
