using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region AE Commands Definition

        private readonly ViscaAEMode _aeCmd;
        private readonly ViscaAEInquiry _aeInquiry;
        //private readonly Action<ViscaRxPacket> _aeCmdReply;
        //private readonly Action _aeCmdCompletion = () => { _ae = _aePending; };

        #endregion AE Commands Definition

        #region AE Commands Implementations

        public event EventHandler<GenericEventArgs<AEMode>> AEChanged;

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
        //private AEMode _aePending;
        public AEMode AE
        {
            get { return _ae; }
            set
            {
                //_aePending = value;
                _visca.EnqueueCommand(_aeCmd.SetMode(value), (rx) => { _ae = value; });
            }
        }

        #endregion AE Commands Implementations

    }
}
