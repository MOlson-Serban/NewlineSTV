using Crestron.SimplSharp;
using System;
using System.Collections.Generic;

namespace NewLineSTV
{
    public class DisplayController
    {
        private const int MAX_BUFFER_SIZE = 2048;
        private const int CMD_TIMEOUT_MS = 2000;

        private CTimer _queueTimer;
        private List<byte> _rxBuffer = new List<byte>();

        private readonly object _bufferLock = new object();
        private readonly object _queueLock = new object();
        private readonly object _timerLock = new object();

        private class QueuedCmd
        {
            public byte[] CommandBytes;
            public int CmdType;
            public ushort PendingValue;
            public bool RequiresAck;
        }

        private Queue<QueuedCmd> _cmdQueue = new Queue<QueuedCmd>();
        private QueuedCmd _activeCmd = null;
        private bool _isWaitingForReply = false;

        public delegate void StateChangeHandler(ushort state);
        public delegate void SerialDataHandler(SimplSharpString data);

        public StateChangeHandler OnPowerChange { get; set; }
        public StateChangeHandler OnInputChange { get; set; }
        public StateChangeHandler OnVolumeChange { get; set; }
        public StateChangeHandler OnMuteChange { get; set; }
        public SerialDataHandler TransmitSerialData { get; set; }

        // =========================================================================
        // QUEUE & TRANSMISSION
        // =========================================================================
        private void EnqueueCommand(byte[] command, int type, ushort val, bool requiresAck = true)
        {
            lock (_queueLock)
            {
                _cmdQueue.Enqueue(new QueuedCmd { CommandBytes = command, CmdType = type, PendingValue = val, RequiresAck = requiresAck });
            }
            ProcessQueue();
        }

