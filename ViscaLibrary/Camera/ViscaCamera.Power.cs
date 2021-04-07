using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Power Commands Definition

        private readonly ViscaPower _powerCmd;
        private readonly ViscaPowerInquiry _powerInquiry;

        public event EventHandler<OnOffEventArgs> PowerChanged;

        #endregion Power Commands Definition

        #region Power Commands Implementations

        protected virtual void OnPowerChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = PowerChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _power;
        public bool Power
        {
            get { return _power; }
            set { _visca.EnqueueCommand(_powerCmd.SetMode(value? OnOffMode.On : OnOffMode.Off).OnCompletion(() => { updatePower(value); })); }
        }

        private void updatePower(bool power)
        {
            if(_power != power)
            {
                _power = power;
                OnPowerChanged(new OnOffEventArgs(power));
            }
        }

        public void PowerPoll() { _visca.EnqueueCommand(_powerInquiry); }

        #endregion Power Commands Implementations

    }
}
