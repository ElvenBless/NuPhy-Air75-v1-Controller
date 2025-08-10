using NuPhyCommander.Models.Enums;

namespace NuPhyCommander.Models;

public sealed record ModeSpec(PixelOrder Order)
{
    public bool SupportsUniformColor { get; init; } = true;
    public bool SupportsBrightness { get; init; } = true;
    public bool SupportsSpeed { get; init; } = true;
    public bool SupportsDebounce { get; init; } = true;
}