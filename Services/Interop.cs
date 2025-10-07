using static ReisProduction.Winhook.Utilities.Constants;
using ReisProduction.Winhook.Utilities.Structs;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Text;
namespace ReisProduction.Winhook.Services;
public static class Interop
{
    internal delegate void WinEventDelegate(nint hWinEventHook, uint eventType, nint hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    internal delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);
    internal delegate nint LowLevelMouseProc(int nCode, nint wParam, nint lParam);
    public static POINT GetCursorPos() => GetCursorPos(out POINT point) ? point :
        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to get cursor position");
    public static string GetWindowTitle(nint hwnd)
    {
        StringBuilder sb = new(MAX_TITLE_LENGTH);
        return GetWindowText(hwnd, sb, sb.Capacity) > 0 ? sb.ToString() : string.Empty;
    }
    public static bool IsProcessRunningAsAdmin(int pid)
    {
        try
        {
            var process = Process.GetProcessById(pid);
            nint processHandle = OpenProcess(
                PROCESS_QUERY_LIMITED_INFORMATION,
                false,
                process.Id);
            try
            {
                if (processHandle == nint.Zero || !OpenProcessToken(processHandle,
                    TOKEN_QUERY, out nint tokenHandle))
                    return false;
                using WindowsIdentity identity = new(tokenHandle);
                WindowsPrincipal principal = new(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            finally { CloseHandle(processHandle); }
        }
        catch (SecurityException) { return true; }
        catch { return false; }
    }
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(nint ProcessHandle, uint DesiredAccess, out nint TokenHandle);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(nint hObject);
    [DllImport("user32.dll")]
    internal static extern nint SetWinEventHook(uint eventMin, uint eventMax, nint hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
    [DllImport("user32.dll")]
    internal static extern bool UnhookWinEvent(nint hWinEventHook);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetWindowText(nint hWnd, StringBuilder lpString, int nMaxCount);
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