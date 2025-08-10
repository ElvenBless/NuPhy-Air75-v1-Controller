using NuPhyCommander.Services;

namespace NuPhyCommander;

public enum PixelOrder
{
    GB0R,
    BG0R,
    GR0B,
    BR0G,
}

public enum Air75Mode : byte
{
    None = 0,
    FixedOn,
    Respire,
    Rainbow,
    FlashAway,
    Raindrops,
    RainbowWheel,
    RipplesShining,
    StarsTwinkle,
    ShadowDisappear,
    RetroSnake,
    NeonStream,
    Reaction,
    SineWave,
    RetinueScanning,
    RotatingWindmill,
    ColorfulWaterfall,
    Blossoming,
    RotatingStorm,
    Collision,
    Perfect,
    GameMode,
    OFF
}

public enum BrightnesMode : byte
{
    None = 0x20,
    One = 0x21,
    Two = 0x22,
    Three = 0x23,
    Four = 0x24,
}

public enum DebounceMode : byte
{
    None = 0x20,
    One = 0x21,
    Two = 0x22,
    Three = 0x23,
    Four = 0x24,
    Fifts = 0x25,
}

public enum SpeedMode : byte
{
    None = 0x00,
    One = 0x13,
    Two = 0x23,
    Three = 0x33,
    Four = 0x43,
}

public sealed class ModeSpec
{
    public PixelOrder Order { get; init; }
    public bool SupportsUniformColor { get; init; } = true;
    public bool SupportsBrightness { get; init; } = true;
    public bool SupportsSpeed { get; init; } = true;
    public bool SupportsDebounce { get; init; } = true;
}

public static class Air75Modes
{
    public static readonly Dictionary<Air75Mode, ModeSpec> Map = new()
    {
        { Air75Mode.FixedOn, new ModeSpec { Order = PixelOrder.BG0R, SupportsSpeed=false } },
        { Air75Mode.NeonStream,   new ModeSpec { Order = PixelOrder.BR0G } },
    };
}

class Program
{
    private const ushort TARGET_VID = 0x05AC;
    private const ushort TARGET_PID = 0x024F;

    static void Main()
    {
        string err2 = string.Empty;

        while (true)
        {
            HidFeatureReportSender.TrySendFeatureReport(TARGET_VID, TARGET_PID, BuildSingleDynamicColor(0x00, 0x00, 0xFF, Air75Modes.Map[Air75Mode.NeonStream].Order), out err2);
            HidFeatureReportSender.TrySendFeatureReport(TARGET_VID, TARGET_PID, BuildMode(Air75Mode.NeonStream, BrightnesMode.Four, DebounceMode.Three, SpeedMode.One), out err2);
            Task.Delay(3000).GetAwaiter().GetResult();
            HidFeatureReportSender.TrySendFeatureReport(TARGET_VID, TARGET_PID, BuildSingleDynamicColor(0x00, 0xFF, 0x00, Air75Modes.Map[Air75Mode.NeonStream].Order), out err2);
            HidFeatureReportSender.TrySendFeatureReport(TARGET_VID, TARGET_PID, BuildMode(Air75Mode.NeonStream, BrightnesMode.Four, DebounceMode.Three, SpeedMode.Four), out err2);
            Task.Delay(3000).GetAwaiter().GetResult();
        }
    }

    public static byte[] BuildSingleDynamicColor(byte r, byte g, byte b, PixelOrder pixelOrder, int ledCount = 250)
    {
        if (ledCount < 0) ledCount = 0;

        byte[] header = [0x06, 0x08, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00];

        const int tailZeros = 16;
        int totalLen = header.Length + ledCount * 4 + tailZeros;
        var frame = new byte[totalLen];

        Buffer.BlockCopy(header, 0, frame, 0, header.Length);

        int p = header.Length;
        for (int i = 0; i < ledCount; i++)
        {
            WritePixel(frame, p, r, g, b, pixelOrder);
            p += 4;
        }

        return frame;
    }

    public static void WritePixel(byte[] buf, int p, byte r, byte g, byte b, PixelOrder order)
    {
        switch (order)
        {
            case PixelOrder.GB0R:
                buf[p + 0] = g; buf[p + 1] = b; buf[p + 2] = 0x00; buf[p + 3] = r; break;

            case PixelOrder.BR0G:
                buf[p + 0] = b; buf[p + 1] = r; buf[p + 2] = 0x00; buf[p + 3] = g; break;

            case PixelOrder.BG0R:
                buf[p + 0] = b; buf[p + 1] = g; buf[p + 2] = 0x00; buf[p + 3] = r; break;

            case PixelOrder.GR0B:
                buf[p + 0] = g; buf[p + 1] = r; buf[p + 2] = 0x00; buf[p + 3] = b; break;
        }
    }

    public static byte[] BuildMode(Air75Mode ledMode, BrightnesMode brightnesMode, DebounceMode debounceMode, SpeedMode speedMode)
    {
        byte[] header = [
            0x06, 0x03, 0xb6, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x5a, 0xa5,
            0x03, 0x03, 0x01, 0x00,
            0x00, 0x0b, 0x20, 0x01,
            0x00, 0x00, 0x00, 0x00,
            0x55, 0x55, 0x01, 0x00,
            0x00, 0x00, 0x05, 0x00,
            0xff, 0xff, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x07, 0x04, 0x07, 0x04,
            0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x5a, 0xa5, 0x03,
            0x03, (byte)debounceMode, 0x00, 0x00,
            (byte)ledMode, 0x20, 0x01, 0x00,
            0x00, 0x00, 0x00, 0x55,
            0x55, 0x01, 0x00, 0x00,
            0x00, 0x00, 0x00, 0xff,
            0xff, 0x00, (byte)brightnesMode, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x00, (byte)speedMode, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x07, 0x23, 0x07,
            0x23, 0x00, 0x00, 0x00,
            0x00, 0x07, 0x04, 0x07,
            0x04, 0x07, 0x04, 0x07,
            0x04, 0x07, 0x04, 0x07,
            0x04, 0x07, 0x04, 0x09,
            0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x09, 0x09,
            0x09, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x5a, 0xa5, 0x00, 0x00];

        const int tailZeros = 768; //192 * 4 blocks of 4 bytes
        int totalLen = header.Length + tailZeros;
        var frame = new byte[totalLen];

        Buffer.BlockCopy(header, 0, frame, 0, header.Length);

        return frame;
    }
}