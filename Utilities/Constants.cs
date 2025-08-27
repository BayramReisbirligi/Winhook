namespace ReisProduction.Winhook.Utilities;
internal static class Constants
{
    internal const uint
        MOUSEEVENTF_INJECTED = 0x00000001,
        XBUTTON1 = 0x0001,
        XBUTTON2 = 0x0002;
    internal const int
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        LLKHF_INJECTED = 0x10;
}