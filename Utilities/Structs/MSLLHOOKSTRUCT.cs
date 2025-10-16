using System.Runtime.InteropServices;
namespace ReisProduction.Winhooks.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct MSLLHOOKSTRUCT
{
    internal POINT pt;
    internal uint
        mouseData,
        flags,
        time,
        dwExtraInfo;
}