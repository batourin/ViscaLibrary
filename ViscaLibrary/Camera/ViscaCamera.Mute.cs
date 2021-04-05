using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Mute Commands Definition

        private readonly ViscaMute _muteCmd;
        private readonly ViscaMuteInquiry _muteInquiry;

        public event EventHandler<OnOffEventArgs> MuteChanged;

        #endregion Mute Commands Definition

        #region Mute Commands Implementations

        protected virtual void OnMuteChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = MuteChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _mute;
        public bool Mute
        {
            get { return _mute; }
            set { _visca.EnqueueCommand(_muteCmd.SetMode(value? OnOffMode.On : OnOffMode.Off).OnCompletion(() => { updateMute(value); })); }
        }

        private void updateMute(bool mute)
        {
            if(_mute != mute)
            {
                _mute = mute;
                OnMuteChanged(new OnOffEventArgs(mute));
            }
        }

        #endregion Mute Commands Implementations

    }
}
