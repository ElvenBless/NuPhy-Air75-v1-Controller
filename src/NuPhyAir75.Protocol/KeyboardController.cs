using NuPhyAir75.Protocol.Models;
using NuPhyAir75.Protocol.Models.Enums;
using NuPhyAir75.Protocol.Services;

namespace NuPhyAir75.Protocol
{
    public record UsbInfo(ushort Vid, ushort Pid);

    public sealed class KeyboardController(HidFeatureTransport transport)
    {
        private readonly HidFeatureTransport _transport = transport;

        public Task<bool> SetModeAsync(Air75Mode mode, BrightnesMode brightnesMode, DebounceMode debounceMode, SpeedMode speedMode, CancellationToken ct = default)
        {
            var frame = FrameBuilder.BuildMode(mode, brightnesMode, debounceMode, speedMode);
            return Task.FromResult(_transport.TrySendFeatureReport(frame, out _));
        }

        public Task<bool> SetColorAsync(Air75Mode ledMode, byte r, byte g, byte b, CancellationToken ct = default)
        {
            var order = Air75Modes.Map[ledMode].Order;
            var frame = FrameBuilder.BuildColor(r, g, b, order);
            return Task.FromResult(_transport.TrySendFeatureReport(frame, out _));
        }

        public Task<bool> SendRawFrameAsync(ReadOnlyMemory<byte> frame, CancellationToken ct = default)
            => Task.FromResult(_transport.TrySendFeatureReport(frame.ToArray(), out _));
    }
}
