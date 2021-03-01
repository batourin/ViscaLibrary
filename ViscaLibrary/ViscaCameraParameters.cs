namespace Visca
{

    public class ViscaRangeLimits<T>
    {
        public T Low;
        public T High;
        public string Message; 
    }

    /// <summary>
    /// Pan Speed values for VISCA are in range 0x01 to 0x18
    /// </summary>
    public class ViscaDefaultPanSpeedLimits:  ViscaRangeLimits<byte>
    {
        public ViscaDefaultPanSpeedLimits()
        {
            Low = 0x01;
            High = 0x18;
            Message = "Pan Speed should be in range 0x01 to 0x18";
        }
    }

    /// <summary>
    /// Tilt Speed values for VISCA are in range 0x01 to 0x14
    /// </summary>
    public class ViscaDefaultTiltSpeedLimits:  ViscaRangeLimits<byte>
    {
        public ViscaDefaultTiltSpeedLimits()
        {
            Low = 0x01;
            High = 0x14;
            Message = "Tilt Speed should be in range 0x01 to 0x14";
        }
    }

    /// <summary>
    /// Zoom Speed values for VISCA are in range 0x00 to 0x07
    /// </summary>
    public class ViscaDefaultZoomSpeedLimits:  ViscaRangeLimits<byte>
    {
        public ViscaDefaultZoomSpeedLimits()
        {
            Low = 0x00;
            High = 0x07;
            Message = "Zoom Speed should be in range 0x00 to 0x07";
        }
    }

    /// <summary>
    /// Focus Speed values for VISCA are in range 0x00 to 0x07
    /// </summary>
    public class ViscaDefaultFocusSpeedLimits:  ViscaRangeLimits<byte>
    {
        public ViscaDefaultFocusSpeedLimits()
        {
            Low = 0x00;
            High = 0x07;
            Message = "Focus Speed should be in range 0x00 to 0x07";
        }
    }

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