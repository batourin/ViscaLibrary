using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region ExpComp Commands Definition

        private readonly ViscaExpComp _expCompCmd;
        private readonly ViscaExpCompValue _expCompValueCmd;
        private readonly ViscaExpCompInquiry _expCompInquiry;

        #endregion ExpComp Commands Definition

        #region ExpComp Commands Implementations

        public void ExpCompUp() { _visca.EnqueueCommand(_expCompCmd.SetMode(UpDownMode.Up)); }
        public void ExpCompDown() { _visca.EnqueueCommand(_expCompCmd.SetMode(UpDownMode.Down)); }

        public event EventHandler<PositionEventArgs> ExpCompChanged;

        protected virtual void OnExpCompChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = ExpCompChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _expComp;

        public int ExpComp
        {
            get { return _expComp; }
            set { _visca.EnqueueCommand(_expCompValueCmd.SetPosition(value).OnCompletion(() => { updateExpComp(value); })); }
        }

        private void updateExpComp(int expComp)
        {
            if(_expComp != expComp)
            {
                _expComp = expComp;
                OnExpCompChanged(new PositionEventArgs(expComp));
            }
        }

        public void ExpCompPoll() { _visca.EnqueueCommand(_expCompInquiry); }

        #endregion ExpComp Commands Implementations

    }
}