        private void ProcessQueue()
        {
            lock (_queueLock)
            {
                if (_isWaitingForReply || _cmdQueue.Count == 0) return;

                _activeCmd = _cmdQueue.Dequeue();
                _isWaitingForReply = true;

                try
                {
                    string hexStr = BitConverter.ToString(_activeCmd.CommandBytes).Replace("-", "");
                    TransmitSerialData?.Invoke(new SimplSharpString(hexStr));

                    // Fire and Forget for Volume/Mute to prevent UI lag
                    if (!_activeCmd.RequiresAck)
                    {
                        ClearActiveCommand();
                        ProcessQueue();
                        return;
                    }

                    lock (_timerLock)
                    {
                        if (_queueTimer != null) { _queueTimer.Stop(); _queueTimer.Dispose(); }
                        _queueTimer = new CTimer(QueueTimeoutCallback, null, CMD_TIMEOUT_MS);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Error("NewlineSTV RS232 Send Error: " + ex.Message);
                    ClearActiveCommand();
                }
            }
        }

        private void QueueTimeoutCallback(object specific)
        {
            lock (_queueLock) { ClearActiveCommand(); }
            ProcessQueue();
        }

        private void ClearActiveCommand()
        {
            _isWaitingForReply = false;
            _activeCmd = null;
            lock (_timerLock)
            {
                if (_queueTimer != null) { _queueTimer.Stop(); _queueTimer.Dispose(); _queueTimer = null; }
            }
        }

        // =========================================================================
        // RECEPTION & PARSING
        // =========================================================================
        public void ReceiveHexData(SimplSharpString rxHexData)
        {
            lock (_bufferLock)
            {
                string hexStr = rxHexData.ToString();
                for (int i = 0; i < hexStr.Length; i += 2)
                {
                    if (i + 1 < hexStr.Length)
                    {
                        _rxBuffer.Add(Convert.ToByte(hexStr.Substring(i, 2), 16));
                    }
                }

                if (_rxBuffer.Count > MAX_BUFFER_SIZE) { _rxBuffer.Clear(); }
                ParseBuffer();
            }
        }

        private void ParseBuffer()
        {
            while (_rxBuffer.Count >= 11)
            {
                int startIdx = _rxBuffer.IndexOf(0x7F);

                if (startIdx < 0) { _rxBuffer.Clear(); return; }
                if (startIdx > 0) { _rxBuffer.RemoveRange(0, startIdx); continue; }

                // Full Header Validation
                if (_rxBuffer.Count >= 8 &&
                    (_rxBuffer[2] != 0x99 || _rxBuffer[3] != 0xA2 ||
                     _rxBuffer[4] != 0xB3 || _rxBuffer[5] != 0xC4 ||
                     _rxBuffer[6] != 0x02 || _rxBuffer[7] != 0xFF))
                {
                    _rxBuffer.RemoveAt(0);
                    continue;
                }

                int payloadLength = _rxBuffer[1];

                // Payload Length Sanity Check (prevents noise lockups)
                if (payloadLength < 4 || payloadLength > 32)
                {
                    _rxBuffer.RemoveAt(0);
                    continue;
                }

                int expectedTotalLength = payloadLength + 3;

                if (_rxBuffer.Count < expectedTotalLength) return;

                if (_rxBuffer[expectedTotalLength - 1] == 0xCF)
                {
                    byte[] msg = new byte[expectedTotalLength];
                    _rxBuffer.CopyTo(0, msg, 0, expectedTotalLength);
                    _rxBuffer.RemoveRange(0, expectedTotalLength);
                    ProcessMessage(msg);
                }
                else
                {
                    _rxBuffer.RemoveAt(0);
                }
            }
        }

        private void ProcessMessage(byte[] msg)
        {
            Action safeInvoke = null;
            bool activeCmdHandled = false;

            if (msg.Length >= 11)
            {
                byte cmdCategory = msg[8];
                byte cmdByte = msg[9];

                bool waitingForPowerAck = (_activeCmd != null && _activeCmd.CmdType == 1 && cmdCategory == 0x01);
                bool waitingForInputAck = (_activeCmd != null && _activeCmd.CmdType == 2 && cmdCategory == 0x01);
                bool waitingForVolumeAck = (_activeCmd != null && _activeCmd.CmdType == 3 && cmdCategory == 0x05);
                bool waitingForMuteAck = (_activeCmd != null && _activeCmd.CmdType == 4 && (cmdCategory == 0x0F || cmdCategory == 0x01));

                // --- PROCESS POWER ---
                if (cmdCategory == 0x01 && cmdByte == 0x37 && msg.Length >= 12)
                {
                    byte val = msg[10];
                    safeInvoke += () => OnPowerChange?.Invoke(val);
                    if (_activeCmd != null && _activeCmd.CmdType == 10) activeCmdHandled = true;
                }
                else if (waitingForPowerAck && msg.Length >= 12 && msg[10] == 0x01)
                {
                    activeCmdHandled = true;
                    ushort fbVal = _activeCmd.PendingValue;
                    safeInvoke += () => OnPowerChange?.Invoke(fbVal);
                }

                // --- PROCESS INPUT ---
                else if (cmdCategory == 0x01 && cmdByte == 0x50 && msg.Length >= 12)
                {
                    byte val = msg[10];
                    ushort inVal = 0;
                    if (val == 0x19) inVal = 1;
                    else if (val == 0x1F) inVal = 2;
                    else if (val == 0x1E) inVal = 3;

                    if (inVal > 0)
                    {
                        safeInvoke += () => OnInputChange?.Invoke(inVal);
                    }
                    if (_activeCmd != null && _activeCmd.CmdType == 11) activeCmdHandled = true;
                }
                else if (waitingForInputAck && msg.Length >= 12 && msg[10] == 0x01)
                {
                    activeCmdHandled = true;
                    ushort fbVal = _activeCmd.PendingValue;
                    safeInvoke += () => OnInputChange?.Invoke(fbVal);
                }

                // --- PROCESS VOLUME ---
                else if ((cmdCategory == 0x01 && cmdByte == 0x33 && msg.Length >= 12) ||
                         (cmdCategory == 0x05 && msg.Length >= 12 && msg[10] == 0x01))
                {
                    byte val = (cmdCategory == 0x05) ? msg[9] : msg[10];
                    safeInvoke += () => OnVolumeChange?.Invoke(val);

                    if (_activeCmd != null && (_activeCmd.CmdType == 12 || _activeCmd.CmdType == 3)) activeCmdHandled = true;
                }

                // --- PROCESS MUTE ---
                else if (cmdCategory == 0x01 && cmdByte == 0x82 && msg.Length >= 12)
                {
                    // muted = 0x01, unmuted = 0x00
                    ushort isMuted = (msg[10] == 0x01) ? (ushort)1 : (ushort)0;
                    safeInvoke += () => OnMuteChange?.Invoke(isMuted);

                    if (_activeCmd != null && (_activeCmd.CmdType == 13 || _activeCmd.CmdType == 4))
                        activeCmdHandled = true;
                }
                else if (waitingForMuteAck && msg.Length >= 12)
                {
                    activeCmdHandled = true;

                    if (_activeCmd != null && _activeCmd.CmdType == 4)
                    {
                        ushort isMuted = (_activeCmd.CommandBytes[9] == 0x01) ? (ushort)1 : (ushort)0;
                        safeInvoke += () => OnMuteChange?.Invoke(isMuted);
                    }
                }

                // --- CONSTRAINED FALLBACK ACK ---
                else if (_activeCmd != null && !activeCmdHandled)
                {
                    if ((_activeCmd.CmdType == 1 && cmdCategory == 0x01) ||
                        (_activeCmd.CmdType == 2 && cmdCategory == 0x01) ||
                        (_activeCmd.CmdType == 3 && cmdCategory == 0x05) ||
                        (_activeCmd.CmdType == 4 && (cmdCategory == 0x0F || cmdCategory == 0x01)))
                    {
                        activeCmdHandled = true;
                    }
                }
            }

            if (activeCmdHandled)
            {
                lock (_queueLock) { ClearActiveCommand(); }
            }

            // Safe delegate invocation protects the C# thread from SIMPL+ crashes
            try { safeInvoke?.Invoke(); }
            catch (Exception ex) { ErrorLog.Error("NewlineSTV SIMPL+ Callback Error: " + ex.Message); }

            ProcessQueue();
        }

        // =========================================================================
        // PUBLIC CONTROL METHODS
        // =========================================================================
        public void PollPower() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x37, 0xCF }, 10, 0, true); }
        public void PollInput() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x50, 0xCF }, 11, 0, true); }
        public void PollVolume() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x33, 0xCF }, 12, 0, true); }

        // NEW: Added explicit Poll Mute method
        public void PollMute() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x82, 0xCF }, 13, 0, true); }

        public void PowerOn() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x00, 0xCF }, 1, 1, true); }
        public void PowerOff() { EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x01, 0xCF }, 1, 0, true); }

        public void SetInput(ushort inputVal)
        {
            byte inputByte = 0x0A;
            if (inputVal == 2) inputByte = 0x52;
            else if (inputVal == 3) inputByte = 0x53;
            EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, inputByte, 0xCF }, 2, inputVal, true);
        }

        public void SetVolume(ushort volVal)
        {
            byte v = (byte)(volVal > 100 ? 100 : volVal);
            EnqueueCommand(new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x05, v, 0xCF }, 3, volVal, false);

            // Optimistic Feedback
            OnVolumeChange?.Invoke(v);
        }

        public void SetMute(ushort muteState)
        {
            // muted = 0x01, unmuted = 0x00
            byte m = muteState > 0 ? (byte)0x01 : (byte)0x00;

            EnqueueCommand(
                new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x0F, m, 0xCF }, 4, muteState, false );

            // Optimistic feedback
            ushort isMuted = (m == 0x01) ? (ushort)1 : (ushort)0;
            OnMuteChange?.Invoke(isMuted);

            // Reconcile with actual device state
            PollMute();
        }

        public void ToggleMute()
        {
            EnqueueCommand(
                new byte[] { 0x7F, 0x08, 0x99, 0xA2, 0xB3, 0xC4, 0x02, 0xFF, 0x01, 0x02, 0xCF }, 4, 0, false );

            PollMute();
        }
    }
}