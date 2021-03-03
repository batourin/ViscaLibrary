using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Zoom Commands Definition

        private readonly ViscaZoomStop _zoomStopCmd;
        private readonly ViscaZoomTele _zoomTeleCmd;
        private readonly ViscaZoomWide _zoomWideCmd;
        private readonly ViscaZoomSpeed _zoomSpeed;
        private readonly ViscaZoomTeleWithSpeed _zoomTeleWithSpeedCmd;
        private readonly ViscaZoomWideWithSpeed _zoomWideWithSpeedCmd;
        private readonly ViscaZoomPosition _zoomPositionCmd;

        #endregion Zoom Commands Definition

        #region Zoom Commands Implementations
        public void ZoomStop() { _visca.EnqueueCommand(_zoomStopCmd); }
        public void ZoomTele() { _visca.EnqueueCommand(_zoomTeleCmd); }
        public void ZoomWide() { _visca.EnqueueCommand(_zoomWideCmd); }

        public byte ZoomSpeed
        {
            get { return _zoomSpeed.Value; }
            set
            {
                _zoomSpeed.Value = value;
            }
        }

        public void ZoomTeleWithSpeed() { _visca.EnqueueCommand(_zoomTeleWithSpeedCmd); }
        public void ZoomWideWithSpeed() { _visca.EnqueueCommand(_zoomWideWithSpeedCmd); }

        public void ZoomPosition(int position) { _visca.EnqueueCommand(_zoomPositionCmd.SetPosition(position)); }

        #endregion Zoom Commands Implementations

    }
}
