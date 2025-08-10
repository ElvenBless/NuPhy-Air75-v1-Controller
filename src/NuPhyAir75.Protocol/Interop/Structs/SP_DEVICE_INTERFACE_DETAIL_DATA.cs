using System.Runtime.InteropServices;

namespace NuPhyAir75.Protocol.Interop.Structs;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
{
    public uint cbSize;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string DevicePath;
}