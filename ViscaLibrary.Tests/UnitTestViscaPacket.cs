using NUnit.Framework;
using System.Linq;

namespace Visca.Tests
{
    [TestFixture]
    class UnitTestViscaPacket
    {
        [Test]
        public void ViscaPacketComparsions()
        {
            ViscaPower power1 = new ViscaPower(0x01, true);
            ViscaPower power2 = new ViscaPower(0x01, true);


            Assert.That(power1.Equals(null), Is.False, "ViscaTxPacket.Equals returned true on null");
            Assert.That(power1.Equals(power2), Is.True, "ViscaTxPacket.Equals returned not true on same commands");

            Assert.That(power1 != null, Is.True, "Command is not null");
            Assert.That(power1 == null, Is.False, "Command is not null");
            Assert.That(null == power1, Is.False, "Command is not null");
            Assert.That(power1 == power2, Is.True, "Commands are not equal");

        }

    }
}
