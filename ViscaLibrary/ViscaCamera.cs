using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visca
{

    public partial class ViscaCamera
    {
        private readonly ViscaCameraId _id;
        private readonly ViscaCameraParameters _parameters;
        private readonly ViscaProtocolProcessor _visca;

        #region Power Commands Definition

        private readonly ViscaPower _powerOnCmd;
        private readonly ViscaPower _powerOffCmd;
        private readonly ViscaPowerInquiry _powerInquiry;
        private readonly Action<ViscaRxPacket> _powerOnOffCmdReply;

        #endregion Power Commands Definition

        #region Zoom Commands Definition

        private readonly ViscaZoomStop _zoomStopCmd;
        private readonly ViscaZoomTele _zoomTeleCmd;
        private readonly ViscaZoomWide _zoomWideCmd;
        private readonly ViscaZoomSpeed _zoomSpeed;
        private readonly ViscaZoomTeleWithSpeed _zoomTeleWithSpeedCmd;
        private readonly ViscaZoomWideWithSpeed _zoomWideWithSpeedCmd;
        private readonly ViscaZoomPosition _zoomPositionCmd;

        #endregion Zoom Commands Definition

        #region PTZ Commands Definition

        private readonly ViscaPTZHome _ptzHome;
        private readonly ViscaPanSpeed _ptzPanSpeed;
        private readonly ViscaTiltSpeed _ptzTiltSpeed;
        private readonly ViscaPTZStop _ptzStop;
        private readonly ViscaPTZUp _ptzUp;
        private readonly ViscaPTZDown _ptzDown;
        private readonly ViscaPTZLeft _ptzLeft;
        private readonly ViscaPTZRight _ptzRight;
        private readonly ViscaPTZUpLeft _ptzUpLeft;
        private readonly ViscaPTZUpRight _ptzUpRight;
        private readonly ViscaPTZDownLeft _ptzDownLeft;
        private readonly ViscaPTZDownRight _ptzDownRight;
        private readonly ViscaPTZPosition _ptzAbsolute;
        private readonly ViscaPTZPosition _ptzRelative;

        #endregion PTZ Commands Definition

        public ViscaCamera(ViscaCameraId id, ViscaCameraParameters parameters, ViscaProtocolProcessor visca)
        {
            _id = id;

            if(parameters == null)
                _parameters = new ViscaCameraDefaultParameters();
            else
                _parameters = parameters;
            
            _visca = visca;

            if(!_visca.Attach(id, this))
                throw new ArgumentOutOfRangeException("Id", "Camera with supplied Id already registered");

            _powerOnCmd = new ViscaPower((byte)id, true);
            _powerOffCmd = new ViscaPower((byte)id, false);
            _powerInquiry = new ViscaPowerInquiry((byte)id, new Action<bool>( power => { _power = power; OnPowerChanged(new OnOffEventArgs(power)); }));
            _powerOnOffCmdReply = new Action<ViscaRxPacket>( rxPacket => { if ( rxPacket.IsCompletionCommand ) _visca.EnqueueCommand(_powerInquiry); } );

            _zoomStopCmd = new ViscaZoomStop((byte)id);
            _zoomTeleCmd = new ViscaZoomTele((byte)id);
            _zoomWideCmd = new ViscaZoomWide((byte)id);
            _zoomSpeed = new ViscaZoomSpeed(_parameters.ZoomSpeedLimits);
            _zoomTeleWithSpeedCmd = new ViscaZoomTeleWithSpeed((byte)id, _zoomSpeed);
            _zoomWideWithSpeedCmd = new ViscaZoomWideWithSpeed((byte)id, _zoomSpeed);
            _zoomPositionCmd = new ViscaZoomPosition((byte)id, 0);

            _focusStopCmd = new ViscaFocusStop((byte)id);
            _focusFarCmd = new ViscaFocusFar((byte)id);
            _focusNearCmd = new ViscaFocusNear((byte)id);
            _focusSpeed = new ViscaFocusSpeed(_parameters.FocusSpeedLimits);
            _focusFarWithSpeedCmd = new ViscaFocusFarWithSpeed((byte)id, _focusSpeed);
            _focusNearWithSpeedCmd = new ViscaFocusNearWithSpeed((byte)id, _focusSpeed);
            _focusPositionCmd = new ViscaFocusPosition((byte)id, 0);
            _focusTriggerCmd = new ViscaFocusTrigger((byte)id);
            _focusInfinityCmd = new ViscaFocusInfinity((byte)id);
            _focusPositionInquiry = new ViscaFocusPositionInquiry((byte)id, new Action<int>(position => { _focusPosition = position; OnFocusPositionChanged(new PositionEventArgs(position)); }));
            _focusAutoOnCmd = new ViscaFocusAutoOn((byte)id);
            _focusAutoOffCmd = new ViscaFocusAutoOff((byte)id);
            _focusAutoToggleCmd = new ViscaFocusAutoToggle((byte)id);
            _focusAutoInquiry = new ViscaFocusAutoInquiry((byte)id, new Action<bool>(focusAuto => { _focusAuto = focusAuto; OnFocusAutoChanged(new OnOffEventArgs(focusAuto)); }));
            _focusAutoOnOffCmdReply = new Action<ViscaRxPacket>(rxPacket => { if (rxPacket.IsCompletionCommand) _visca.EnqueueCommand(_focusAutoInquiry); });


            // PTZ Commands
            _ptzHome = new ViscaPTZHome((byte)id);
            _ptzPanSpeed = new ViscaPanSpeed(_parameters.PanSpeedLimits);
            _ptzTiltSpeed = new ViscaTiltSpeed(_parameters.TiltSpeedLimits);
            _ptzStop = new ViscaPTZStop((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzUp = new ViscaPTZUp((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzDown = new ViscaPTZDown((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzLeft = new ViscaPTZLeft((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzRight = new ViscaPTZRight((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzUpLeft = new ViscaPTZUpLeft((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzUpRight = new ViscaPTZUpRight((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzDownLeft = new ViscaPTZDownLeft((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzDownRight = new ViscaPTZDownRight((byte)id, _ptzPanSpeed, _ptzTiltSpeed);
            _ptzAbsolute = new ViscaPTZPosition((byte)id, false, _ptzPanSpeed, _ptzTiltSpeed, 0, 0);
            _ptzRelative = new ViscaPTZPosition((byte)id, true, _ptzPanSpeed, _ptzTiltSpeed, 0, 0);

        }

        #region Power Commands Implementations

        public event EventHandler<OnOffEventArgs> PowerChanged;

        protected virtual void OnPowerChanged(OnOffEventArgs e)
        {
            EventHandler<OnOffEventArgs> handler = PowerChanged;
            if(handler != null)
                handler(this, e);
        }
        private bool _power;
        public bool Power
        {
            get { return _power;}
            set 
            {
                if(value)
                    _visca.EnqueueCommand(_powerOnCmd, _powerOnOffCmdReply);
                else
                    _visca.EnqueueCommand(_powerOffCmd, _powerOnOffCmdReply);
            }
        }

        #endregion Power Commands Implementations

        #region Zoom Commands Implementations

        public void ZoomStop() { _visca.EnqueueCommand(_zoomStopCmd); }
        public void ZoomTele() { _visca.EnqueueCommand(_zoomTeleCmd); }
        public void ZoomWide() { _visca.EnqueueCommand(_zoomWideCmd); }

        public byte ZoomSpeed
        {
            get { return _zoomSpeed.Value; }
            set 
            {
                _zoomSpeed.Value = value;
            }
        }

        public void ZoomTeleWithSpeed() { _visca.EnqueueCommand(_zoomTeleWithSpeedCmd); }
        public void ZoomWideWithSpeed() { _visca.EnqueueCommand(_zoomWideWithSpeedCmd); }

        public void ZoomPosition(int position) { _visca.EnqueueCommand(_zoomPositionCmd.SetPosition(position)); }

        #endregion Zoom Commands Implementations

        public class PositionEventArgs : EventArgs
        {
            public int Position;
            public PositionEventArgs(int position) : base() { this.Position = position; }
        }

        public class OnOffEventArgs : EventArgs
        {
            private readonly bool _value;
            public bool On { get { return _value; } }
            public bool Off { get { return !_value; } }
            public OnOffEventArgs(bool value) : base() { _value = value; }
        }

        #region PTZ Commands Implementations

        public byte PanSpeed
        {
            get { return _ptzPanSpeed.Value; }
            set 
            {
                _ptzPanSpeed.Value = value;
            }
        }

        public byte TiltSpeed
        {
            get { return _ptzTiltSpeed.Value; }
            set 
            {
                _ptzTiltSpeed.Value = value;
            }
        }

        public void Home() { _visca.EnqueueCommand(_ptzHome); }
        public void Stop() { _visca.EnqueueCommand(_ptzStop); }
        public void Up() { _visca.EnqueueCommand(_ptzUp); }
        public void Down() { _visca.EnqueueCommand(_ptzDown); }
        public void Left() { _visca.EnqueueCommand(_ptzLeft); }
        public void Right() { _visca.EnqueueCommand(_ptzRight); }
        public void UpLeft() { _visca.EnqueueCommand(_ptzUpLeft); }
        public void UpRight() { _visca.EnqueueCommand(_ptzUpRight); }
        public void DownLeft() { _visca.EnqueueCommand(_ptzDownLeft); }
        public void DownRight() { _visca.EnqueueCommand(_ptzDownRight); }

        public void PositionAbsolute(int panPosition, int tiltPosition)
        {
            _visca.EnqueueCommand(_ptzAbsolute.SetPosition(panPosition, tiltPosition));
        }

        public void PositionRelative(int panPosition, int tiltPosition)
        {
            _visca.EnqueueCommand(_ptzRelative.SetPosition(panPosition, tiltPosition));
        }

        #endregion PTZ Commands Implementations

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("ViscaCamera: {0}\r\n", (byte)_id);
            sb.AppendFormat("\tPower: {0:X1}\r\n", Power);

            return sb.ToString();
        }
    }
}