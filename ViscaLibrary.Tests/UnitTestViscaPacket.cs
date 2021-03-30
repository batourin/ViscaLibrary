using NUnit.Framework;

namespace Visca.Tests
{
    [TestFixture]
    class UnitTestViscaPacket
    {
        [Test]
        public void ViscaPacketComparsions()
        {
            ViscaPower power1 = new ViscaPower(0x01, OnOffMode.On);
            ViscaPower power2 = new ViscaPower(0x01, OnOffMode.On);

            Assert.That(power1.Equals(null), Is.False, "ViscaTxPacket.Equals returned true on null");
            Assert.That(power1.Equals(power2), Is.True, "ViscaTxPacket.Equals returned not true on same commands");

            Assert.That(power1 != null, Is.True, "Command is not null");
            Assert.That(power1 == null, Is.False, "Command is not null");
            Assert.That(null == power1, Is.False, "Command is not null");
            Assert.That(power1 == power2, Is.True, "Commands are not equal");

        }

        [Test]
        public void ViscaDynamicCloneCommandComparsions()
        {
            ViscaMemoryRecall recallCmd = new ViscaMemoryRecall(0x01, 0x01);
            recallCmd.UsePreset(0x02);

            ViscaCommand recallCloneCmd = recallCmd.Clone();
            Assert.That(recallCloneCmd != null, Is.True, "Clonned Command != null");
            Assert.That(recallCloneCmd == null, Is.False, "Clonned Command == null");
            Assert.That(null == recallCloneCmd, Is.False, "null == Clonned Command");

            recallCmd.UsePreset(0x03);
            Assert.That(recallCmd == recallCloneCmd, Is.False, "recallCmd with Preset 3 and Clonned Command with Preset 2 are equal");

            recallCmd.UsePreset(0x02);
            Assert.That(recallCmd == recallCloneCmd, Is.True, "recallCmd with Preset 2 and Clonned Command with Preset 2 are not equal");
        }
    }
}
