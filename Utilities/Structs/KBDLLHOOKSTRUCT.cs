using System.Runtime.InteropServices;
namespace ReisProduction.Winhook.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct KBDLLHOOKSTRUCT
{
    internal int
        vkCode,
        scanCode,
        flags,
        time;
    internal nint dwExtraInfo;
}