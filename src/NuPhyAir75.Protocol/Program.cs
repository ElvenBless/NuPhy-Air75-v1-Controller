using NuPhyAir75.Protocol.Models.Enums;

namespace NuPhyAir75.Protocol;

class Program
{
    private const ushort TARGET_VID = 0x05AC;
    private const ushort TARGET_PID = 0x024F;

    static async Task Main()
    {
        KeyboardController keyboardController = new(new(new(TARGET_VID, TARGET_PID)));

        while (true)
        {
            await keyboardController.SetColorAsync(Air75Mode.NeonStream, 0x00, 0x00, 0xFF);
            await keyboardController.SetModeAsync(Air75Mode.NeonStream, BrightnesMode.Fourth, DebounceMode.Third, SpeedMode.First);
            Task.Delay(3000).GetAwaiter().GetResult();
            await keyboardController.SetColorAsync(Air75Mode.NeonStream, 0x00, 0xFF, 0x00);
            await keyboardController.SetModeAsync(Air75Mode.NeonStream, BrightnesMode.Fourth, DebounceMode.Third, SpeedMode.Fourth);
            Task.Delay(3000).GetAwaiter().GetResult();
        }
    }
}