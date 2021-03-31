using System;

namespace Visca
{
    public partial class ViscaCamera
    {
        #region Memory Commands Definition

        private readonly ViscaMemorySet _memorySetCmd;
        public event EventHandler<GenericEventArgs<byte>> MemorySetComplete;

        private readonly ViscaMemoryRecall _memoryRecallCmd;
        public event EventHandler<GenericEventArgs<byte>> MemoryRecallComplete;

        #endregion Memory Commands Definition

        #region Memory Commands Implementations

        public void MemorySet(byte preset) { _visca.EnqueueCommand(_memorySetCmd.UsePreset(preset).OnCompletion(() => { OnMemorySetComplete(new GenericEventArgs<byte>(preset)); })); }
        public void MemoryRecall(byte preset) { _visca.EnqueueCommand(_memoryRecallCmd.UsePreset(preset).OnCompletion(() => { OnMemoryRecallComplete(new GenericEventArgs<byte>(preset)); })); }

        protected virtual void OnMemorySetComplete(GenericEventArgs<byte> e)
        {
            EventHandler<GenericEventArgs<byte>> handler = MemorySetComplete;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        protected virtual void OnMemoryRecallComplete(GenericEventArgs<byte> e)
        {
            EventHandler<GenericEventArgs<byte>> handler = MemoryRecallComplete;
#if SSHARP
            if (handler != null)
                handler(this, e);
#else
            handler?.Invoke(this, e);
#endif
        }

        #endregion Memory Commands Implementations
    }
}
