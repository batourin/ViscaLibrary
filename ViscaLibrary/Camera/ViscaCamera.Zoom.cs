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
        private readonly ViscaZoomPositionInquiry _zoomPositionInquiry;

        public event EventHandler<PositionEventArgs> ZoomPositionChanged;

        #endregion Zoom Commands Definition

        #region Zoom Commands Implementations

        public void ZoomStop() { _visca.EnqueueCommand(_zoomStopCmd); }
        public void ZoomTele() { _visca.EnqueueCommand(_zoomTeleCmd); }
        public void ZoomWide() { _visca.EnqueueCommand(_zoomWideCmd); }

        public byte ZoomSpeed
        {
            get { return _zoomSpeed.Value; }
            set { _zoomSpeed.Value = value; }
        }

        public void ZoomTeleWithSpeed() { _visca.EnqueueCommand(_zoomTeleWithSpeedCmd); }
        public void ZoomWideWithSpeed() { _visca.EnqueueCommand(_zoomWideWithSpeedCmd); }

        public void ZoomSetPosition(int position) { _visca.EnqueueCommand(_zoomPositionCmd.SetPosition(position)); }

        protected virtual void OnZoomPositionChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = ZoomPositionChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _zoomPosition;
        public int ZoomPosition
        {
            get { return _zoomPosition; }
            set { _visca.EnqueueCommand(_zoomPositionCmd.SetPosition(value).OnCompletion(() => { updateZoomPosition(value); })); }
        }

        private void updateZoomPosition(int zoomPosition)
        {
            if(_zoomPosition != zoomPosition)
            {
                _zoomPosition = zoomPosition;
                OnZoomPositionChanged(new PositionEventArgs(zoomPosition));
            }
        }

        public void ZoomPositionPoll() { _visca.EnqueueCommand(_zoomPositionInquiry); }

        #endregion Zoom Commands Implementations

    }
}
