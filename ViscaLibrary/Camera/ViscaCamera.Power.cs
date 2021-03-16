using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Power Commands Definition

        private readonly ViscaPower _powerOnCmd;
        private readonly ViscaPower _powerOffCmd;
        private readonly ViscaPowerInquiry _powerInquiry;
        private readonly Action<ViscaRxPacket> _powerOnOffCmdReply;

        #endregion Power Commands Definition

        #region Power Commands Implementations

        public event EventHandler<OnOffEventArgs> PowerChanged;

        protected virtual void OnPowerChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = PowerChanged;
            if (handler != null)
                handler(this, e);
        }

        private bool _power;
        public bool Power
        {
            get { return _power; }
            set
            {
                if (value)
                    _visca.EnqueueCommand(_powerOnCmd, _powerOnOffCmdReply);
                else
                    _visca.EnqueueCommand(_powerOffCmd, _powerOnOffCmdReply);
            }
        }

        #endregion Power Commands Implementations

    }
}
