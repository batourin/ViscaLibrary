using System;
using System.Collections.Generic;

#if SSHARP
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using Crestron.SimplSharpPro.CrestronThread;
#else
using System.Collections.Concurrent;
using System.Threading;
#endif

namespace Visca
{

#if SSHARP
    public static class EnumExtensions
    {
        public static IEnumerable<Enum> GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType parameter is not System.Enum");

            List<Enum> enumerations = new List<Enum>();
            foreach (FieldInfo fieldInfo in enumType.GetCType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add((Enum)fieldInfo.GetValue(null));
            }
            return enumerations;
        }
    }
#else
    public static class QueueExtensions
    {
        public static void Enqueue<T>(this BlockingCollection<T> queue, T item)
        {
            queue.Add(item);
        }

        public static T Dequeue<T>(this BlockingCollection<T> queue)
        {
            return queue.Take();
        }
    }
#endif

    public class ViscaProtocolProcessor
    {
        private readonly Dictionary<ViscaCameraId, ViscaCamera> _cameras = new Dictionary<ViscaCameraId, ViscaCamera>(7);

        private readonly byte[] _incomingBuffer = new byte[1024];
        private int _incomingBufferLength = 0;

#if SSHARP
        private readonly CrestronQueue<ViscaTxPacket> _sendQueue = new CrestronQueue<ViscaTxPacket>(30);
        private readonly CrestronQueue<ViscaRxPacket> _responseQueue = new CrestronQueue<ViscaRxPacket>(15);
        private CTimer _sendQueueItemInProgressTimer;
#else
        private readonly BlockingCollection<ViscaTxPacket> _sendQueue = new BlockingCollection<ViscaTxPacket>(30);
        private readonly BlockingCollection<ViscaRxPacket> _responseQueue = new BlockingCollection<ViscaRxPacket>(15);
        private readonly CancellationTokenSource _responseQueueCancel = new CancellationTokenSource();
        private readonly Timer _sendQueueItemInProgressTimer;
#endif
        /// <summary>
        /// Holds currently submited command
        /// </summary>
        private ViscaTxPacket _sendQueueCommandInProgress;
        /// <summary>
        /// Holds socket number for command sent to camera
        /// </summary>
        private byte? _socketInProgress;
        /// <summary>
        /// Thread to process responses from camera
        /// </summary>
        private readonly Thread _responseParseThread;

        private readonly Action<byte, string, object[]> _logAction; 

		/// <summary>
		/// Action to send data to consumer
		/// </summary>
		/// <param>byte array to send</param>
        private readonly Action<byte[]> _sendData;

        public ViscaProtocolProcessor(Action<byte[]> sendData, Action<byte, string, object[]> logAction)
        {
            _sendData = sendData;
            _logAction = logAction;

            Cameras = new CamerasIndexer(getCameraAction);
#if SSHARP
            _sendQueueItemInProgressTimer = new CTimer((o) => 
#else
            _sendQueueItemInProgressTimer = new Timer((o) =>
#endif
                {
                    _sendQueueCommandInProgress = null;
                    logMessage(1, "Command timeout"); 
                    sendNextQueuedCommand();
                }, null, Timeout.Infinite, Timeout.Infinite);

#if SSHARP
            _responseParseThread = new Thread(parseResponse, null, Thread.eThreadStartOptions.Running);
#else
            _responseParseThread = new Thread(parseResponse);
            _responseParseThread.Start();
#endif
        }

#if !SSHARP
        ~ViscaProtocolProcessor()
        {
            _responseQueue.CompleteAdding();
            _responseQueueCancel.Cancel(false);
        }
#endif

        public void EnqueueCommand(ViscaTxPacket command)
        {
            // check for existing command in the Queue
            bool commandIsEnqueued = false;
            foreach (var sendQueueItem in _sendQueue)
            {
                //if (sendQueueItem.Packet == command)
                if (sendQueueItem == command)
                {
                    commandIsEnqueued = true;
                    break;
                }
            }

            if (commandIsEnqueued)
            {
                logMessage(1, "Enqueueing command '{0}' is duplicate, skipping. CommandQueue Size: '{1}'", command.ToString(), _sendQueue.Count);
                logMessage(2, "CommandQueue:");
                foreach(var sendQueueItem in _sendQueue)
                    logMessage(2, "\t'{0}'", sendQueueItem.ToString());
            }
            else
            {
                // If command is ViscaDynamicCommand, clone it to get static version for enquing
                if (command is ViscaDynamicCommand)
                    command = (command as ViscaDynamicCommand).Clone();
                _sendQueue.Enqueue(command);
                logMessage(1, "Enqueued command '{0}'. CommandQueue Size: '{1}'", command.ToString(), _sendQueue.Count);
            }

            if (_sendQueueCommandInProgress == null && (_responseQueue.Count == 0))
                sendNextQueuedCommand();
        }

        public void InitializeChain()
        {
            logMessage(1, "Initializing Chain");
            _sendData(new byte[] { (byte)0x88, (byte)0x30, 0x01, 0xFF });
            // Send broadcast clear
            _sendData(new ViscaClear());
        }

        public void CancelCommand()
        {
            if(_socketInProgress.HasValue && _sendQueueCommandInProgress != null && _sendQueueCommandInProgress.IsCommand)
            {
                logMessage(1, "Command '{0}' canceling on socket '{1}'", _sendQueueCommandInProgress.ToString(), _socketInProgress.Value);
                _sendData(new byte[] { (byte)(0x80 + _sendQueueCommandInProgress.Destination), (byte)(0x20 + _socketInProgress.Value), 0xFF});
            }
        }

        /// <summary>
        /// Sends the next queued command to the device
        /// </summary>
        private void sendNextQueuedCommand()
        {
            if (_sendQueue.Count > 0)
            {
                _sendQueueCommandInProgress = _sendQueue.Dequeue();
                logMessage(1, "Command '{0}' Dequeued. CommandQueue Size: {1}", _sendQueueCommandInProgress.ToString(), _sendQueue.Count);
                // start the timer to expire current command in case of no response
#if SSHARP
                _sendQueueItemInProgressTimer.Reset(2000);
#else
                _sendQueueItemInProgressTimer.Change(2000, Timeout.Infinite);
#endif
                _sendData(_sendQueueCommandInProgress);
            }
        }

#if SSHARP
        private object parseResponse(object obj)
#else
        private void parseResponse(object obj)
#endif
        {
            while (true)
            {
                try
                {
                    ViscaRxPacket rxPacket = _responseQueue.Dequeue();
                    if (rxPacket == null)
                    {
                        logMessage(2, "Exception in parseResponse thread, deque byte array is null");
                        break;
                    }

                    //if (Debug.Level == 2) // This check is here to prevent following string format from building unnecessarily on level 0 or 1
                        logMessage(2, "Response '{0}' Dequeued. ResponseQueue Size: {1}",
                            rxPacket.ToString(),
                            //String.Concat(message.Select(b => string.Format(@"[{0:X2}]", (int)b)).ToArray()),
                            _responseQueue.Count);

                    if (rxPacket.IsNetworkChange)
                    {
                        // TODO: rxPacket.IsNetworkChange not sure what shall we doo
                        continue;
                    } else if (rxPacket.IsClearBroadcast)
                    {
                        // TODO: rxPacket.IsClearBroadcast not sure what shall we doo
                        continue;
                    }


                    if (_sendQueueCommandInProgress == null)
                    {
                        /// response is not associated with any particular command
                        logMessage(2, "Collision, response for command not in progress");
                    }
                    else
                    {
#if SSHARP
                        _sendQueueItemInProgressTimer.Stop();
#else
                        _sendQueueItemInProgressTimer.Change(Timeout.Infinite, Timeout.Infinite);
#endif
                        if (rxPacket.IsAck)
                        {
                            _socketInProgress = rxPacket.Socket;
                            logMessage(1, "Command '{0}' accepted on socket '{1}'", _sendQueueCommandInProgress.ToString(), _socketInProgress.Value);
                            continue;
                        } // rxPacket.IsAck
                        else  if (rxPacket.IsCompletionCommand)
                        {
                            if (!_sendQueueCommandInProgress.IsCommand)
                                logMessage(2, "Collision, completion message is not for Command type message");
                            ViscaCommand command = _sendQueueCommandInProgress as ViscaCommand;
                            if (command != null && command.CompletionAction != null)
                                command.CompletionAction();
                        } // rxPacket.IsCompletionCommand
                        else if (rxPacket.IsCompletionInquiry)
                        {
                            // we have pending clearance command in progress, use it's processing hook
                            var query = _sendQueueCommandInProgress as ViscaInquiry;
                            if(query != null)
                                query.Process(rxPacket);
                            else
                                logMessage(2, "Collision, expecting ViscaInquiry type as command in progress");
                        } // rxPacket.IsCompletionInquiry
                        else if (rxPacket.IsError)
                        {
                            // Error message
                            switch (rxPacket.Error)
                            {
                                case ViscaError.Length:
                                    // Message Length Error
                                    logMessage(2, "Error from device: Message Length Error");
                                    break;
                                case ViscaError.Syntax:
                                    // Syntax Error
                                    logMessage(2, "Error from device: Syntax Error");
                                    break;
                                case ViscaError.Full:
                                    // Command Buffer Full
                                    logMessage(2, "Error from device: Command Buffer Full");
                                    break;
                                case ViscaError.Canceled:
                                    // Command Cancelled
                                    logMessage(2, "Error from device: Command Cancelled");
                                    break;
                                case ViscaError.NoSocket:
                                    // No Socket
                                    logMessage(2, "Error from device: No Socket");
                                    break;
                                case ViscaError.NotExecutable:
                                    // Command not executable
                                    logMessage(2, "Error from device: Command not executable");
                                    break;
                            }
#if SSHARP
                            if (_sendQueueCommandInProgress.ErrorAction != null)
                                _sendQueueCommandInProgress.ErrorAction(rxPacket.Error);
#else
                            _sendQueueCommandInProgress.ErrorAction?.Invoke(rxPacket.Error);
#endif
                        } // rxPacket.IsError
                        else
                        {
                            logMessage(1, "Error: unknown packet type");
                        }

                        logMessage(2, "Completing command in progress: '{0}'", _sendQueueCommandInProgress.ToString());
                        _sendQueueCommandInProgress = null;
                        _socketInProgress = null;
                    }
                }
#if SSHARP
#else
                catch (OperationCanceledException)
                {
                    logMessage(2, "Visca Response Queue shutdown");
                    break;
                }
#endif
                catch (Exception e)
                {
                    logMessage(2, "Exception in parseResponse thread: '{0}'\n{1}", e.Message, e.StackTrace);
                }

                if ((_sendQueue.Count > 0) && (_responseQueue.Count == 0))
                    sendNextQueuedCommand();
            } // while(true)
#if SSHARP
            return null;
#else
            return;
#endif
        }

#region Cameras

        public bool Attach(ViscaCameraId id, ViscaCamera camera)
        {
            if(_cameras.ContainsKey(id))
                return false;
            _cameras.Add(id, camera);
            return true;
        }

        public class CamerasIndexer
        {
            private readonly Func<ViscaCameraId, ViscaCamera> _getCameraAction;
            public CamerasIndexer(Func<ViscaCameraId, ViscaCamera> getCameraAction)
            {
                _getCameraAction = getCameraAction;
            }
            public ViscaCamera this[ViscaCameraId cameraId]
            {
                get { return _getCameraAction(cameraId); }
            }
        }

        /// <summary>
        /// Process Visca byte stream
        /// </summary>
        /// <remark>
        /// process byte stream as it comes from source, splits into individual packets
        /// </remark>
        /// <prarm name="data">bytes recived from visca interface</param>
        private ViscaCamera getCameraAction(ViscaCameraId cameraId)
        {
            return _cameras[cameraId];
        }

        public readonly CamerasIndexer Cameras;

#endregion Cameras

#region IO processing

        /// <summary>
        /// Process Visca byte stream
        /// </summary>
        /// <remark>
        /// incoming data will be split into separate messages, rest of data saved in bufeer
        /// buffer being cyclic
        /// </remark>
        /// <prarm name="data">raw data recived from visca camera</param>
        public void ProcessIncomingData(byte[] data)
        {
            if(data == null)
#if SSHARP
                throw new ArgumentNullException("data", "Supplied data is not in visca packet format");
#else
                throw new ArgumentNullException(nameof(data), "Supplied data is not in visca packet format");
#endif
            if (data.Length == 0)
#if SSHARP
                throw new ArgumentOutOfRangeException("data", "Supplied data length can't be zero");
#else
                throw new ArgumentOutOfRangeException(nameof(data), "Supplied data length can't be zero");
#endif

            if ((_incomingBufferLength + data.Length) > _incomingBuffer.Length)
#if SSHARP
                throw new ArgumentOutOfRangeException("data", "Out of incoming buffer: Incoming data can't be added to current buffer");
#else
                throw new ArgumentOutOfRangeException(nameof(data), "Out of incoming buffer: Incoming data can't be added to current buffer");
#endif

            data.CopyTo(_incomingBuffer, _incomingBufferLength);
            _incomingBufferLength += data.Length;

            // Search for the delimiter 0xFF character
            int idxStart = 0;
            for (int i = 0; i < _incomingBufferLength; i++)
            {
                if (_incomingBuffer[i] == 0xFF)
                {
                    byte[] message = new byte[i + 1 - idxStart];
                    Array.Copy(_incomingBuffer, idxStart, message, 0, i + 1 - idxStart);
                    ProcessPacket(message);
                    // move start position to next element
                    idxStart = i + 1;
                }
            }
            // Skip over delimmiter and save the rest for next time
            Array.Copy(_incomingBuffer, idxStart, _incomingBuffer, 0, _incomingBufferLength - idxStart);
            _incomingBufferLength -=  idxStart;
        }

        /// <summary>
        /// Process Visca message
        /// </summary>
        /// <remark>
        /// message should be single Visca packet with address and terminator
        /// </remark>
        /// <prarm name="data">Single Visca packet</param>
        public void ProcessPacket(byte[] data)
        {
            if(data == null)
#if SSHARP
                throw new ArgumentNullException("data", "Supplied data is not in visca packet format");
#else
                throw new ArgumentNullException(nameof(data), "Supplied data is not in visca packet format");
#endif
            if ((data.Length == 0 ) || (data[data.Length-1] != Visca.Terminator))
#if SSHARP
                throw new ArgumentException("Supplied data is not in visca packet format", "data");
#else
                throw new ArgumentException("Supplied data is not in visca packet format", nameof(data));
#endif
            _responseQueue.Enqueue(new ViscaRxPacket(data));
        }

#endregion IO processing

        /// <summary>
        /// Sends log data to associated log action with log level
        /// </summary>
        private void logMessage(byte level, string format, params object[] args)
        {
#if SSHARP
            if(_logAction != null)
                _logAction(level, format, args);
#else
            _logAction?.Invoke(level, format, args);
#endif
        }
    }
}