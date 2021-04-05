using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Shutter Commands Definition

        private readonly ViscaShutter _shutterCmd;
        private readonly ViscaShutterValue _shutterValueCmd;
        private readonly ViscaShutterInquiry _shutterInquiry;

        public event EventHandler<PositionEventArgs> ShutterChanged;

        #endregion Shutter Commands Definition

        #region Shutter Commands Implementations

        public void ShutterUp() { _visca.EnqueueCommand(_shutterCmd.SetMode(UpDownMode.Up)); }
        public void ShutterDown() { _visca.EnqueueCommand(_shutterCmd.SetMode(UpDownMode.Down)); }

        protected virtual void OnShutterChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = ShutterChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _shutter;

        public int Shutter
        {
            get { return _shutter; }
            set { _visca.EnqueueCommand(_shutterValueCmd.SetPosition(value).OnCompletion(() => { updateShutter(value); })); }
        }

        private void updateShutter(int shutter)
        {
            if (_shutter != shutter)
            {
                _shutter = shutter;
                OnShutterChanged(new PositionEventArgs(shutter));
            }
        }

        #endregion Shutter Commands Implementations

    }
}
