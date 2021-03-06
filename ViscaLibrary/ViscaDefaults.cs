using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Visca defaults to 6 presets
        /// </summary>
        public static IViscaRangeLimits<byte> ViscaDefaultPresetLimits = new ViscaRangeLimits<byte>(0x00, 0x05, "Preset should be in range 0 to 5");
    }

}
