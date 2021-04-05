using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region AE Commands Definition

        private readonly ViscaAEMode _aeCmd;
        private readonly ViscaAEInquiry _aeInquiry;

        public event EventHandler<GenericEventArgs<AEMode>> AEChanged;

        #endregion AE Commands Definition

        #region AE Commands Implementations

        protected virtual void OnAEChanged(GenericEventArgs<AEMode> e)
        {
            EventHandler<GenericEventArgs<AEMode>> handler = AEChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);      
#endif
        }

        private AEMode _ae;

        public AEMode AE
        {
            get { return _ae; }
            set { _visca.EnqueueCommand(_aeCmd.SetMode(value).OnCompletion(() => { updateAE(value); })); }
        }

        private void updateAE(AEMode mode)
        {
            if (_ae != mode)
            {
                _ae = mode;
                OnAEChanged(new GenericEventArgs<AEMode>(mode));
            }
        }

        #endregion AE Commands Implementations

    }
}
