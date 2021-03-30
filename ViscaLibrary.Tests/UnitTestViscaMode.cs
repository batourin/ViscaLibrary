using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace Visca.Tests
{
    [TestFixture]
    class UnitTestViscaMode
    {
        [Test]
        public void ViscaModeCommand()
        {
            OnOffMode mode = OnOffMode.On;
            ViscaModeCommand<OnOffMode> titleCommand = new ViscaModeCommand<OnOffMode>(0x01, new byte[] { Visca.Category.Camera1, Visca.Commands.Title }, "Title", mode);

            byte[] titleCommandBytes = titleCommand;
            Assert.That(titleCommandBytes.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x74, 0x02, 0xff }), Is.True, "Title On bytes sequence does not match expected");
        }

        [Test]
        public void ViscaModeInquiry()
        {
            OnOffMode OnOff = OnOffMode.Off;

            ViscaModeInquiry<OnOffMode> titleInquiry = new ViscaModeInquiry<OnOffMode>(0x01, new byte[] { Visca.Category.Camera1, Visca.Commands.Title }, "Title", new Action<OnOffMode>(mode => {
                Console.WriteLine("Mode: {0}", mode.ToString());
                OnOff = mode;
            }));

            byte[] titleInquiryBytes = titleInquiry;

            Assert.That(titleInquiryBytes.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x74, 0xff }), Is.True, "Title Inquiry bytes sequence does not match expected");

            ViscaRxPacket titleInquiryReply = new ViscaRxPacket(new byte[] { 0x90, 0x50, 0x02, 0xff });

            titleInquiry.Process(titleInquiryReply);


            Assert.That(OnOff == true, Is.True, "Title.Inquiry returned Off while expected On");

        }

    }
}
