using static ReisProduction.Windelay.Models.DelayExecutor;
using static ReisProduction.Winhook.Utilities.Constants;
using static ReisProduction.Winhook.Services.Interop;
using ReisProduction.Winhook.Utilities.Structs;
using ReisProduction.Windelay.Utilities.Enums;
using ReisProduction.Winhook.Utilities.Enums;
using ReisProduction.Windelay.Utilities;
using ReisProduction.Winhook.Utilities;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Management;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class Winhook
{
    private nint SetEventHook(uint eventMin, uint eventMax) => SetWinEventHook(eventMin, eventMax, nint.Zero, _winEvents, 0, 0, WINEVENT_OUTOFCONTEXT);
    private readonly ConcurrentDictionary<VirtualKey, CancellationTokenSource> _keyHoldTokens = [];
    private readonly ConcurrentDictionary<ButtonType, CancellationTokenSource> _holdTokens = [];
    private readonly ConcurrentDictionary<ButtonType, DateTime> _lastClickTimes = [];
    private readonly ConcurrentDictionary<VirtualKey, DateTime> _lastKeyTimes = [];
    private readonly Dictionary<Type, ManagementEventWatcher?> _watchers = [];
    private readonly HashSet<VirtualKey> _filteredKeys = [];
    private readonly HashSet<MouseType> _filteredMice = [];
    private readonly HashSet<string> _files = [];
    private FileSystemWatcher? _fileWatcher;
    private static DateTime _lastMoveTime;
    private static int _lastX, _lastY;
    public DelayType MouseHoldDelayType { get; set; } = DelayType.TaskDelay;
    public DelayType KeyHoldDelayType { get; set; } = DelayType.TaskDelay;
    public bool AllowClickOnDoubleClick { get; set; } = true;
    public bool AllowPressOnDoublePress { get; set; } = true;
    public bool AcceptInjectedKeyboard { get; set; } = true;
    public bool AcceptInjectedMouse { get; set; } = true;
    public bool AcceptNoneInput { get; set; } = false;
    public bool AcceptSYSDown { get; set; } = true;
    public bool AcceptSYSUp { get; set; } = true;
    public bool AllowKeyPress { get; set; } = false;
    public bool AllowKeyHold { get; set; } = false;
    public bool AllowClick { get; set; } = false;
    public bool AllowHold { get; set; } = false;
    public int DoubleClickThresholdMs { get; set; } = 250;
    public int DoublePressThresholdMs { get; set; } = 250;
    public int MovementThreshold { get; set; } = 1;
    public int KeyHoldIntervalMs { get; set; } = 50;
    public int HoldIntervalMs { get; set; } = 50;
    public int MoveThresholdMs { get; set; } =
        Math.Clamp(200 / Environment.ProcessorCount, 25, 100);
    private RegisteredWaitHandle? _registeredWait;
    private SafeRegistryHandle? _registryHandle;
    public event Action? RegistryChanged;
    private AutoResetEvent? _event;
    private readonly string _keyPath;
    private readonly nuint _hive;
    public int RegPollTimeOut { get; set; } = -1;
    public void Start(bool WatchSubTree)
    {
        if (RegOpenKeyEx(_hive, _keyPath, 0, KEY_READ, out var handle) is not 0)
            throw new InvalidOperationException("Registry key açılamadı!");
        _registryHandle = new(handle, true);
        _event = new(false);
        _registeredWait = ThreadPool.RegisterWaitForSingleObject(
            _event,
            (_, __) => OnRegistryChanged(),
            null,
            RegPollTimeOut,
            true
        );
        Watch(WatchSubTree);
    }

    private void Watch(RegChangeNotifyFilter reg)
    {
        RegNotifyChangeKeyValue(
            _registryHandle!.DangerousGetHandle(),
            true, reg,
            _event!.SafeWaitHandle.DangerousGetHandle(),
            true
        );
    }

    private void OnRegistryChanged()
    {
        RegistryChanged?.Invoke();
        Watch();
    }

    public void Stop()
    {
        _registeredWait?.Unregister(null);
        _event?.Dispose();
        _registryHandle?.Dispose();
    }

    public void Dispsose() => Stop();

    #region WinAPI

    private const int KEY_READ = 0x20019;

    [Flags]
    private enum RegChangeNotifyFilter : uint
    {
        REG_NOTIFY_CHANGE_NAME = 1,
        REG_NOTIFY_CHANGE_ATTRIBUTES = 2,
        REG_NOTIFY_CHANGE_LAST_SET = 4,
        REG_NOTIFY_CHANGE_SECURITY = 8,
    }

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int RegOpenKeyEx(
        UIntPtr hKey, string lpSubKey, int ulOptions, int samDesired, out IntPtr phkResult);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegNotifyChangeKeyValue(
        IntPtr hKey, bool bWatchSubtree, RegChangeNotifyFilter dwNotifyFilter,
        IntPtr hEvent, bool fAsynchronous);

    #endregion
    private void StartOrStopHook(HookBase hook)
    {
        var type = hook.GetType();
        switch (hook)
        {
            case KeyboardHook:
                if (hook.ShouldStart) StartKeyboardHook();
                else StopKeyboardHook();
                break;
            case MouseHook:
                if (hook.ShouldStart) StartMouseHook();
                else StopMouseHook();
                break;
            case FileHook f:
                if (hook.ShouldStart) HookFiles(f);
                else UnhookFiles(f.Paths);
                break;
            default:
                if (_hookRanges.ContainsKey(type))
                {
                    if (hook.ShouldStart)
                    {
                        if (_hookIds[type] == nint.Zero)
                            _hookIds[type] = SetEventHook(_hookRanges[type].min, _hookRanges[type].max);
                    }
                    else if (_hookIds[type] != nint.Zero)
                    {
                        UnhookWinEvent(_hookIds[type]);
                        _hookIds[type] = nint.Zero;
                    }
                    return;
                }
                break;
        }
        if (!hook.ShouldStart)
        {
            if (_watchers.TryGetValue(type, out var w) && w is not null)
            {
                w.Dispose();
                _watchers.Remove(type);
            }
            return;
        }
        EventArrivedEventHandler handler;
        string query = "SELECT * FROM ";
        switch (hook)
        {
            case ProcessStartHook p:
                query += BuildQuery("Win32_ProcessStartTrace", "ProcessName", p.ProcessNames);
                handler = OnProcessStarted;
                break;
            case ProcessStopHook p:
                query += BuildQuery("Win32_ProcessStopTrace", "ProcessName", p.ProcessNames);
                handler = OnProcessStopped;
                break;
            case ServiceStartHook s:
                query = BuildQuery("Win32_ServiceStartTrace", "ServiceName", s.ServiceNames);
                handler = OnServiceStarted;
                break;
            case ServiceStopHook s:
                query = BuildQuery("Win32_ServiceStopTrace", "ServiceName", s.ServiceNames);
                handler = OnServiceStopped;
                break;
            case ThreadStartHook t:
                query += BuildQuery("Win32_ThreadStartTrace", "ThreadID", t.ThreadIds.Select(x => x.ToString()));
                handler = OnThreadStarted;
                break;
            case ThreadStopHook t:
                query += BuildQuery("Win32_ThreadStopTrace", "ThreadID", t.ThreadIds.Select(x => x.ToString()));
                handler = OnThreadStopped;
                break;
            case ModuleLoadHook m:
                query += BuildQuery("Win32_ModuleLoadTrace", "ModuleName", m.ModuleNames);
                handler = OnModuleLoaded;
                break;
            case ModuleUnloadHook m:
                query += BuildQuery("Win32_ModuleUnloadTrace", "ModuleName", m.ModuleNames);
                handler = OnModuleUnloaded;
                break;
            case SessionChangeHook:
                query = "Win32_SessionChangeEvent";
                handler = OnSessionChanged;
                break;
            case DeviceChangeHook d:
                query += BuildQuery("Win32_DeviceChangeEvent", "DeviceID", d.DeviceIds);
                handler = OnDeviceChanged;
                break;
            case VolumeChangeHook v:
                query += BuildQuery("Win32_VolumeChangeEvent", "DriveName", v.DriveNames);
                handler = OnVolumeChanged;
                break;
            case PowerManagementHook:
                query = "Win32_PowerManagementEvent";
                handler = OnPowerChanged;
                break;
            case SystemConfigChangeHook:
                query = "Win32_SystemConfigurationChangeEvent";
                handler = OnSystemConfigChanged;
                break;
            case TimeChangeHook:
                query = "Win32_TimeChangeEvent";
                handler = OnTimeChanged;
                break;
            case SystemTraceHook:
                query = "Win32_SystemTrace";
                handler = OnSystemTrace;
                break;
            case IP4RouteTableHook:
                query = "Win32_IP4RouteTableEvent";
                handler = OnIP4RouteChanged;
                break;
            case IP6RouteTableHook:
                query = "Win32_IP6RouteTableEvent";
                handler = OnIP6RouteChanged;
                break;
            case NetworkAdapterConfigChangeHook:
                query = "Win32_NetworkAdapterConfigurationChangeEvent";
                handler = OnNetworkAdapterConfigChanged;
                break;
            case ProcessTraceHook:
                query = "Win32_ProcessTrace";
                handler = OnProcessTrace;
                break;
            case ProcessStartTraceHook p:
                query += BuildQuery("Win32_ProcessStartTrace", "ProcessName", p.ProcessNames);
                handler = OnProcessTraceStarted;
                break;
            case ProcessStopTraceHook p:
                query += BuildQuery("Win32_ProcessStopTrace", "ProcessName", p.ProcessNames);
                handler = OnProcessTraceStopped;
                break;
            case ThreadTraceHook:
                query = "Win32_ThreadTrace";
                handler = OnThreadTrace;
                break;
            case ModuleTraceHook:
                query = "Win32_ModuleTrace";
                handler = OnModuleTrace;
                break;
            case BatchJobStartHook b:
                query += BuildQuery("Win32_BatchJobStartTrace", "JobName", b.JobNames);
                handler = OnBatchJobStarted;
                break;
            case BatchJobStopHook b:
                query += BuildQuery("Win32_BatchJobStopTrace", "JobName", b.JobNames);
                handler = OnBatchJobStopped;
                break;
            default:
                throw new NotSupportedException($"Hook not supported: {hook.GetType().Name}");
        }
        ManagementEventWatcher watcher = _watchers[type] = new(new WqlEventQuery(query));
        if (handler is not null) watcher.EventArrived += handler;
        watcher.Start();
    }
    private void RecreateFileWatcher(NotifyFilters filters = AllNotifyFilters,
        bool includeCreated = true, bool includeChanged = true,
        bool includeDeleted = true, bool includeRenamed = true,
        bool includeError = true)
    {
        _fileWatcher?.Dispose();
        _fileWatcher = null;
        if (_files.Count is 0) return;
        string folder = Path.GetDirectoryName(_files.First())!;
        _fileWatcher = new(folder)
        {
            Filter = "*.*",
            NotifyFilter = filters,
            IncludeSubdirectories = false
        };
        if (includeCreated) _fileWatcher.Created += OnFileCreated;
        if (includeChanged) _fileWatcher.Changed += OnFileChanged;
        if (includeDeleted) _fileWatcher.Deleted += OnFileDeleted;
        if (includeRenamed) _fileWatcher.Renamed += OnFileRenamed;
        if (includeError) _fileWatcher.Error += OnFileWatcherError;
        _fileWatcher.EnableRaisingEvents = true;
    }
    private static string BuildQuery(string query, string column, IEnumerable<string> filters)
    {
        if (filters.Any())
            query += " WHERE " + string.Join(" OR ", filters.Select(f => $"{column}='{f}'"));
        return query;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WinEventCallback(nint __, uint eventType, nint hWnd, int ___, int ____, uint _____, uint ______)
    {
        switch (eventType)
        {
            case EVENT_SYSTEM_SOUND: OnWindowSound(hWnd); break;
            case EVENT_SYSTEM_ALERT: OnWindowAlert(hWnd); break;
            case EVENT_SYSTEM_FOREGROUND: OnWindowForegroundChanged(hWnd); break;
            case EVENT_SYSTEM_MENUSTART: OnWindowMenuStart(hWnd); break;
            case EVENT_SYSTEM_MENUEND: OnWindowMenuEnd(hWnd); break;
            case EVENT_SYSTEM_MENUPOPUPSTART: OnWindowMenuPopupStart(hWnd); break;
            case EVENT_SYSTEM_MENUPOPUPEND: OnWindowMenuPopupEnd(hWnd); break;
            case EVENT_SYSTEM_CAPTURESTART: OnWindowCaptureStart(hWnd); break;
            case EVENT_SYSTEM_CAPTUREEND: OnWindowCaptureEnd(hWnd); break;
            case EVENT_SYSTEM_MOVESIZESTART: OnWindowMoveSizeStart(hWnd); break;
            case EVENT_SYSTEM_MOVESIZEEND: OnWindowMoveSizeEnd(hWnd); break;
            case EVENT_SYSTEM_CONTEXTHELPSTART: OnWindowContextHelpStart(hWnd); break;
            case EVENT_SYSTEM_CONTEXTHELPEND: OnWindowContextHelpEnd(hWnd); break;
            case EVENT_SYSTEM_DRAGDROPSTART: OnWindowDragDropStart(hWnd); break;
            case EVENT_SYSTEM_DRAGDROPEND: OnWindowDragDropEnd(hWnd); break;
            case EVENT_SYSTEM_DIALOGSTART: OnWindowDialogStart(hWnd); break;
            case EVENT_SYSTEM_DIALOGEND: OnWindowDialogEnd(hWnd); break;
            case EVENT_SYSTEM_SCROLLINGSTART: OnWindowScrollingStart(hWnd); break;
            case EVENT_SYSTEM_SCROLLINGEND: OnWindowScrollingEnd(hWnd); break;
            case EVENT_SYSTEM_SWITCHSTART: OnWindowSwitchStart(hWnd); break;
            case EVENT_SYSTEM_SWITCHEND: OnWindowSwitchEnd(hWnd); break;
            case EVENT_SYSTEM_MINIMIZESTART: OnWindowMinimizeStart(hWnd); break;
            case EVENT_SYSTEM_MINIMIZEEND: OnWindowMinimizeEnd(hWnd); break;
            case EVENT_SYSTEM_DESKTOPSWITCH: OnWindowDesktopSwitch(hWnd); break;
            case EVENT_CONSOLE_CARET: OnConsoleCaret(hWnd); break;
            case EVENT_CONSOLE_UPDATE_REGION: OnConsoleUpdateRegion(hWnd); break;
            case EVENT_CONSOLE_UPDATE_SIMPLE: OnConsoleUpdateSimple(hWnd); break;
            case EVENT_CONSOLE_UPDATE_SCROLL: OnConsoleUpdateScroll(hWnd); break;
            case EVENT_CONSOLE_LAYOUT: OnConsoleLayout(hWnd); break;
            case EVENT_CONSOLE_START_APPLICATION: OnConsoleStartApplication(hWnd); break;
            case EVENT_CONSOLE_END_APPLICATION: OnConsoleEndApplication(hWnd); break;
            case EVENT_UIA_PATTERNCHANGE: OnUiaPatternChange(hWnd); break;
            case EVENT_UIA_STRUCTURECHANGE: OnUiaStructureChange(hWnd); break;
            case EVENT_UIA_EVENTID_START: OnUiaEventIdStart(hWnd); break;
            case EVENT_UIA_EVENTID_END: OnUiaEventIdEnd(hWnd); break;
            case EVENT_OBJECT_CREATE: OnObjectCreate(hWnd); break;
            case EVENT_OBJECT_DESTROY: OnObjectDestroy(hWnd); break;
            case EVENT_OBJECT_SHOW: OnObjectShow(hWnd); break;
            case EVENT_OBJECT_HIDE: OnObjectHide(hWnd); break;
            case EVENT_OBJECT_REORDER: OnObjectReorder(hWnd); break;
            case EVENT_OBJECT_FOCUS: OnObjectFocus(hWnd); break;
            case EVENT_OBJECT_SELECTION: OnObjectSelection(hWnd); break;
            case EVENT_OBJECT_SELECTIONADD: OnObjectSelectionAdd(hWnd); break;
            case EVENT_OBJECT_SELECTIONREMOVE: OnObjectSelectionRemove(hWnd); break;
            case EVENT_OBJECT_SELECTIONWITHIN: OnObjectSelectionWithin(hWnd); break;
            case EVENT_OBJECT_STATECHANGE: OnObjectStateChange(hWnd); break;
            case EVENT_OBJECT_LOCATIONCHANGE: OnObjectLocationChange(hWnd); break;
            case EVENT_OBJECT_NAMECHANGE: OnObjectNameChange(hWnd); break;
            case EVENT_OBJECT_DESCRIPTIONCHANGE: OnObjectDescriptionChange(hWnd); break;
            case EVENT_OBJECT_VALUECHANGE: OnObjectValueChange(hWnd); break;
            case EVENT_OBJECT_PARENTCHANGE: OnObjectParentChange(hWnd); break;
            case EVENT_OBJECT_HELPCHANGE: OnObjectHelpChange(hWnd); break;
            case EVENT_OBJECT_DEFACTIONCHANGE: OnObjectDefActionChange(hWnd); break;
            case EVENT_OBJECT_ACCELERATORCHANGE: OnObjectAcceleratorChange(hWnd); break;
            case EVENT_OBJECT_INVOKED: OnObjectInvoked(hWnd); break;
            case EVENT_OBJECT_TEXTSELECTIONCHANGED: OnObjectTextSelectionChanged(hWnd); break;
            case EVENT_OBJECT_CONTENTSCROLLED: OnObjectContentScrolled(hWnd); break;
            case EVENT_OBJECT_ARRANGEMENTPREVIEW: OnObjectArrangementPreview(hWnd); break;
            case EVENT_OBJECT_CLOAKED: OnObjectCloaked(hWnd); break;
            case EVENT_OBJECT_UNCLOAKED: OnObjectUncloaked(hWnd); break;
            case EVENT_OBJECT_LIVEREGIONCHANGED: OnObjectLiveRegionChanged(hWnd); break;
            case EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED: OnObjectHostedObjectsInvalidated(hWnd); break;
            case EVENT_OBJECT_DRAGSTART: OnObjectDragStart(hWnd); break;
            case EVENT_OBJECT_DRAGCANCEL: OnObjectDragCancel(hWnd); break;
            case EVENT_OBJECT_DRAGCOMPLETE: OnObjectDragComplete(hWnd); break;
            case EVENT_OBJECT_DRAGENTER: OnObjectDragEnter(hWnd); break;
            case EVENT_OBJECT_DRAGLEAVE: OnObjectDragLeave(hWnd); break;
            case EVENT_OBJECT_DRAGDROPPED: OnObjectDragDropped(hWnd); break;
            case EVENT_OBJECT_IME_SHOW: OnObjectImeShow(hWnd); break;
            case EVENT_OBJECT_IME_HIDE: OnObjectImeHide(hWnd); break;
            case EVENT_OBJECT_IME_CHANGE: OnObjectImeChange(hWnd); break;
            case EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED: OnObjectTextEditConversionTargetChanged(hWnd); break;
        }
    }
    private void StartMouseHook()
    {
        if (_mouseHookID == nint.Zero)
        {
            var _cursorPos = GetCursorPos();
            _lastX = _cursorPos.X;
            _lastY = _cursorPos.Y;
            _mouseHookID = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, nint.Zero, 0);
            if (_mouseHookID == nint.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Mouse hook failed");
            GC.KeepAlive(_mouseProc);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private nint MouseHookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0)
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        var data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        if (!AcceptInjectedMouse && (data.flags & MOUSEEVENTF_INJECTED) is not 0)
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
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
                return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }
        if (_filteredMice.Contains((MouseType)mappedKey))
            return 1;
        else if (isScroll && TryGetScrollType(mappedKey, out var scroll))
            OnMouseScroll(scroll);
        else if (TryGetButtonType(mappedKey, out var button))
            if (isDown)
            {
                OnMouseDown(button);
                if (AllowHold)
                {
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
                            DelayType: MouseHoldDelayType
                        );
                        while (!token.IsCancellationRequested)
                        {
                            OnMouseHold(button);
                            await HandleDelay(action);
                        }
                    });
                }
            }
            else
            {
                if (_holdTokens.TryRemove(button, out var cts))
                    cts.Cancel();
                OnMouseUp(button);
                if (AllowClick)
                {
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
            }
        return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
    }
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
    private static void StopMouseHook()
    {
        if (_mouseHookID != nint.Zero)
        {
            UnhookWindowsHookEx(_mouseHookID);
            _mouseHookID = nint.Zero;
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
            else if (wParam is WM_KEYDOWN || (wParam is WM_SYSKEYDOWN && AcceptSYSDown))
            {
                var now = DateTime.UtcNow;
                OnKeyDown(key);
                if (AllowKeyHold)
                {
                    CancellationTokenSource token = new();
                    _keyHoldTokens.AddOrUpdate(key, token, (_, existingToken) =>
                    {
                        existingToken.Cancel();
                        return token;
                    });
                    _ = Task.Run(async () =>
                    {
                        DelayAction action = new(
                            DelayMilisecond: KeyHoldIntervalMs,
                            Token: token.Token,
                            DelayType: KeyHoldDelayType
                        );
                        while (!token.IsCancellationRequested)
                        {
                            OnKeyHold(key);
                            await HandleDelay(action);
                        }
                    });
                }
                if (AllowKeyPress)
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
            else if (wParam is WM_KEYUP || (wParam is WM_SYSKEYUP && AcceptSYSUp))
            {
                if (_keyHoldTokens.TryRemove(key, out var cts))
                    cts.Cancel();
                OnKeyUp(key);
            }
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