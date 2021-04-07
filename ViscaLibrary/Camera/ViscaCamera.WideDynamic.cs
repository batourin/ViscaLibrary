using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region WideDynamic Commands Definition

        private readonly ViscaWideDynamicMode _wideDynamicCmd;
        private readonly ViscaWideDynamicInquiry _wideDynamicInquiry;

        public event EventHandler<OnOffEventArgs> WideDynamicChanged;

        #endregion WideDynamic Commands Definition

        #region WideDynamic Commands Implementations

        protected virtual void OnWideDynamicChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = WideDynamicChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _wideDynamicMode;
        public bool WideDynamicMode
        {
            get { return _wideDynamicMode; }
            set { _visca.EnqueueCommand(_wideDynamicCmd.SetMode(value ? OnOffMode.On : OnOffMode.Off).OnCompletion(() => { updateWideDynamicMode(value); })); }
        }

        private void updateWideDynamicMode(bool wideDynamicMode)
        {
            if (_wideDynamicMode != wideDynamicMode)
            {
                _wideDynamicMode = wideDynamicMode;
                OnWideDynamicChanged(new OnOffEventArgs(wideDynamicMode));
            }
        }

        public void WideDynamicModePoll() { _visca.EnqueueCommand(_wideDynamicInquiry); }

        #endregion WideDynamic Commands Implementations

    }
}
