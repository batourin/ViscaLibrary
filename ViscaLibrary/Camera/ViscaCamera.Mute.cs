using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Mute Commands Definition

        private readonly ViscaMute _muteCmd;
        private readonly ViscaMuteInquiry _muteInquiry;

        #endregion Mute Commands Definition

        #region Mute Commands Implementations

        public event EventHandler<OnOffEventArgs> MuteChanged;

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
            set
            {
                if (value)
                    _visca.EnqueueCommand(_muteCmd.SetMode(OnOffMode.On).OnCompletion(() => { _mute = value; }));
                else
                    _visca.EnqueueCommand(_muteCmd.SetMode(OnOffMode.Off).OnCompletion(() => { _mute = value; }));
            }
        }

        #endregion Mute Commands Implementations

    }
}
