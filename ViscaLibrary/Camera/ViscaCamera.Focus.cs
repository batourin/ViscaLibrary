using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Focus Commands Definition

        private readonly ViscaFocusStop _focusStopCmd;
        private readonly ViscaFocusFar _focusFarCmd;
        private readonly ViscaFocusNear _focusNearCmd;
        private readonly ViscaFocusSpeed _focusSpeed;
        private readonly ViscaFocusFarWithSpeed _focusFarWithSpeedCmd;
        private readonly ViscaFocusNearWithSpeed _focusNearWithSpeedCmd;
        private readonly ViscaFocusTrigger _focusTriggerCmd;
        private readonly ViscaFocusInfinity _focusInfinityCmd;
        private readonly ViscaFocusNearLimit _focusNearLimitCmd;

        private readonly ViscaFocusAuto _focusAutoCmd;
        private readonly ViscaFocusAutoToggle _focusAutoToggleCmd;
        private readonly ViscaFocusAutoInquiry _focusAutoInquiry;
        public event EventHandler<OnOffEventArgs> FocusAutoChanged;

        private readonly ViscaFocusPosition _focusPositionCmd;
        private readonly ViscaFocusPositionInquiry _focusPositionInquiry;
        public event EventHandler<PositionEventArgs> FocusPositionChanged;

        #endregion Focus Commands Definition

        public void ViscaCameraFocus()
        {
        }

        #region Focus Commands Implementations

        public void FocusStop() { _visca.EnqueueCommand(_focusStopCmd); }
        public void FocusFar() { _visca.EnqueueCommand(_focusFarCmd); }
        public void FocusNear() { _visca.EnqueueCommand(_focusNearCmd); }

        public byte FocusSpeed
        {
            get { return _focusSpeed.Value; }
            set { _focusSpeed.Value = value; }
        }

        public void FocusFarWithSpeed() { _visca.EnqueueCommand(_focusFarWithSpeedCmd); }
        public void FocusNearWithSpeed() { _visca.EnqueueCommand(_focusNearWithSpeedCmd); }

        public void FocusTrigger() { _visca.EnqueueCommand(_focusTriggerCmd); }
        public void FocusInfinity() { _visca.EnqueueCommand(_focusInfinityCmd); }

        public void FocusNearLimit(int position) { _visca.EnqueueCommand(_focusNearLimitCmd.SetPosition(position)); }

        protected virtual void OnFocusAutoChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = FocusAutoChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _focusAuto;
        public bool FocusAuto
        {
            get { return _focusAuto; }
            set { _visca.EnqueueCommand(_focusAutoCmd.SetMode(value ? OnOffMode.On: OnOffMode.Off).OnCompletion(() => { updateFocusAuto(value); })); }
        }

        public void FocusAutoToggle() { _visca.EnqueueCommand(_focusAutoToggleCmd); }

        public void FocusSetPosition(int position) { _visca.EnqueueCommand(_focusPositionCmd.SetPosition(position)); }

        protected virtual void OnFocusPositionChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = FocusPositionChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _focusPosition;
        public int FocusPosition
        {
            get { return _focusPosition; }
            set { _visca.EnqueueCommand(_focusPositionCmd.SetPosition(value).OnCompletion(()=> { updateFocusPosition(value); })); }
        }

        private void updateFocusAuto(bool focusAuto)
        {
            if (_focusAuto != focusAuto)
            {
                _focusAuto = focusAuto;
                OnFocusAutoChanged(new OnOffEventArgs(focusAuto));
            }
        }

        private void updateFocusPosition(int focusPosition)
        {
            if(_focusPosition != focusPosition)
            {
                _focusPosition = focusPosition;
                OnFocusPositionChanged(new PositionEventArgs(focusPosition));
            }
        }

        #endregion Focus Commands Implementations

    }
}
