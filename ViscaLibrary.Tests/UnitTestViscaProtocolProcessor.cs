using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace Visca.Tests
{
    [TestFixture]
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
        public void MalformedIncmoingData()
        {
            // Send 0xFF infront of Completion
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
                delegate { visca.ProcessIncomingData(new byte[] { 0xFF, 0x90, 0x50, 0xFF }); });
        }

        [Test]
        public void SplitIncmoingData()
        {
            Assert.DoesNotThrowAsync(async () =>
           {
               // Send partial ACK
               visca.ProcessIncomingData(new byte[] { 0x90, 0x40 });
               // Send rest of ACK and Completion
               visca.ProcessIncomingData(new byte[] { 0xFF, 0x90, 0x50, 0xFF });
               await Task.Delay(100);
           });
        }

        [Test]
        public async Task PowerOnOffWithFeedback()
        {
            camera.Power.Should().Be(false);
            camera.Power = true;

            Assert.That(sendQueue.TryTake(out byte[] powerPacket, 100), Is.True, "Timeout on sending data for Power=true");
            Assert.That(powerPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }), Is.True, "Power=true bytes sequence does not match expected");

            // Send Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            // Next should be Power Inquiry
            Assert.That(sendQueue.TryTake(out byte[] powerInquiry, 100), Is.True, "Timeout on sending data for Power Inquiry");
            Assert.That(powerInquiry.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x00, 0xff }), Is.True, "Power Inquiry bytes sequence does not match expected");

            camera.PowerChanged += (s, e) => { Console.WriteLine("Event: Camera {0} Power: {1}", s.ToString(), e.On); };

            using (var monitoredSubject = camera.Monitor())
            {
                // Send Power is ON
                visca.ProcessPacket(new byte[] { 0x90, 0x50, 0x02, 0xFF });
                await Task.Delay(100);
                monitoredSubject.Should().Raise("PowerChanged").WithSender(camera).WithArgs<ViscaCamera.OnOffEventArgs>(args => args.On == true);
            }
            camera.Power.Should().Be(true);
        }

        [Test]
        public async Task FocusOperationsTests()
        {
            camera.FocusStop();

            Assert.That(sendQueue.TryTake(out byte[] focusStopPacket, 100), Is.True, "Timeout on sending data for Focus Stop command");
            Assert.That(focusStopPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x08, 0x00, 0xff }), Is.True, "Focus Stop bytes sequence does not match expected");

            // Send Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            camera.FocusSpeed = 4;
            camera.FocusFarWithSpeed();
            Assert.That(sendQueue.TryTake(out byte[] focusFarWithSpeedPacket, 100), Is.True, "Timeout on sending data for Focus Far with Speed command");
            Assert.That(focusFarWithSpeedPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x08, 0x24, 0xff }), Is.True, "Focus Far with Speed bytes sequence does not match expected");

            // Send Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            camera.FocusPosition = 0xabcd;
            Assert.That(sendQueue.TryTake(out byte[] focusPositionPacket, 100), Is.True, "Timeout on sending data for Focus Position command");
            Assert.That(focusPositionPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x48, 0x0a, 0x0b, 0x0c, 0x0d, 0xff }), Is.True, "Focus Far with Speed bytes sequence does not match expected");
        }
    }
}
