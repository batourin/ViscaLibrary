﻿using System;
using System.Collections.Generic;
using System.Text;
#if SSHARP
using Crestron.SimplSharp;
#else
using System.Threading;
#endif

namespace Visca
{

    public partial class ViscaCamera
    {
        public class GenericEventArgs<T> : EventArgs
        {
            public T EventData { get; private set; }

            public GenericEventArgs(T EventData)
            {
                this.EventData = EventData;
            }
        }

        public class OnOffEventArgs : EventArgs
        {
            private readonly bool _value;
            public bool On { get { return _value; } }
            public bool Off { get { return !_value; } }
            public OnOffEventArgs(bool value) : base() { _value = value; }
        }

        public class PositionEventArgs : EventArgs
        {
            public int Position;
            public PositionEventArgs(int position) : base() { this.Position = position; }
        }

        private readonly ViscaCameraId _id;
        private readonly ViscaCameraParameters _parameters;
        private readonly ViscaProtocolProcessor _visca;
        private readonly List<ViscaInquiry> _pollCommands;

        /// <summary>
        /// Poll timer controls how often poll camera
        /// </summary>
#if SSHARP
        private readonly CTimer _pollTimer;
#else
        private readonly Timer _pollTimer;
#endif

        public ViscaCamera(ViscaCameraId id, ViscaCameraParameters parameters, ViscaProtocolProcessor visca)
        {
            _id = id;

            if(parameters == null)
                _parameters = new ViscaCameraDefaultParameters();
            else
                _parameters = parameters;
            
            _visca = visca;

            _pollCommands = new List<ViscaInquiry>();

#if SSHARP
            _pollTimer = new CTimer((o) => 
#else
            _pollTimer = new Timer((o) =>
#endif
                {
                    Poll();
                }, null, Timeout.Infinite, Timeout.Infinite);

            #region AE Commands Constructors

            _aeCmd = new ViscaAEMode((byte)id, AEMode.FullAuto);
            _aeInquiry = new ViscaAEInquiry((byte)id, new Action<AEMode>(mode => { updateAE(mode); }));

            #endregion AE Commands Constructors

            #region Aperture Commands Constructors

            _apertureCmd = new ViscaAperture((byte)id, UpDownMode.Up);
            _apertureValueCmd = new ViscaApertureValue((byte)id, 0);
            _apertureInquiry = new ViscaApertureInquiry((byte)id, new Action<int>(position => { updateAperture(position); }));

            #endregion Aperture Commands Constructors

            #region BackLight Commands Constructors

            _backLightCmd = new ViscaBackLight((byte)id, OnOffMode.On);
            _backLightInquiry = new ViscaBackLightInquiry((byte)id, new Action<OnOffMode>(mode => { updateBackLight(mode); }));

            #endregion BackLight Commands Constructors

            #region BGain Commands Constructors

            _bGainCmd = new ViscaBGain((byte)id, UpDownMode.Up);
            _bGainValueCmd = new ViscaBGainValue((byte)id, 0);
            _bGainInquiry = new ViscaBGainInquiry((byte)id, new Action<int>(position => { updateBGain(position); }));

            #endregion Gain Commands Constructors

            #region ExpComp Commands Constructors

            _expCompCmd = new ViscaExpComp((byte)id, UpDownMode.Up);
            _expCompValueCmd = new ViscaExpCompValue((byte)id, 0);
            _expCompInquiry = new ViscaExpCompInquiry((byte)id, new Action<int>(position => { updateExpComp(position); }));

            #endregion ExpComp Commands Constructors

            #region Focus Commands Constructors

            _focusStopCmd = new ViscaFocusStop((byte)id);
            _focusFarCmd = new ViscaFocusFar((byte)id);
            _focusNearCmd = new ViscaFocusNear((byte)id);
            _focusSpeed = new ViscaFocusSpeed(_parameters.FocusSpeedLimits);
            _focusFarWithSpeedCmd = new ViscaFocusFarWithSpeed((byte)id, _focusSpeed);
            _focusNearWithSpeedCmd = new ViscaFocusNearWithSpeed((byte)id, _focusSpeed);
            _focusTriggerCmd = new ViscaFocusTrigger((byte)id);
            _focusInfinityCmd = new ViscaFocusInfinity((byte)id);

            _focusNearLimitCmd = new ViscaFocusNearLimit((byte)id, 0x1000);

            _focusAutoCmd = new ViscaFocusAuto((byte)id, OnOffMode.On);
            _focusAutoToggleCmd = new ViscaFocusAutoToggle((byte)id);
            _focusAutoInquiry = new ViscaFocusAutoInquiry((byte)id, new Action<OnOffMode>(mode => { updateFocusAuto(mode); }));

            _focusPositionCmd = new ViscaFocusPosition((byte)id, 0);
            _focusPositionInquiry = new ViscaFocusPositionInquiry((byte)id, new Action<int>(position => { updateFocusPosition(position); }));

            #endregion Focus Commands Constructors

            #region Gain Commands Constructors

            _gainCmd = new ViscaGain((byte)id, UpDownMode.Up);
            _gainValueCmd = new ViscaGainValue((byte)id, 0);
            _gainInquiry = new ViscaGainInquiry((byte)id, new Action<int>(position => { updateGain(position); }));

            #endregion Gain Commands Constructors

            #region Iris Commands Constructors

            _irisCmd = new ViscaIris((byte)id, UpDownMode.Up);
            _irisValueCmd = new ViscaIrisValue((byte)id, 0);
            _irisInquiry = new ViscaIrisInquiry((byte)id, new Action<int>(position => { updateIris(position); }));

            #endregion Gain Commands Constructors

            #region Memory Commands Constructors

            _memorySetCmd = new ViscaMemorySet((byte)id, 0);
            _memoryRecallCmd = new ViscaMemoryRecall((byte)id, 0);

            #endregion Memory Commands Constructors

            #region Mute Commands Constructors

            _muteCmd = new ViscaMute((byte)id, OnOffMode.On);
            _muteInquiry = new ViscaMuteInquiry((byte)id, new Action<OnOffMode>(mode => { updateMute(mode); }));

            #endregion Mute Commands Constructors

            #region Power Commands Constructors

            _powerCmd = new ViscaPower((byte)id, OnOffMode.On);
            _powerInquiry = new ViscaPowerInquiry((byte)id, new Action<OnOffMode>( mode => { updatePower(mode); }));

            #endregion Power Commands Constructors

            #region PTZ Commands Constructors

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
            _ptzPositionInquiry = new ViscaPTZPositionInquiry((byte)id, new Action<int, int>((panPosition, tiltPosition) => { updatePTZPosition(panPosition, tiltPosition); }));

            #endregion PTZ Commands Constructors

            #region RGain Commands Constructors

            _rGainCmd = new ViscaRGain((byte)id, UpDownMode.Up);
            _rGainValueCmd = new ViscaRGainValue((byte)id, 0);
            _rGainInquiry = new ViscaRGainInquiry((byte)id, new Action<int>(position => { updateRGain(position); }));

            #endregion RGain Commands Constructors

            #region Shutter Commands Constructors

            _shutterCmd = new ViscaShutter((byte)id, UpDownMode.Up);
            _shutterValueCmd = new ViscaShutterValue((byte)id, 0);
            _shutterInquiry = new ViscaShutterInquiry((byte)id, new Action<int>(position => { updateShutter(position); }));

            #endregion Gain Commands Constructors

            #region Title Commands Constructors

            _titleCmd = new ViscaTitle((byte)id, OnOffMode.On);
            _titleInquiry = new ViscaTitleInquiry((byte)id, new Action<OnOffMode>(mode => { updateTitle(mode); }));

            #endregion Power Commands Constructors

            #region WB Commands Constructors

            _wbCmd = new ViscaWBMode((byte)id, WBMode.Auto);
            _wbInquiry = new ViscaWBInquiry((byte)id, new Action<WBMode>(mode => { updateWB(mode); }));

            #endregion WB Commands Constructors

            #region WideDynamic Commands Constructors

            _wideDynamicCmd = new ViscaWideDynamicMode((byte)id, OnOffMode.On);
            _wideDynamicInquiry = new ViscaWideDynamicInquiry((byte)id, new Action<OnOffMode>(mode => { updateWideDynamicMode(mode); }));

            #endregion WideDynamic Commands Constructors

            #region Zoom Commands Constructors

            _zoomStopCmd = new ViscaZoomStop((byte)id);
            _zoomTeleCmd = new ViscaZoomTele((byte)id);
            _zoomWideCmd = new ViscaZoomWide((byte)id);
            _zoomSpeed = new ViscaZoomSpeed(_parameters.ZoomSpeedLimits);
            _zoomTeleWithSpeedCmd = new ViscaZoomTeleWithSpeed((byte)id, _zoomSpeed);
            _zoomWideWithSpeedCmd = new ViscaZoomWideWithSpeed((byte)id, _zoomSpeed);
            _zoomPositionCmd = new ViscaZoomPosition((byte)id, 0);
            _zoomPositionInquiry = new ViscaZoomPositionInquiry((byte)id, new Action<int>(position => { updateZoomPosition(position); }));

            #endregion Zoom Commands Constructors

            _pollCommands.Add(_aeInquiry);
            _pollCommands.Add(_apertureInquiry);
            _pollCommands.Add(_backLightInquiry);
            _pollCommands.Add(_bGainInquiry);
            _pollCommands.Add(_expCompInquiry);
            _pollCommands.Add(_gainInquiry);
            _pollCommands.Add(_focusAutoInquiry);
            _pollCommands.Add(_focusPositionInquiry);
            _pollCommands.Add(_irisInquiry);
            _pollCommands.Add(_muteInquiry);
            _pollCommands.Add(_powerInquiry);
            _pollCommands.Add(_ptzPositionInquiry);
            _pollCommands.Add(_rGainInquiry);
            _pollCommands.Add(_shutterInquiry);
            _pollCommands.Add(_titleInquiry);
            _pollCommands.Add(_wbInquiry);
            _pollCommands.Add(_wideDynamicInquiry);
            _pollCommands.Add(_zoomPositionInquiry);
        }

        #region Polling commands

        /// <summary>
        /// Enable or Disable Poll option
        /// </summary>
        public bool PollEnabled { get; set; }

        private int _pollTime = Timeout.Infinite;
        /// <summary>
        /// Poll interval for automatic polling
        /// </summary>
        public int PollTime
        {
            get { return _pollTime; }
            set 
            {
                if(_pollTime != value)
                {
                    _pollTime = value;
#if SSHARP
                    _pollTimer.Reset(_pollTime, _pollTime);
#else
                    _pollTimer.Change(_pollTime, _pollTime);
#endif
                }
            }
        }

        /// <summary>
        /// Manual Poll, have effect only if Polling Enabled
        /// </summary>
        public void Poll()
        {
            if (PollEnabled)
            {
                foreach (var command in _pollCommands)
                    _visca.EnqueueCommand(command);
            }
        }

        #endregion Polling Commands

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("ViscaCamera: {0}\r\n", (byte)_id);
            sb.AppendFormat("\tPower:\t\t{0}\r\n", Power ? "ON" : "OFF");
            sb.AppendFormat("\tPan:\t\t{0}\r\n", PanPosition);
            sb.AppendFormat("\tPan Speed:\t\t{0}\r\n", PanSpeed);
            sb.AppendFormat("\tTilt:\t\t{0}\r\n", TiltPosition);
            sb.AppendFormat("\tTilt Speed:\t\t{0}\r\n", TiltSpeed);
            sb.AppendFormat("\tAE:\t\t{0}\r\n", AE);
            sb.AppendFormat("\tAperture:\t\t{0}\r\n", Aperture);
            sb.AppendFormat("\tBackLight:\t\t{0}\r\n", BackLight ? "ON" : "OFF");
            sb.AppendFormat("\tExpComp:\t\t{0}\r\n", ExpComp);
            sb.AppendFormat("\tFocusMode:\t\t{0}\r\n", FocusAuto ? "ON" : "OFF");
            sb.AppendFormat("\tFocus:\t\t{0}\r\n", FocusPosition);
            sb.AppendFormat("\tGain:\t\t{0}\r\n", Gain);
            sb.AppendFormat("\tBGain:\t\t{0}\r\n", BGain);
            sb.AppendFormat("\tRGain:\t\t{0}\r\n", RGain);
            sb.AppendFormat("\tRGain: \t{0}\r\n", Iris);
            sb.AppendFormat("\tMute:\t\t{0}\r\n", Mute ? "ON":"OFF");
            sb.AppendFormat("\tShutter:\t\t{0}\r\n", Shutter);
            sb.AppendFormat("\tTitle:\t\t{0}\r\n", Title ? "ON" : "OFF");
            sb.AppendFormat("\tWB:\t\t{0}\r\n", WB);
            sb.AppendFormat("\tWideDynamic:\t\t{0}\r\n", WideDynamicMode);
            sb.AppendFormat("\tZoom:\t\t{0}\r\n", ZoomPosition);
            sb.AppendFormat("\tZoom Speed:\t\t{0}\r\n", ZoomSpeed);

            return sb.ToString();
        }
    }
}