using NUnit.Framework;
using System.Linq;

namespace Visca.Tests
{
    class UnitTestViscaPacket
    {
        [Test]
        public void ViscaPowerOnTest()
        {
            byte[] power = new ViscaPower(0x01, true);

            Assert.IsTrue(power.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }));
        }

    }
}
