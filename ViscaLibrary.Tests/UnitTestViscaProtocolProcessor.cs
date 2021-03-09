using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace Visca.Tests
{
    [TestFixture]
    class UnitTestViscaProtocolProcessor
    {
        ViscaProtocolProcessor visca;
        readonly BlockingCollection<byte[]> sendQueue = new BlockingCollection<byte[]>(15);
        readonly byte id = 1;

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

            //camera = new ViscaCamera(ViscaCameraId.Camera1, null, visca);
        }

        [Test]
        public void MalformedIncmoingData()
        {
            // Respond 0xFF infront of Completion
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
                delegate { visca.ProcessIncomingData(new byte[] { 0xFF, 0x90, 0x50, 0xFF }); });
        }

        [Test]
        public void SplitIncmoingData()
        {
            Assert.DoesNotThrowAsync(async () =>
           {
               // Respond partial ACK
               visca.ProcessIncomingData(new byte[] { 0x90, 0x40 });
               // Respond rest of ACK and Completion
               visca.ProcessIncomingData(new byte[] { 0xFF, 0x90, 0x50, 0xFF });
               await Task.Delay(100);
           });
        }

        [Test]
        public async Task Power_OnWithFeedback()
        {
            ViscaPower powerOnCmd = new ViscaPower(id, true);
            bool power = false;
            ViscaPowerInquiry powerInquiry = new ViscaPowerInquiry(id, new Action<bool>(p => { power = p; Console.WriteLine("Event: power: {0}", p); }));
            Action<ViscaRxPacket> powerOnOffCmdReply = new Action<ViscaRxPacket>(rxPacket => { if (rxPacket.IsCompletionCommand) visca.EnqueueCommand(powerInquiry); });

            visca.EnqueueCommand(powerOnCmd, powerOnOffCmdReply);

            Assert.That(sendQueue.TryTake(out byte[] powerPacket, 100), Is.True, "Timeout on sending data for Power On");
            Assert.That(powerPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x00, 0x02, 0xff }), Is.True, "Power On bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            // Next should be Power Inquiry
            Assert.That(sendQueue.TryTake(out byte[] powerInquiryPacket, 100), Is.True, "Timeout on sending data for Power Inquiry");
            Assert.That(powerInquiryPacket.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x00, 0xff }), Is.True, "Power Inquiry bytes sequence does not match expected");

            // Respond Power is On
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0x02, 0xFF });
            await Task.Delay(100);
            power.Should().Be(true);

            /*
            using (var monitoredSubject = power.Monitor())
            {
                // Send Power is ON
                visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0x02, 0xFF });
                await Task.Delay(100);
                monitoredSubject.Should().Raise("PowerChanged").WithSender(camera).WithArgs<ViscaCamera.OnOffEventArgs>(args => args.On == true);
            }
            camera.Power.Should().Be(true);
            */
        }

        [Test]
        public async Task Focus_Stop_PositionWithFeedback()
        {
            ViscaFocusStop focusStopCmd = new ViscaFocusStop(id);
            visca.EnqueueCommand(focusStopCmd);

            Assert.That(sendQueue.TryTake(out byte[] focusStopPacket, 100), Is.True, "Timeout on sending data for Focus Stop command");
            Assert.That(focusStopPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x08, 0x00, 0xff }), Is.True, "Focus Stop bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            ViscaFocusSpeed focusSpeed = new ViscaFocusSpeed(4);
            ViscaFocusFarWithSpeed focusFarWithSpeedCmd = new ViscaFocusFarWithSpeed(id, focusSpeed);
            visca.EnqueueCommand(focusFarWithSpeedCmd);

            Assert.That(sendQueue.TryTake(out byte[] focusFarWithSpeedPacket, 100), Is.True, "Timeout on sending data for Focus Far with Speed command");
            Assert.That(focusFarWithSpeedPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x08, 0x24, 0xff }), Is.True, "Focus Far with Speed bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            // Set Position 0xabcd or 43891
            ViscaFocusPosition focusPositionCmd = new ViscaFocusPosition(id, 0xabcd);
            visca.EnqueueCommand(focusPositionCmd);

            Assert.That(sendQueue.TryTake(out byte[] focusPositionPacket, 100), Is.True, "Timeout on sending data for Focus Position command");
            Assert.That(focusPositionPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x48, 0x0a, 0x0b, 0x0c, 0x0d, 0xff }), Is.True, "Focus Position bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            int focusPosition = 0;
            ViscaFocusPositionInquiry focusPositionInquiry = new ViscaFocusPositionInquiry(id, new Action<int>(f => { focusPosition = f; Console.WriteLine("Event: focus: 0x{0:X4} ({0})", f); }));
            visca.EnqueueCommand(focusPositionInquiry);

            Assert.That(sendQueue.TryTake(out byte[] focusPositionInquiryPacket, 100), Is.True, "Timeout on sending data for Focus Position Inquiry command");
            Assert.That(focusPositionInquiryPacket.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x48, 0xff }), Is.True, "Focus Position Inquiry bytes sequence does not match expected");

            // Respond Position 0xabcd
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0x0a, 0x0b, 0x0c, 0x0d, 0xFF });
            await Task.Delay(100);

            focusPosition.Should().Be(0xabcd);
        }

        [Test]
        public async Task PTZ_UpLeft_PositionWithFeedback()
        {
            ViscaPanSpeed panSpeed = new ViscaPanSpeed();
            ViscaTiltSpeed tiltSpeed = new ViscaTiltSpeed(4);

            panSpeed.Value = 9;
            ViscaPTZUpLeft ptzUpleftCmd = new ViscaPTZUpLeft(id, panSpeed, tiltSpeed);
            visca.EnqueueCommand(ptzUpleftCmd);

            Assert.That(sendQueue.TryTake(out byte[] ptzUpLeftPacket, 100), Is.True, "Timeout on sending data for PTZ UpLeft command");
            Assert.That(ptzUpLeftPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x06, 0x01, 0x09, 0x04, 0x01, 0x01, 0xff }), Is.True, "PTZ UpLeft with PanSpeed 9 TiltSpeed 4 bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            ViscaPTZPosition ptzPosition = new ViscaPTZPosition(id, false, panSpeed, tiltSpeed, 0x2abc, 0x3cba);
            visca.EnqueueCommand(ptzPosition);

            Assert.That(sendQueue.TryTake(out byte[] ptzPositionPacket, 100), Is.True, "Timeout on sending data for PTZ Position command");
            Assert.That(ptzPositionPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x06, 0x02, 0x09, 0x04, 0x02, 0x0a, 0x0b, 0x0c, 0x03, 0x0c, 0x0b, 0x0a, 0xff }), Is.True, "PTZ Position Pan:0x2abc Tilt:0x3cba with PanSpeed 9 TiltSpeed 4 bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            int panPosition = 0;
            int tiltPosition = 0;
            ViscaPTZPositionInquiry ptzPositionInquiry = new ViscaPTZPositionInquiry(id, new Action<int, int>((pan, tilt) => { panPosition = pan; tiltPosition = tilt; Console.WriteLine("Event: position: pan: 0x{0:X4} ({0}), tilt: 0x{1:X4} ({1})", pan, tilt); }));
            visca.EnqueueCommand(ptzPositionInquiry);

            Assert.That(sendQueue.TryTake(out byte[] ptzPositionInquiryPacket, 100), Is.True, "Timeout on sending data for PTZ Position Inquiry command");
            Assert.That(ptzPositionInquiryPacket.SequenceEqual(new byte[] { 0x81, 0x09, 0x06, 0x12, 0xff }), Is.True, "PTZ Position Inquiry bytes sequence does not match expected");

            // Respond Position pan at 0x3cba and tilt at 0x2abc
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0x03, 0x0c, 0x0b, 0x0a, 0x02, 0x0a, 0x0b, 0x0c, 0xFF });
            await Task.Delay(100);

            panPosition.Should().Be(0x3cba);
            tiltPosition.Should().Be(0x2abc);
        }

        [Test]
        public void Memory_SetRecall()
        {
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
                delegate { ViscaMemorySet presetBadCmd = new ViscaMemorySet(id, 9); });

            ViscaMemorySet presetCmd = new ViscaMemorySet(id, 5);
            visca.EnqueueCommand(presetCmd);

            Assert.That(sendQueue.TryTake(out byte[] memorySetPacket, 100), Is.True, "Timeout on sending data for Memory Set command");
            Assert.That(memorySetPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x3f, 0x01, 0x05, 0xff }), Is.True, "Memory Set 5 bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });

            ViscaMemoryRecall recallCmd = new ViscaMemoryRecall(id, 3);
            visca.EnqueueCommand(recallCmd);

            Assert.That(sendQueue.TryTake(out byte[] memoryRecallPacket, 100), Is.True, "Timeout on sending data for Memory Recall command");
            Assert.That(memoryRecallPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x3f, 0x02, 0x03, 0xff }), Is.True, "Memory Recall 3 bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });
        }

        [Test]
        public void ModeCommandTest()
        {
            ViscaModeCommand<WBMode> viscaModeCmd = new ViscaModeCommand<WBMode>(
                1,
                new byte[]{
                    Visca.Category.Camera1,
                    Visca.Commands.WB
                },
                "WB",
                PolyWBMode.Table);

            viscaModeCmd.Mode.Should().Equals(PolyWBMode.Table);

            viscaModeCmd.Mode = PolyWBMode.Indoor;

            viscaModeCmd.Mode.Should().Equals(PolyWBMode.Indoor);

            visca.EnqueueCommand(viscaModeCmd.SetMode(PolyWBMode.Table));

            Assert.That(sendQueue.TryTake(out byte[] testPacket, 100), Is.True, "Timeout on sending data for ViscaModeCommand<WBMode>");
            Assert.That(testPacket.SequenceEqual(new byte[] { 0x81, 0x01, 0x04, 0x35, 0x06, 0xff }), Is.True, "ViscaModeCommand<WBMode> with PolyWBMode.Table bytes sequence does not match expected");

            // Respond Completion
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });
        }

        [Test]
        public async Task WBInquiry()
        {
            WBMode wbMode = WBMode.Auto;
            ViscaWBInquiry wbInquiry = new ViscaWBInquiry(id, new Action<WBMode>( wb => { wbMode = wb; Console.WriteLine("Event: WB Mode: {0}", wb); }));
            visca.EnqueueCommand(wbInquiry);

            Assert.That(sendQueue.TryTake(out byte[] wbInquiryPacket, 100), Is.True, "Timeout on sending data for WB Inquiry command");
            Assert.That(wbInquiryPacket.SequenceEqual(new byte[] { 0x81, 0x09, 0x04, 0x35, 0xff }), Is.True, "WB Inquiry bytes sequence does not match expected");

            // Respond WB mode Manual
            visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0x05, 0xFF });
            await Task.Delay(100);

            wbMode.Should().Be(WBMode.Manual);

        }
    }
}
