using System;

namespace Visca
{
    public class ViscaVariable
    {
        public readonly string Name;

        private byte _value;

        public ViscaVariable(string name, byte value)
        {
            Name = name;
            _value = value;
        }

        public byte Value
        {
            get{ return _value;}
            set
            {
                if(_value != value && checkRange(value))
                {
                    _value = value;
                    OnVariableChanged(new VariableEventArgs(value));
                }
            }
        }

        public class VariableEventArgs: EventArgs
        {
            public byte Value;
            public VariableEventArgs(byte value) : base() { this.Value = value; }
        }
        public event EventHandler<VariableEventArgs> VariableChanged;

        protected virtual void OnVariableChanged(VariableEventArgs e)
        {
            EventHandler<VariableEventArgs> handler = VariableChanged;
            if(handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Base implementation to check value range.
        /// To be overriden in ancestor classes for specific ranges
        /// </summary>
        /// <returns>true if in the range </returns>
        protected virtual bool checkRange(byte value)
        {
            return true;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Name, _value);
        }
    }

    public interface IViscaRangeLimits<T>
    {
        T Low { get; }
        T High { get; }
        string Message { get; }
    }

    public class ViscaRangeLimits<T> : IViscaRangeLimits<T>
    {
        public T Low { get; private set; }
        public T High { get; private set; }
        public string Message { get; private set; }

        public ViscaRangeLimits(T low, T high, string message)
        {
            Low = low;
            High = high;
            Message = message;
        }
    }

    public class ViscaVariableWithLimits: ViscaVariable
    {
        private IViscaRangeLimits<byte> _limits;
    
        public ViscaVariableWithLimits(string name, byte value, IViscaRangeLimits<byte> limits)
            :base(name, value)
        {
            _limits = limits;
            checkRange(value);
        }
        
        /// <summary>
        /// Checking range of supplied value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>true if in limits or exception</returns>
        protected override bool checkRange(byte value)
        {
            if(value < _limits.Low || value > _limits.High)
            {
                throw new ArgumentOutOfRangeException(Name, String.Format("{0} should be in range of 0x{1:X2} to 0x{2:X2}}.", Name, _limits.Low, _limits.High));
            }
            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} with range [0x{1:X2} - 0x{2:X2}]", base.ToString(), _limits.Low, _limits.High);
        }
    }
}