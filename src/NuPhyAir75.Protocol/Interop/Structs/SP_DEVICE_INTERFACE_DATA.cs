using System.Runtime.InteropServices;

namespace NuPhyAir75.Protocol.Interop.Structs;

[StructLayout(LayoutKind.Sequential)]
internal struct SP_DEVICE_INTERFACE_DATA
{
    public uint cbSize;
    public Guid InterfaceClassGuid;
    public uint Flags;
    public nint Reserved;
}