using System.Runtime.InteropServices;
namespace ReisProduction.Winhook.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
public struct POINT(int x, int y)
{
    public int
        X = x,
        Y = y;
}