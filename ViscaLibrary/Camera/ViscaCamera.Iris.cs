using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Iris Commands Definition

        private readonly ViscaIris _irisCmd;
        private readonly ViscaIrisValue _irisValueCmd;
        private readonly ViscaIrisInquiry _irisInquiry;

        public event EventHandler<PositionEventArgs> IrisChanged;

        #endregion Iris Commands Definition

        #region Iris Commands Implementations

        public void IrisUp() { _visca.EnqueueCommand(_irisCmd.SetMode(UpDownMode.Up)); }
        public void IrisDown() { _visca.EnqueueCommand(_irisCmd.SetMode(UpDownMode.Down)); }

        protected virtual void OnIrisChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = IrisChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _iris;

        public int Iris
        {
            get { return _iris; }
            set { _visca.EnqueueCommand(_irisValueCmd.SetPosition(value).OnCompletion(() => { updateIris(value); })); }
        }

        private void updateIris(int iris)
        {
            if(_iris != iris)
            {
                _iris = iris;
                OnIrisChanged(new PositionEventArgs(iris));
            }
        }

        public void IrisPoll() { _visca.EnqueueCommand(_irisInquiry); }

        #endregion Iris Commands Implementations

    }
}
