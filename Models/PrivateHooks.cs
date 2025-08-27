using static ReisProduction.Windelay.Models.DelayExecutor;
using static ReisProduction.Winhook.Utilities.Constants;
using static ReisProduction.Winhook.Services.Interop;
using ReisProduction.Winhook.Utilities.Structs;
using ReisProduction.Windelay.Utilities.Enums;
using ReisProduction.Winhook.Utilities.Enums;
using ReisProduction.Windelay.Utilities;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.ComponentModel;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class InputHook
{
    private readonly ConcurrentDictionary<ButtonType, CancellationTokenSource> _holdTokens = [];
    private readonly ConcurrentDictionary<ButtonType, DateTime> _lastClickTimes = [];
    private readonly HashSet<VirtualKey> _filteredKeys = [];
    private readonly HashSet<MouseType> _filteredMice = [];
    public void FilterKeys(params VirtualKey[] keys)
    {
        foreach (var key in keys)
            _filteredKeys.Add(key);
    }
    public void UnfilterKeys(params VirtualKey[] keys)
    {
        foreach (var key in keys)
            _filteredKeys.Remove(key);
    }
    public void FilterMice(params MouseType[] mice)
    {
        foreach (var mouse in mice)
            _filteredMice.Add(mouse);
    }
    public void UnfilterMice(params MouseType[] mice)
    {
        foreach (var mouse in mice)
            _filteredMice.Remove(mouse);
    }
    public void ClearKeyFilters() => _filteredKeys.Clear();
    public void ClearMouseFilters() => _filteredMice.Clear();
    public DelayType HoldDelayType { get; set; } = DelayType.TaskDelay;
    public bool AllowClickOnDoubleClick { get; set; } = true;
    public bool AllowPressOnDoublePress { get; set; } = true;
    public bool AcceptInjectedKeyboard { get; set; } = true;
    public bool AcceptInjectedMouse { get; set; } = true;
    public bool AcceptNoneInput { get; set; } = false;
    public int DoubleClickThresholdMs { get; set; } = 250;
    public int DoublePressThresholdMs { get; set; } = 250;
    public int MovementThreshold { get; set; } = 1;
    public int PressIntervalMs { get; set; } = 50;
    public int HoldIntervalMs { get; set; } = 50;
    public int MoveThresholdMs { get; set; } =
        Math.Clamp(200 / Environment.ProcessorCount, 25, 100);
    private static DateTime _lastMoveTime;
    private static int _lastX, _lastY;
    private void StartHookMouse()
    {
        if (_mouseHookId == nint.Zero)
        {
            var _cursorPos = GetCursorPos();
            _lastX = _cursorPos.X;
            _lastY = _cursorPos.Y;
            _mouseHookId = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, nint.Zero, 0);
            if (_mouseHookId == nint.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Mouse hook failed");
            GC.KeepAlive(_mouseProc);
        }
    }
    private nint MouseHookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0)
            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
        var data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        if (!AcceptInjectedMouse && (data.flags & MOUSEEVENTF_INJECTED) is not 0)
            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
        var mappedKey = InputType.None;
        bool isDown = false, isScroll = false;
        switch (wParam)
        {
            case WM_LBUTTONDOWN:
                mappedKey = InputType.LeftButton;
                isDown = true;
                break;
            case WM_LBUTTONUP:
                mappedKey = InputType.LeftButton;
                break;
            case WM_RBUTTONDOWN:
                mappedKey = InputType.RightButton;
                isDown = true;
                break;
            case WM_RBUTTONUP:
                mappedKey = InputType.RightButton;
                break;
            case WM_MBUTTONDOWN:
                mappedKey = InputType.MiddleButton;
                isDown = true;
                break;
            case WM_MBUTTONUP:
                mappedKey = InputType.MiddleButton;
                break;
            case WM_XBUTTONDOWN or WM_XBUTTONUP:
                var button = data.mouseData >> 16;
                if (button is XBUTTON1 or XBUTTON2)
                {
                    mappedKey = button is XBUTTON1 ? InputType.XButton1 : InputType.XButton2;
                    if (wParam is WM_XBUTTONDOWN) isDown = true;
                }
                break;
            case WM_MOUSEWHEEL:
                var delta = (short)(data.mouseData >> 16);
                mappedKey = delta > 0 ? InputType.MouseScrollUp : InputType.MouseScrollDown;
                isScroll = true;
                break;
            case WM_MOUSEHWHEEL:
                var deltaH = (short)(data.mouseData >> 16);
                mappedKey = deltaH > 0 ? InputType.MouseScrollRight : InputType.MouseScrollLeft;
                isScroll = true;
                break;
            case WM_MOUSEMOVE:
                HandleMouseMovement(data.pt.X, data.pt.Y);
                return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
        }
        if (_filteredMice.Contains((MouseType)mappedKey))
            return 1;
        else if (isScroll && TryGetScrollType(mappedKey, out var scroll))
            OnMouseScroll(scroll);
        else if (TryGetButtonType(mappedKey, out var button))
            if (isDown)
            {
                OnMouseDown(button);
                CancellationTokenSource token = new();
                _holdTokens.AddOrUpdate(button, token, (_, existingToken) =>
                {
                    existingToken.Cancel();
                    return token;
                });
                _ = Task.Run(async () =>
                {
                    DelayAction action = new(
                        DelayMilisecond: HoldIntervalMs,
                        Token: token.Token,
                        DelayType: HoldDelayType
                    );
                    while (!token.IsCancellationRequested)
                    {
                        OnMouseHold(button);
                        await HandleDelay(action);
                    }
                });
            }
            else
            {
                OnMouseUp(button);
                if (_holdTokens.TryRemove(button, out var cts))
                    cts.Cancel();
                var now = DateTime.UtcNow;
                if (_lastClickTimes.TryGetValue(button, out var last) &&
                    (now - last).TotalMilliseconds <= DoubleClickThresholdMs)
                {
                    _lastClickTimes.TryRemove(button, out _);
                    if (AllowClickOnDoubleClick)
                        OnMouseClick(button);
                    OnMouseDoubleClick(button);
                }
                else
                    OnMouseClick(button);
                _lastClickTimes.AddOrUpdate(button, now, (_, _) => now);
            }
        return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
    }
    private readonly ConcurrentDictionary<VirtualKey, DateTime> _lastKeyTimes = [];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void HandleMouseMovement(int x, int y)
    {
        var now = DateTime.UtcNow;
        if ((now - _lastMoveTime).TotalMilliseconds < MoveThresholdMs)
            return;
        _lastMoveTime = now;
        int deltaX = x - _lastX,
            deltaY = y - _lastY;
        if (Math.Abs(deltaX) >= MovementThreshold || Math.Abs(deltaY) >= MovementThreshold)
        {
            OnMouseMove(deltaX, deltaY);
            _lastX = x;
            _lastY = y;
        }
    }
    private static void StopHookMouse()
    {
        if (_mouseHookId != nint.Zero)
        {
            UnhookWindowsHookEx(_mouseHookId);
            _mouseHookId = nint.Zero;
        }
    }
    private void StartKeyboardHook()
    {
        if (_keyboardHookID == nint.Zero)
        {
            _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, nint.Zero, 0);
            if (_keyboardHookID == nint.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Keyboard hook failed");
            GC.KeepAlive(_keyboardProc);
        }
    }
    private nint KeyboardHookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0)
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        var kbd = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
        if (!AcceptInjectedKeyboard && (kbd.flags & LLKHF_INJECTED) is not 0)
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        var key = (VirtualKey)kbd.vkCode;
        if (IsValidKey(key))
            if (_filteredKeys.Contains(key))
                return 1;
            else if (wParam is WM_KEYDOWN or WM_SYSKEYDOWN)
            {
                var now = DateTime.UtcNow;
                OnKeyDown(key);
                if (_lastKeyTimes.TryGetValue(key, out var last) &&
                   (now - last).TotalMilliseconds <= DoublePressThresholdMs)
                {
                    _lastKeyTimes.TryRemove(key, out _);
                    if (AllowPressOnDoublePress)
                        OnKeyPress(key);
                    OnKeyDoublePress(key);
                }
                else
                {
                    OnKeyPress(key);
                    _lastKeyTimes.AddOrUpdate(key, now, (_, _) => now);
                }
            }
            else if (wParam is WM_KEYUP or WM_SYSKEYUP)
                OnKeyUp(key);
        return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
    }
    private static void StopKeyboardHook()
    {
        if (_keyboardHookID != nint.Zero)
        {
            UnhookWindowsHookEx(_keyboardHookID);
            _keyboardHookID = nint.Zero;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryGetButtonType(InputType input, out ButtonType button) =>
    (button = input switch
    {
        InputType.LeftButton => ButtonType.LeftButton,
        InputType.RightButton => ButtonType.RightButton,
        InputType.MiddleButton => ButtonType.MiddleButton,
        InputType.XButton1 => ButtonType.XButton1,
        InputType.XButton2 => ButtonType.XButton2,
        _ => ButtonType.None
    }) is not ButtonType.None || AcceptNoneInput;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryGetScrollType(InputType input, out ScrollType scroll) =>
    (scroll = input switch
    {
        InputType.MouseScrollLeft => ScrollType.MouseScrollLeft,
        InputType.MouseScrollRight => ScrollType.MouseScrollRight,
        InputType.MouseScrollUp => ScrollType.MouseScrollUp,
        InputType.MouseScrollDown => ScrollType.MouseScrollDown,
        _ => ScrollType.None
    }) is not ScrollType.None || AcceptNoneInput;
    private static readonly HashSet<ushort> _excludedValues =
    [
        .. Enum.GetValues<MouseType>().Cast<ushort>().Where(x => x is not (ushort)MouseType.None)
        .Concat(Enum.GetValues<MoveType>().Cast<ushort>().Where(x => x is not (ushort)MoveType.None))
    ];
    private static readonly HashSet<VirtualKey> _validKeys =
    [
        .. Enum.GetValues<InputType>()
        .Where(x => !_excludedValues.Contains((ushort)x))
        .Select(x => (VirtualKey)x)
    ];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsValidKey(VirtualKey key) =>
        key is (VirtualKey)InputType.None ? AcceptNoneInput : _validKeys.Contains(key);

}