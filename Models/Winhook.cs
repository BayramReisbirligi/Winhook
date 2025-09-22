using static ReisProduction.Winhook.Utilities.Constants;
using static ReisProduction.Winhook.Services.Interop;
using ReisProduction.Winhook.Utilities.Enums;
using System.Management;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class Winhook : IDisposable
{
    private readonly Dictionary<EventType, (uint min, uint max)> _hookRanges = new()
    {
        { EventType.Foreground, (EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND) },
        { EventType.Minimize, (EVENT_SYSTEM_MINIMIZESTART, EVENT_SYSTEM_MINIMIZEEND) },
        { EventType.Switch, (EVENT_SYSTEM_SWITCHSTART, EVENT_SYSTEM_SWITCHEND) },
        { EventType.MoveSize, (EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND) },
        { EventType.Menu, (EVENT_SYSTEM_MENUSTART, EVENT_SYSTEM_MENUEND) },
        { EventType.MenuPopup, (EVENT_SYSTEM_MENUPOPUPSTART, EVENT_SYSTEM_MENUPOPUPEND) },
        { EventType.Capture, (EVENT_SYSTEM_CAPTURESTART, EVENT_SYSTEM_CAPTUREEND) },
        { EventType.ContextHelp, (EVENT_SYSTEM_CONTEXTHELPSTART, EVENT_SYSTEM_CONTEXTHELPEND) },
        { EventType.DragDrop, (EVENT_SYSTEM_DRAGDROPSTART, EVENT_SYSTEM_DRAGDROPEND) },
        { EventType.Dialog, (EVENT_SYSTEM_DIALOGSTART, EVENT_SYSTEM_DIALOGEND) },
        { EventType.Scrolling, (EVENT_SYSTEM_SCROLLINGSTART, EVENT_SYSTEM_SCROLLINGEND) },
        { EventType.DesktopSwitch, (EVENT_SYSTEM_DESKTOPSWITCH, EVENT_SYSTEM_DESKTOPSWITCH) },
        { EventType.Console, (EVENT_CONSOLE_CARET, EVENT_CONSOLE_END_APPLICATION) },
        { EventType.Uia, (EVENT_UIA_EVENTID_START, EVENT_UIA_EVENTID_END) },
        { EventType.Object, (EVENT_OBJECT_CREATE, EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED) }
    };
    private readonly Dictionary<EventType, nint> _hookIds = new()
    {
        { EventType.Foreground, nint.Zero },
        { EventType.Minimize, nint.Zero },
        { EventType.Switch, nint.Zero },
        { EventType.MoveSize, nint.Zero },
        { EventType.Menu, nint.Zero },
        { EventType.MenuPopup, nint.Zero },
        { EventType.Capture, nint.Zero },
        { EventType.ContextHelp, nint.Zero },
        { EventType.DragDrop, nint.Zero },
        { EventType.Dialog, nint.Zero },
        { EventType.Scrolling, nint.Zero },
        { EventType.DesktopSwitch, nint.Zero },
        { EventType.Console, nint.Zero },
        { EventType.Uia, nint.Zero },
        { EventType.Object, nint.Zero }
    };
    private readonly LowLevelKeyboardProc _keyboardProc;
    private readonly LowLevelMouseProc _mouseProc;
    private readonly WinEventDelegate _winEvents;
    private static nint
        _keyboardHookID = nint.Zero,
        _mouseHookID = nint.Zero;
    public Winhook()
    {
        _keyboardProc = KeyboardHookCallback;
        _mouseProc = MouseHookCallback;
        _winEvents = WinEventCallback;
        GC.KeepAlive(this);
    }
    public void StartOrStopHooks(params (EventType Action, bool State)[] items)
    {
        foreach (var item in items)
        {
            var action = item.Action;
            var start = item.State;
            switch (action)
            {
                case EventType.Keyboard:
                    if (start) StartKeyboardHook();
                    else StopKeyboardHook();
                    break;
                case EventType.Mouse:
                    if (start) StartMouseHook();
                    else StopMouseHook();
                    break;
                case EventType.Session:
                    HookSessionEvents(start);
                    break;
                default:
                    if (_hookRanges.ContainsKey(action))
                        if (start)
                        {
                            if (_hookIds[action] == nint.Zero)
                                _hookIds[action] = SetEventHook(_hookRanges[action].min, _hookRanges[action].max);
                        }
                        else if (_hookIds[action] != nint.Zero)
                        {
                            UnhookWinEvent(_hookIds[action]);
                            _hookIds[action] = nint.Zero;
                        }
                    break;
            }
        }
    }
    public void HookDeviceEvents(bool shouldRun, params string[] deviceIds)
    {
        if (shouldRun)
        {
            foreach (var d in deviceIds.Where(x => !string.IsNullOrWhiteSpace(x)))
                _devices.Add(d.Trim());
            _deviceChanged?.Dispose();
            string query = "SELECT * FROM Win32_DeviceChangeEvent";
            if (_devices.Count > 0)
                query += " WHERE " + string.Join(" OR ", _devices.Select(d => $"DeviceID='{d}'"));
            _deviceChanged = new(new WqlEventQuery(query));
            _deviceChanged.EventArrived += OnDeviceChanged;
            _deviceChanged.Start();
        }
        else
        {
            _deviceChanged?.Dispose();
            _devices.Clear();
        }
    }
    public void HookVolumeEvents(bool shouldRun, params string[] drives)
    {
        if (shouldRun)
        {
            foreach (var v in drives.Where(x => !string.IsNullOrWhiteSpace(x)))
                _volumes.Add(v.Trim());
            _volumeChanged?.Dispose();
            string query = "SELECT * FROM Win32_VolumeChangeEvent";
            if (_volumes.Count > 0)
                query += " WHERE " + string.Join(" OR ", _volumes.Select(v => $"DriveName='{v}'"));
            _volumeChanged = new(new WqlEventQuery(query));
            _volumeChanged.EventArrived += OnVolumeChanged;
            _volumeChanged.Start();
        }
        else
        {
            _volumeChanged?.Dispose();
            _volumes.Clear();
        }
    }
    public void HookProcess(bool shouldRun, params string[] names)
    {
        var set = shouldRun ? _start : _stop;
        foreach (var n in names.Where(x => !string.IsNullOrWhiteSpace(x)))
            set.Add(n.Trim());
        RecreateProcessWatcher(shouldRun);
    }
    public void UnhookProcess(bool shouldRun, params string[] names)
    {
        var set = shouldRun ? _start : _stop;
        foreach (var n in names.Where(x => !string.IsNullOrWhiteSpace(x)))
            set.Remove(n.Trim());
        RecreateProcessWatcher(shouldRun);
    }
    public void ClearHookedProcess(bool shouldRun)
    {
        (shouldRun ? _start : _stop).Clear();
        RecreateProcessWatcher(shouldRun);
    }
    public void HookFiles(NotifyFilters filters = AllNotifyFilters,
        bool includeCreated = true, bool includeChanged = true,
        bool includeDeleted = true, bool includeRenamed = true,
        bool includeError = true, params string[] paths)
    {
        foreach (var p in paths.Where(File.Exists))
            _files.Add(Path.GetFullPath(p));
        RecreateFileWatcher(filters, includeCreated, includeChanged, includeDeleted, includeRenamed, includeError);
    }
    public void UnhookFiles(params string[] paths)
    {
        foreach (var p in paths)
            _files.Remove(Path.GetFullPath(p));
        RecreateFileWatcher();
    }
    public void ClearHookedFiles()
    {
        _files.Clear();
        RecreateFileWatcher();
    }
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
    public void FilterAllType(bool isKey, bool filter)
    {
        if (isKey)
            if (filter)
                _filteredKeys.UnionWith(_validKeys);
            else
                _filteredKeys.Clear();
        else if (filter)
            _filteredMice.UnionWith(Enum.GetValues<MouseType>());
        else
            _filteredMice.Clear();
    }
    public void Dispose()
    {
        StartOrStopHooks
        ([
            .. _hookIds.Keys
            .Select(k => (Action: k, State: false))
        ]);
        _procStarted?.Dispose();
        _procStopped?.Dispose();
        GC.SuppressFinalize(this);
    }
}