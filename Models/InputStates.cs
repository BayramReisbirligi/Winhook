using static ReisProduction.Winhook.Services.Interop;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public static class InputStates
{
    public static bool IsKeyDown(VirtualKey key) => GetKeyState((ushort)key) < 0;
    public static bool IsKeyUp(VirtualKey key) => !IsKeyDown(key);
    public static bool IsHardwareKeyDown(VirtualKey key) => GetAsyncKeyState((ushort)key) < 0;
    public static bool IsHardwareKeyUp(VirtualKey key) => !IsHardwareKeyDown(key);
    public static bool IsTogglingKeyInEffect(VirtualKey key) => (GetKeyState((ushort)key) & 0x1) is 0x1;
}