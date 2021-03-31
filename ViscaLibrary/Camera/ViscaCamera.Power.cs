using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Power Commands Definition

        private readonly ViscaPower _powerCmd;
        private readonly ViscaPowerInquiry _powerInquiry;

        #endregion Power Commands Definition

        #region Power Commands Implementations

        public event EventHandler<OnOffEventArgs> PowerChanged;

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
            set
            {
                if (value)
                    _visca.EnqueueCommand(_powerCmd.SetMode(OnOffMode.On).OnCompletion(() => { _power = value; }));
                else
                    _visca.EnqueueCommand(_powerCmd.SetMode(OnOffMode.Off).OnCompletion(() => { _power = value; }));
            }
        }

        #endregion Power Commands Implementations

    }
}
