using System.Runtime.InteropServices;
namespace ReisProduction.Winhook.Utilities.Structs;
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