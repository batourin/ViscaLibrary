using NUnit.Framework;
using System.Linq;

namespace Visca.Tests
{
    [TestFixture]
    public class UintTestPowerCommands
    {
        [SetUp]
        public void Setup()
        {
            //ViscaProtocolProcessor visca = new ViscaProtocolProcessor(null, null);
        }

        [Test]
        public void PowerOnBytesMatch()
        {
            byte[] power = new ViscaPower(0x01, true);

            //Assert.IsTrue(power.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }));
            Assert.That(power.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }), Is.True, "Power.On bytes sequence does not match expected");
        }

        [Test]
        public void PowerOffBytesMatch()
        {
            byte[] power = new ViscaPower(0x01, false);

            Assert.That(power.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x03, 0xff }), Is.True, "Power.Off bytes sequence does not match expected");
        }
    }

}