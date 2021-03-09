using System;

namespace Visca
{
    public static class ViscaDefaultsPoly
    {
        /// <summary>
        /// Poly Iris value limits are 0 - 50
        /// </summary>
        public static IViscaRangeLimits<int> IrisLimits = new ViscaRangeLimits<int>(0x0, 0x32, "Poly Iris range is from 0 to 50");

    }

}
