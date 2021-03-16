using System;

namespace Visca
{
    public abstract class ViscaMemory : ViscaDynamicCommand
    {
        protected readonly ViscaVariableWithLimits _preset;

        public ViscaMemory(byte address, byte operation, byte preset, IViscaRangeLimits<byte> limits)
        : base(address)
        {
            _preset = new ViscaVariableWithLimits("Preset", preset, limits);

            Append(new byte[]{
                Visca.Category.Camera1,
                Visca.Commands.Memory,
                operation,
            });
            Append(_preset);
        }

        public ViscaMemory UsePreset(byte preset)
        {
            _preset.Value = preset;
            return this;
        }
    }

    public class ViscaMemoryReset : ViscaMemory
    {
        public ViscaMemoryReset(byte address, byte preset)
        : this(address, preset, ViscaDefaults.ViscaDefaultPresetLimits)
        {
        }

        public ViscaMemoryReset(byte address, byte preset, IViscaRangeLimits<byte> limits)
        : base(address, Visca.Commands.MemoryCommands.Reset, preset, limits)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Memory.Reset using {1} preset", this.Destination, _preset.Value);
        }
    }

    public class ViscaMemorySet : ViscaMemory
    {
        public ViscaMemorySet(byte address, byte preset)
        : this(address, preset, ViscaDefaults.ViscaDefaultPresetLimits)
        {
        }

        public ViscaMemorySet(byte address, byte preset, IViscaRangeLimits<byte> limits)
        : base(address, Visca.Commands.MemoryCommands.Set, preset, limits)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Memory.Set using {1} preset", this.Destination, _preset.Value);
        }
    }

    public class ViscaMemoryRecall : ViscaMemory
    {
        public ViscaMemoryRecall(byte address, byte preset)
        : this(address, preset, ViscaDefaults.ViscaDefaultPresetLimits)
        {
        }

        public ViscaMemoryRecall(byte address, byte preset, IViscaRangeLimits<byte> limits)
        : base(address, Visca.Commands.MemoryCommands.Recall, preset, limits)
        {
        }

        public override string ToString()
        {
            return String.Format("Camera{0} Memory.Recall using {1} preset", this.Destination, _preset.Value);
        }
    }

}
