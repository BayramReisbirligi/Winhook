using System.Runtime.InteropServices;
namespace ReisProduction.Winhooks.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
public struct POINT(int x, int y)
{
    public int
        X = x,
        Y = y;
    public override readonly string ToString() => $"POINT [X={X}, Y={Y}]";
}