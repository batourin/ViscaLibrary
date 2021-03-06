namespace Visca
{

    public class ViscaCameraParameters
    {
        public ViscaRangeLimits<byte> PanSpeedLimits;

        public ViscaRangeLimits<byte> TiltSpeedLimits;
        public ViscaRangeLimits<byte> ZoomSpeedLimits;

        public ViscaRangeLimits<byte> FocusSpeedLimits;
    }

    public class ViscaCameraDefaultParameters: ViscaCameraParameters
    {
        public ViscaCameraDefaultParameters()
        {
            PanSpeedLimits = new ViscaDefaultPanSpeedLimits();
            TiltSpeedLimits = new ViscaDefaultTiltSpeedLimits();
            ZoomSpeedLimits = new ViscaDefaultZoomSpeedLimits();
            FocusSpeedLimits = new ViscaDefaultFocusSpeedLimits();
        }
    }

}