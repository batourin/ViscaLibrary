using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visca
{
    public static class ViscaDefaultsVaddio
    {
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
