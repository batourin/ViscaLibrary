namespace Visca
{

    public class ViscaCameraParameters
    {
        public IViscaRangeLimits<byte> PanSpeedLimits;

        public IViscaRangeLimits<byte> TiltSpeedLimits;
        public IViscaRangeLimits<byte> ZoomSpeedLimits;

        public IViscaRangeLimits<byte> FocusSpeedLimits;
    }

    public class ViscaCameraDefaultParameters: ViscaCameraParameters
    {
        public ViscaCameraDefaultParameters()
        {
            PanSpeedLimits = ViscaDefaults.PanSpeedLimits;
            TiltSpeedLimits = ViscaDefaults.TiltSpeedLimits;
            ZoomSpeedLimits = ViscaDefaults.ZoomSpeedLimits;
            FocusSpeedLimits = ViscaDefaults.FocusSpeedLimits;
        }
    }

}