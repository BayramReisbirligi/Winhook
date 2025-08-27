using static ReisProduction.Winhook.Services.Interop;
using ReisProduction.Winhook.Utilities.Enums;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class InputHook : IDisposable
{
    public event Action<InputType> InputDown = delegate { };
    public event Action<InputType> InputPress = delegate { };
    public event Action<InputType> InputDoublePress = delegate { };
    public event Action<InputType> InputUp = delegate { };
    public event Action<VirtualKey> KeyDown = delegate { };
    public event Action<VirtualKey> KeyPress = delegate { };
    public event Action<VirtualKey> KeyDoublePress = delegate { };
    public event Action<VirtualKey> KeyUp = delegate { };
    public event Action<ButtonType> MouseDown = delegate { };
    public event Action<ButtonType> MouseClick = delegate { };
    public event Action<ButtonType> MouseHold = delegate { };
    public event Action<ButtonType> MouseDoubleClick = delegate { };
    public event Action<ButtonType> MouseUp = delegate { };
    public event Action<ScrollType> MouseScroll = delegate { };
    public event Action<ScrollType> MouseScrollLeft = delegate { };
    public event Action<ScrollType> MouseScrollRight = delegate { };
    public event Action<ScrollType> MouseScrollUp = delegate { };
    public event Action<ScrollType> MouseScrollDown = delegate { };
    public event Action<int, int> MouseMove = delegate { };
    public event Action<int, int> MouseMoveLeft = delegate { };
    public event Action<int, int> MouseMoveRight = delegate { };
    public event Action<int, int> MouseMoveUp = delegate { };
    public event Action<int, int> MouseMoveDown = delegate { };
    public event Action<int, int> MouseMoveHorizontal = delegate { };
    public event Action<int, int> MouseMoveVertical = delegate { };
    private readonly LowLevelKeyboardProc _keyboardProc;
    private readonly LowLevelMouseProc _mouseProc;
    private static nint
        _keyboardHookID = nint.Zero,
        _mouseHookId = nint.Zero;
    public InputHook()
    {
        _keyboardProc = KeyboardHookCallback;
        _mouseProc = MouseHookCallback;
        GC.KeepAlive(this);
    }
    public void StartOrStopHook(bool isKeyboard, bool isStart)
    {
        if (isKeyboard)
            if (isStart)
                StartKeyboardHook();
            else
                StopKeyboardHook();
        else if (isStart)
            StartHookMouse();
        else
            StopHookMouse();
    }
    public void Dispose()
    {
        StopHookMouse();
        StopKeyboardHook();
        GC.SuppressFinalize(this);
    }
}