using ReisProduction.Winhook.Utilities.Structs;
using System.Runtime.InteropServices;
using System.ComponentModel;
namespace ReisProduction.Winhook.Services;
public static class Interop
{
    internal delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);
    internal delegate nint LowLevelMouseProc(int nCode, nint wParam, nint lParam);
    public static POINT GetCursorPos() => GetCursorPos(out POINT point) ? point :
        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to get cursor position");
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, nint hMod, uint dwThreadId);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnhookWindowsHookEx(nint hhk);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern nint GetModuleHandle(string lpModuleName);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern short GetKeyState(int nVirtKey);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern short GetAsyncKeyState(ushort key);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool CloseHandle(nint hObject);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint CreateWaitableTimer(nint lpTimerAttributes, bool bManualReset, string? lpTimerName);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetWaitableTimer(nint hTimer, ref long pDueTime, int lPeriod, nint pfnCompletionRoutine, nint lpArgToCompletionRoutine, bool fResume);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);
    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint timeBeginPeriod(uint uMilliseconds);
    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint timeEndPeriod(uint uMilliseconds);
}