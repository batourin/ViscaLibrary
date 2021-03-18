using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace Visca.Tests
{
    [TestFixture]
    class UnitTestViscaCamera
    {
        ViscaProtocolProcessor visca;
        ViscaCamera camera;
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
                    Console.WriteLine("CAMERA LOG:[{0}]", String.Format(f, o));
                })
            );

            camera = new ViscaCamera(ViscaCameraId.Camera1, null, visca);
        }

        [Test]
        public async Task CameraMemoryRecallEvent()
        {
            camera.MemoryRecallComplete += (s, e) => { Console.WriteLine("Recall complete: {0} loaded", e.EventData); };
            camera.MemoryRecall(1);
            using (var monitoredSubject = camera.Monitor())
            {
                // Respond Completion
                visca.ProcessIncomingData(new byte[] { 0x90, 0x50, 0xFF });
                await Task.Delay(100);
                monitoredSubject.Should().Raise("MemoryRecallComplete").WithSender(camera).WithArgs<ViscaCamera.GenericEventArgs<byte>>(args => args.EventData == 1);
            }

        }
    }
}
