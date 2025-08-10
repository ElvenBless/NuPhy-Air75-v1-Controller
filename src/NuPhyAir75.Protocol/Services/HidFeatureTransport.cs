using NuPhyAir75.Protocol.Interop;
using NuPhyAir75.Protocol.Interop.Structs;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NuPhyAir75.Protocol.Services
{
    public sealed class HidFeatureTransport(UsbInfo _usbInfo)
    {
        public bool TrySendFeatureReport(byte[] reportStartingWithId, out string error)
        {
            error = null;

            if (reportStartingWithId == null || reportStartingWithId.Length < 2)
            {
                error = "Empty payload.";
                return false;
            }

            HidNativeMethods.HidD_GetHidGuid(out Guid hidGuid);

            nint devs = HidNativeMethods.SetupDiGetClassDevs(ref hidGuid, nint.Zero, nint.Zero,
                HidNativeMethods.DIGCF_PRESENT | HidNativeMethods.DIGCF_DEVICEINTERFACE);
            if (devs == nint.Zero || devs.ToInt64() == -1)
            {
                error = "SetupDiGetClassDevs failed: " + new Win32Exception(Marshal.GetLastWin32Error()).Message;
                return false;
            }

            try
            {
                var did = new SP_DEVICE_INTERFACE_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>() };
                uint index = 0;

                while (HidNativeMethods.SetupDiEnumDeviceInterfaces(devs, nint.Zero, ref hidGuid, index, ref did))
                {
                    index++;

                    HidNativeMethods.SetupDiGetDeviceInterfaceDetail(devs, ref did, nint.Zero, 0, out uint needed, nint.Zero);

                    var detail = new SP_DEVICE_INTERFACE_DETAIL_DATA
                    {
                        cbSize = (uint)(nint.Size == 8 ? 8 : 4 + Marshal.SystemDefaultCharSize)
                    };

                    if (!HidNativeMethods.SetupDiGetDeviceInterfaceDetail(devs, ref did, ref detail, (uint)Marshal.SizeOf<SP_DEVICE_INTERFACE_DETAIL_DATA>(), out _, nint.Zero))
                        continue;

                    using var handle = HidNativeMethods.CreateFile(detail.DevicePath,
                        HidNativeMethods.GENERIC_READ | HidNativeMethods.GENERIC_WRITE,
                        HidNativeMethods.FILE_SHARE_READ | HidNativeMethods.FILE_SHARE_WRITE,
                        nint.Zero,
                        HidNativeMethods.OPEN_EXISTING,
                        0,
                        nint.Zero);

                    if (handle.IsInvalid)
                        continue;

                    var attr = new HIDD_ATTRIBUTES { Size = Marshal.SizeOf<HIDD_ATTRIBUTES>() };
                    if (!HidNativeMethods.HidD_GetAttributes(handle, ref attr))
                        continue;
                    if (attr.VendorID != _usbInfo.Vid || attr.ProductID != _usbInfo.Pid)
                        continue;

                    if (!HidNativeMethods.HidD_GetPreparsedData(handle, out nint ppd))
                        continue;

                    try
                    {
                        if (HidNativeMethods.HidP_GetCaps(ppd, out HIDP_CAPS caps) != 0)
                        {
                            ushort featureLen = caps.FeatureReportByteLength;

                            if (featureLen >= reportStartingWithId.Length)
                            {
                                var buffer = new byte[featureLen];
                                Array.Copy(reportStartingWithId, 0, buffer, 0, reportStartingWithId.Length);

                                if (!HidNativeMethods.HidD_SetFeature(handle, buffer, buffer.Length))
                                {
                                    error = "HidD_SetFeature failed: " +
                                            new Win32Exception(Marshal.GetLastWin32Error()).Message;
                                    return false;
                                }

                                return true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        HidNativeMethods.HidD_FreePreparsedData(ppd);
                    }
                }

                error = "No suitable HID interface was found. Most likely you get to the wrong MI_xx (interface) with short Feature.";
                return false;
            }
            finally
            {
                HidNativeMethods.SetupDiDestroyDeviceInfoList(devs);
            }
        }
    }
}