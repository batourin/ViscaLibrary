namespace Visca
{
    public static partial class Visca
    {
        public static partial class Commands
        {

            public static class WBModePoly
            {
                public const byte Table = 0x06;
            }

        }
    }

    public class PolyWBMode : WBMode
    {
        public static readonly PolyWBMode Table = new PolyWBMode(Visca.Commands.WBModePoly.Table, "Table");

        public PolyWBMode(byte key, string value) : base(key, value)
        { }
    }

}
