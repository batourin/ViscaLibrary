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
            set
            {
                if (value)
                    _visca.EnqueueCommand(_backLightCmd.SetMode(OnOffMode.On).OnCompletion(() => { _backLight = value; }));
                else
                    _visca.EnqueueCommand(_backLightCmd.SetMode(OnOffMode.Off).OnCompletion(() => { _backLight = value; }));
            }
        }

        #endregion BackLight Commands Implementations

    }
}
