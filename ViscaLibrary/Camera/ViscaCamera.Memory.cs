using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Memory Commands Definition

        private readonly ViscaMemorySet _memorySetCmd;
        private readonly Action<ViscaRxPacket, Object> _memorySetCmdReply;
        public event EventHandler<GenericEventArgs<byte>> MemorySetComplete;

        private readonly ViscaMemoryRecall _memoryRecallCmd;
        private readonly Action<ViscaRxPacket, Object> _memoryRecallCmdReply;
        public event EventHandler<GenericEventArgs<byte>> MemoryRecallComplete;

        #endregion Memory Commands Definition

        #region Memory Commands Implementations

        public void MemorySet(byte preset) { _visca.EnqueueCommand(_memorySetCmd.UsePreset(preset), _memorySetCmdReply, preset); }
        public void MemoryRecall(byte preset) { _visca.EnqueueCommand(_memoryRecallCmd.UsePreset(preset), _memoryRecallCmdReply, preset); }

        protected virtual void OnMemorySetComplete(GenericEventArgs<byte> e)
        {
            EventHandler<GenericEventArgs<byte>> handler = MemorySetComplete;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnMemoryRecallComplete(GenericEventArgs<byte> e)
        {
            EventHandler<GenericEventArgs<byte>> handler = MemoryRecallComplete;
            if (handler != null)
                handler(this, e);
        }

        #endregion Memory Commands Implementations
    }
}
