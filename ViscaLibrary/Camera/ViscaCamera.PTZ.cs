﻿using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region PTZ Commands Definition

        private readonly ViscaPTZHome _ptzHome;
        private readonly ViscaPanSpeed _ptzPanSpeed;
        private readonly ViscaTiltSpeed _ptzTiltSpeed;
        private readonly ViscaPTZStop _ptzStop;
        private readonly ViscaPTZUp _ptzUp;
        private readonly ViscaPTZDown _ptzDown;
        private readonly ViscaPTZLeft _ptzLeft;
        private readonly ViscaPTZRight _ptzRight;
        private readonly ViscaPTZUpLeft _ptzUpLeft;
        private readonly ViscaPTZUpRight _ptzUpRight;
        private readonly ViscaPTZDownLeft _ptzDownLeft;
        private readonly ViscaPTZDownRight _ptzDownRight;
        private readonly ViscaPTZPosition _ptzAbsolute;
        private readonly ViscaPTZPosition _ptzRelative;
        private readonly ViscaPTZPositionInquiry _ptzPositionInquiry;

        public class PTZPositionEventArgs : EventArgs
        {
            public int PanPosition;
            public int TiltPosition;
            public PTZPositionEventArgs(int panPosition, int tiltPosition) : base() { PanPosition = panPosition; TiltPosition = tiltPosition; }
        }

        public event EventHandler<PTZPositionEventArgs> PTZPositionChanged;

        #endregion PTZ Commands Definition

        #region PTZ Commands Implementations

        public byte PanSpeed
        {
            get { return _ptzPanSpeed.Value; }
            set
            {
                _ptzPanSpeed.Value = value;
            }
        }

        public byte TiltSpeed
        {
            get { return _ptzTiltSpeed.Value; }
            set
            {
                _ptzTiltSpeed.Value = value;
            }
        }

        private int _panPosition;
        public int PanPosition
        {
            get { return _panPosition; }
        }

        private int _tiltPosition;
        public int TiltPosition
        {
            get { return _tiltPosition; }
        }

        protected virtual void OnPTZPositionChanged(PTZPositionEventArgs e)
        {
            EventHandler<PTZPositionEventArgs> handler = PTZPositionChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        public void Home() { _visca.EnqueueCommand(_ptzHome); }
        public void Stop() { _visca.EnqueueCommand(_ptzStop); }
        public void Up() { _visca.EnqueueCommand(_ptzUp); }
        public void Down() { _visca.EnqueueCommand(_ptzDown); }
        public void Left() { _visca.EnqueueCommand(_ptzLeft); }
        public void Right() { _visca.EnqueueCommand(_ptzRight); }
        public void UpLeft() { _visca.EnqueueCommand(_ptzUpLeft); }
        public void UpRight() { _visca.EnqueueCommand(_ptzUpRight); }
        public void DownLeft() { _visca.EnqueueCommand(_ptzDownLeft); }
        public void DownRight() { _visca.EnqueueCommand(_ptzDownRight); }

        public void PositionAbsolute(int panPosition, int tiltPosition)
        {
            _visca.EnqueueCommand(_ptzAbsolute.SetPosition(panPosition, tiltPosition).OnCompletion(() => { updatePTZPosition(panPosition, tiltPosition); }));
        }

        public void PositionRelative(int panPosition, int tiltPosition)
        {
            _visca.EnqueueCommand(_ptzRelative.SetPosition(panPosition, tiltPosition));
        }

        private void updatePTZPosition(int panPosition, int tiltPosition)
        {
            if (panPosition != _panPosition || tiltPosition != _tiltPosition)
            {
                _panPosition = panPosition;
                _tiltPosition = tiltPosition;
                OnPTZPositionChanged(new PTZPositionEventArgs(panPosition, tiltPosition));
            }
        }

        public void PanTiltPositionPoll() { _visca.EnqueueCommand(_ptzPositionInquiry); }

        #endregion PTZ Commands Implementations

    }
}
