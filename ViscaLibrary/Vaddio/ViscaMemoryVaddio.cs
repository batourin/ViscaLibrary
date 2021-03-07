using System;

namespace Visca
{
    public class ViscaMemoryVaddioSetWithScene : ViscaMemory
    {
        public ViscaMemoryVaddioSetWithScene(byte address, byte preset)
        : base(address, Visca.Commands.MemoryCommandsVaddio.SetScene, preset, ViscaDefaultsVaddio.PresetLimits)
        { }

        public override string ToString()
        {
            return String.Format("Camera{0} Memory.SetWithScene using {1} preset", this.Destination, _preset.Value);
        }
    }

    public class TriSyncSpeedVariable: ViscaVariableWithLimits
    {
        public TriSyncSpeedVariable()
            : this(ViscaDefaultsVaddio.DefaultTriSyncSpeed)
        { }

        public TriSyncSpeedVariable(byte speed)
            : base("TriSync Speed", speed, ViscaDefaultsVaddio.TriSyncSpeedLimits)
        { }
    }

    public abstract class ViscaMemoryVaddioTriSync : ViscaMemory 
    {

        protected readonly TriSyncSpeedVariable _triSyncSpeed;
        private readonly ViscaVariable _triSyncSpeedByte1;
        private readonly ViscaVariable _triSyncSpeedByte2;

        public ViscaMemoryVaddioTriSync(byte address, byte preset, byte operation)
            : this(address, preset, operation, ViscaDefaultsVaddio.DefaultTriSyncSpeed)
        { }

        public ViscaMemoryVaddioTriSync(byte address, byte preset, byte operation, byte speed)
            : this(address, preset, operation, new TriSyncSpeedVariable(speed))
        { }

        public ViscaMemoryVaddioTriSync(byte address, byte preset, byte operation, TriSyncSpeedVariable speed)
            : base(address, preset, operation, ViscaDefaultsVaddio.PresetLimits)
        {
            _triSyncSpeed = speed;
            _triSyncSpeed.VariableChanged += (s, e) => 
                {
                    _triSyncSpeedByte1.Value = (byte)(e.Value & 0xf0);
                    _triSyncSpeedByte1.Value = (byte)(e.Value & 0x0f);
                };

            _triSyncSpeedByte1 = new ViscaVariable("TriSyncSpeedByte1", (byte)(_triSyncSpeed.Value & 0xf0));
            _triSyncSpeedByte2 = new ViscaVariable("TriSyncSpeedByte2", (byte)(_triSyncSpeed.Value & 0x0f));

            Append(operation);
            Append(_triSyncSpeedByte1);
            Append(_triSyncSpeedByte2);
        }
    }

    public class ViscaMemoryVaddioSetTriSync : ViscaMemoryVaddioTriSync
    {
        public ViscaMemoryVaddioSetTriSync(byte address, byte preset)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSync)
        { }

        public ViscaMemoryVaddioSetTriSync(byte address, byte preset, byte speed)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSync, speed)
        { }
        public ViscaMemoryVaddioSetTriSync(byte address, byte preset, TriSyncSpeedVariable speed)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSync, speed)
        { }

        public override string ToString()
        {
            return String.Format("Camera{0} Memory.SetTriSync using {1} preset", this.Destination, _preset.Value);
        }
    }

    public class ViscaMemoryVaddioSetTriSyncWithScene : ViscaMemoryVaddioTriSync
    {
        public ViscaMemoryVaddioSetTriSyncWithScene(byte address, byte preset)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSyncWithScene)
        { }

        public ViscaMemoryVaddioSetTriSyncWithScene(byte address, byte preset, byte speed)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSyncWithScene, speed)
        { }
        public ViscaMemoryVaddioSetTriSyncWithScene(byte address, byte preset, TriSyncSpeedVariable speed)
            : base(address, preset, Visca.Commands.MemoryCommandsVaddio.SetTriSyncWithScene, speed)
        { }
        public override string ToString()
        {
            return String.Format("Camera{0} Memory.SetTriSyncWithSpeed using {1} preset", this.Destination, _preset.Value);
        }
    }

}
