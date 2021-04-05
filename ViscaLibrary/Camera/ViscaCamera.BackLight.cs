using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region BackLight Commands Definition

        private readonly ViscaBackLight _backLightCmd;
        private readonly ViscaBackLightInquiry _backLightInquiry;

        #endregion BackLight Commands Definition

        #region BackLight Commands Implementations

        public event EventHandler<OnOffEventArgs> BackLightChanged;

        protected virtual void OnBackLightChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = BackLightChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _backLight;

        public bool BackLight
        {
            get { return _backLight; }
            set { _visca.EnqueueCommand(_backLightCmd.SetMode(value? OnOffMode.On : OnOffMode.Off).OnCompletion(() => { updateBackLight(value); })); }
        }

        private void updateBackLight(bool backLight)
        {
            if(_backLight != backLight)
            {
                _backLight = backLight;
                OnBackLightChanged(new OnOffEventArgs(backLight));
            }
        }

        #endregion BackLight Commands Implementations

    }
}
