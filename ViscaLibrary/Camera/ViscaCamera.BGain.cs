using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region BGain Commands Definition

        private readonly ViscaBGain _bGainCmd;
        private readonly ViscaBGainValue _bGainValueCmd;
        private readonly ViscaBGainInquiry _bGainInquiry;

        public event EventHandler<PositionEventArgs> BGainChanged;

        #endregion BGain Commands Definition

        #region BGain Commands Implementations

        public void BGainUp() { _visca.EnqueueCommand(_bGainCmd.SetMode(UpDownMode.Up)); }
        public void BGainDown() { _visca.EnqueueCommand(_bGainCmd.SetMode(UpDownMode.Down)); }

        protected virtual void OnBGainChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = BGainChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _bGain;

        public int BGain
        {
            get { return _bGain; }
            set { _visca.EnqueueCommand(_bGainValueCmd.SetPosition(value).OnCompletion(() => { updateBGain(value); })); }
        }

        private void updateBGain(int bGain)
        {
            if (_bGain != bGain)
            {
                _bGain = bGain;
                OnBGainChanged(new PositionEventArgs(bGain));
            }
        }

        public void BGainPoll() { _visca.EnqueueCommand(_bGainInquiry); }

        #endregion BGain Commands Implementations

    }
}
