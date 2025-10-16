using System.Runtime.InteropServices;
namespace ReisProduction.Winhooks.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct HARDWAREINPUT
{
    internal int uMsg;
    internal short
        wParamL,
        wParamH;
}