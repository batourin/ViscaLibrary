using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace Visca.Tests
{
    class UnitTestViscaProtocolProcessor
    {
        ViscaProtocolProcessor visca;
        ViscaCamera camera;
        readonly BlockingCollection<byte[]> sendQueue = new BlockingCollection<byte[]>(15);

        [SetUp]
        public void Setup()
        {
            visca = new ViscaProtocolProcessor(
                new Action<byte[]>(b =>
                {
                    sendQueue.Add(b);
                }),
                new Action<byte, string, object[]>((l, f, o) =>
                {
                    Console.WriteLine("LOG:[{0}]", String.Format(f, o));
                })
            );

            camera = new ViscaCamera(ViscaCameraId.Camera1, null, visca);
        }

        [Test]
        public async Task PowerOnOffWithFeedback()
        {
            camera.Power.Should().Be(false);
            camera.Power = true;

            Assert.That(sendQueue.TryTake(out byte[] powerPacket, 100), Is.True, "Timeout on sending data for Power=true");
            Assert.That(powerPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }), Is.True, "Power=true bytes sequence does not match expected");

            // Send ACK
            visca.ProcessIncomingData(new byte[] { 0x90, 0x40 });
            // Send Completion
            visca.ProcessIncomingData(new byte[] { 0xFF, 0x90, 0x50, 0xFF });

            // Next should be Power Inquiry
            Assert.That(sendQueue.TryTake(out byte[] powerInquiry, 100), Is.True, "Timeout on sending data for Power Inquiry");
            Assert.That(powerInquiry.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x00, 0xff }), Is.True, "Power Inquiry bytes sequence does not match expected");

            camera.PowerChanged += (s, e) => { Console.WriteLine("Event: Camera {0} Power: {1}", s.ToString(), e.Power); };

            using (var monitoredSubject = camera.Monitor())
            {
                // Send Power is ON
                visca.ProcessPacket(new byte[] { 0x90, 0x50, 0x02, 0xFF });
                await Task.Delay(100);
                monitoredSubject.Should().Raise("PowerChanged").WithSender(camera).WithArgs<ViscaCamera.PowerEventArgs>(args => args.Power == true);
            }
            camera.Power.Should().Be(true);
        }

    }
}
