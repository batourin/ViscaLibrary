using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Aperture Commands Definition

        private readonly ViscaAperture _apertureCmd;
        private readonly ViscaApertureValue _apertureValueCmd;
        private readonly ViscaApertureInquiry _apertureInquiry;

        #endregion Aperture Commands Definition

        #region Aperture Commands Implementations

        public void ApertureUp() { _visca.EnqueueCommand(_apertureCmd.SetMode(UpDownMode.Up)); }
        public void ApertureDown() { _visca.EnqueueCommand(_apertureCmd.SetMode(UpDownMode.Down)); }

        public event EventHandler<PositionEventArgs> ApertureChanged;

        protected virtual void OnApertureChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = ApertureChanged;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        private int _aperture;

        public int Aperture
        {
            get { return _aperture; }
            set { _visca.EnqueueCommand(_apertureValueCmd.SetPosition(value).OnCompletion(() => { updateAperture(value); })); }
        }

        private void updateAperture(int aperture)
        {
            if (_aperture != aperture)
            {
                _aperture = aperture;
                OnApertureChanged(new PositionEventArgs(aperture));
            }
        }

        #endregion Aperture Commands Implementations

    }
}
