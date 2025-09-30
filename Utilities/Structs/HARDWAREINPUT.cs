using System.Runtime.InteropServices;
namespace ReisProduction.Winhook.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct HARDWAREINPUT
{
    internal int uMsg;
    internal short
        wParamL,
        wParamH;
}