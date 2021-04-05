using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region RGain Commands Definition

        private readonly ViscaRGain _rGainCmd;
        private readonly ViscaRGainValue _rGainValueCmd;
        private readonly ViscaRGainInquiry _rGainInquiry;

        public event EventHandler<PositionEventArgs> RGainChanged;

        #endregion RGain Commands Definition

        #region RGain Commands Implementations

        public void RGainUp() { _visca.EnqueueCommand(_rGainCmd.SetMode(UpDownMode.Up)); }
        public void RGainDown() { _visca.EnqueueCommand(_rGainCmd.SetMode(UpDownMode.Down)); }

        protected virtual void OnRGainChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = RGainChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _rGain;

        public int RGain
        {
            get { return _rGain; }
            set { _visca.EnqueueCommand(_rGainValueCmd.SetPosition(value).OnCompletion(() => { updateRGain(value); })); }
        }

        private void updateRGain(int rGain)
        {
            if (_rGain != rGain)
            {
                _rGain = rGain;
                OnRGainChanged(new PositionEventArgs(rGain));
            }
        }

        #endregion RGain Commands Implementations

    }
}
