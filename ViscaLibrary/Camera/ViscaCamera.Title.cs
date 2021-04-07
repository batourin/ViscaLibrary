using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Title Commands Definition

        private readonly ViscaTitle _titleCmd;
        private readonly ViscaTitleInquiry _titleInquiry;

        public event EventHandler<OnOffEventArgs> TitleChanged;

        #endregion Title Commands Definition

        #region Title Commands Implementations

        protected virtual void OnTitleChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = TitleChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private bool _title;
        public bool Title
        {
            get { return _title; }
            set { _visca.EnqueueCommand(_titleCmd.SetMode(value ? OnOffMode.On : OnOffMode.Off).OnCompletion(() => { updateTitle(value); })); }
        }

        private void updateTitle(bool title)
        {
            if (_title != title)
            {
                _title = title;
                OnTitleChanged(new OnOffEventArgs(title));
            }
        }

        public void TitlePoll() { _visca.EnqueueCommand(_titleInquiry); }

        #endregion Title Commands Implementations

    }
}
