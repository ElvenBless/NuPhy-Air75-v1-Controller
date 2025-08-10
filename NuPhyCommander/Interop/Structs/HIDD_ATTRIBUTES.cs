using System.Runtime.InteropServices;

namespace NuPhyCommander.Interop.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HIDD_ATTRIBUTES
    {
        public int Size;
        public ushort VendorID;
        public ushort ProductID;
        public ushort VersionNumber;
    }
}