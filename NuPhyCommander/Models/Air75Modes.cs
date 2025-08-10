using NuPhyCommander.Models.Enums;

namespace NuPhyCommander.Models;

public static class Air75Modes
{
    public static readonly Dictionary<Air75Mode, ModeSpec> Map = new()
    {
        { Air75Mode.FixedOn, new ModeSpec(PixelOrder.BG0R) { SupportsSpeed=false } },
        { Air75Mode.NeonStream,   new ModeSpec(PixelOrder.BR0G) },
    };
}