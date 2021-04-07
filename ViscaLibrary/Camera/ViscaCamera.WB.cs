using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region WB Commands Definition

        private readonly ViscaWBMode _wbCmd;
        private readonly ViscaWBInquiry _wbInquiry;

        public event EventHandler<GenericEventArgs<WBMode>> WBChanged;

        #endregion WB Commands Definition

        #region WB Commands Implementations

        protected virtual void OnWBChanged(GenericEventArgs<WBMode> e)
        {
            EventHandler<GenericEventArgs<WBMode>> handler = WBChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private WBMode _wb;

        public WBMode WB
        {
            get { return _wb; }
            set { _visca.EnqueueCommand(_wbCmd.SetMode(value).OnCompletion(() => { updateWB(value); })); }
        }

        private void updateWB(WBMode mode)
        {
            if (_wb != mode)
            {
                _wb = mode;
                OnWBChanged(new GenericEventArgs<WBMode>(mode));
            }
        }

        public void WBModePoll() { _visca.EnqueueCommand(_wbInquiry); }

        #endregion WB Commands Implementations

    }
}
