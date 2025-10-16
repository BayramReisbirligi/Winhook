using static ReisProduction.Winhooks.Services.NativeMethods;
using static ReisProduction.Winhooks.Utilities.Constants;
using ReisProduction.Winhooks.Utilities.Enums;
using ReisProduction.Winhooks.Utilities;
using Windows.System;
namespace ReisProduction.Winhooks.Models;
public partial class Winhooks : IDisposable
{
    private readonly Dictionary<Type, (uint min, uint max)> _hookRanges = new()
    {
        { typeof(ForegroundHook), (EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND) },
        { typeof(MinimizeHook), (EVENT_SYSTEM_MINIMIZESTART, EVENT_SYSTEM_MINIMIZEEND) },
        { typeof(SwitchHook), (EVENT_SYSTEM_SWITCHSTART, EVENT_SYSTEM_SWITCHEND) },
        { typeof(MoveSizeHook), (EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND) },
        { typeof(MenuHook), (EVENT_SYSTEM_MENUSTART, EVENT_SYSTEM_MENUEND) },
        { typeof(MenuPopupHook), (EVENT_SYSTEM_MENUPOPUPSTART, EVENT_SYSTEM_MENUPOPUPEND) },
        { typeof(CaptureHook), (EVENT_SYSTEM_CAPTURESTART, EVENT_SYSTEM_CAPTUREEND) },
        { typeof(ContextHelpHook), (EVENT_SYSTEM_CONTEXTHELPSTART, EVENT_SYSTEM_CONTEXTHELPEND) },
        { typeof(DragDropHook), (EVENT_SYSTEM_DRAGDROPSTART, EVENT_SYSTEM_DRAGDROPEND) },
        { typeof(DialogHook), (EVENT_SYSTEM_DIALOGSTART, EVENT_SYSTEM_DIALOGEND) },
        { typeof(ScrollingHook), (EVENT_SYSTEM_SCROLLINGSTART, EVENT_SYSTEM_SCROLLINGEND) },
        { typeof(DesktopSwitchHook), (EVENT_SYSTEM_DESKTOPSWITCH, EVENT_SYSTEM_DESKTOPSWITCH) },
        { typeof(ConsoleHook), (EVENT_CONSOLE_CARET, EVENT_CONSOLE_END_APPLICATION) },
        { typeof(UiaHook), (EVENT_UIA_EVENTID_START, EVENT_UIA_EVENTID_END) },
        { typeof(ObjectHook), (EVENT_OBJECT_CREATE, EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED) }
    };
    private readonly Dictionary<Type, nint> _hookIds = new()
    {
        { typeof(ForegroundHook), nint.Zero },
        { typeof(MinimizeHook), nint.Zero },
        { typeof(SwitchHook), nint.Zero },
        { typeof(MoveSizeHook), nint.Zero },
        { typeof(MenuHook), nint.Zero },
        { typeof(MenuPopupHook), nint.Zero },
        { typeof(CaptureHook), nint.Zero },
        { typeof(ContextHelpHook), nint.Zero },
        { typeof(DragDropHook), nint.Zero },
        { typeof(DialogHook), nint.Zero },
        { typeof(ScrollingHook), nint.Zero },
        { typeof(DesktopSwitchHook), nint.Zero },
        { typeof(ConsoleHook), nint.Zero },
        { typeof(UiaHook), nint.Zero },
        { typeof(ObjectHook), nint.Zero }
    };
    private readonly LowLevelKeyboardProc _keyboardProc;
    private readonly LowLevelMouseProc _mouseProc;
    private readonly WinEventDelegate _winEvents;
    private static nint
        _keyboardHookID = nint.Zero,
        _mouseHookID = nint.Zero;
    /// <summary>
    /// Raised for low-level Windows events captured via WinEventHook or other system hooks.
    /// Includes window changes, UI Automation notifications, and accessibility events.
    /// </summary>
    /// <param name="name">Windows hook</param>
    public Winhooks()
    {
        _keyboardProc = KeyboardHookCallback;
        _mouseProc = MouseHookCallback;
        _winEvents = WinEventCallback;
        GC.KeepAlive(this);
    }
    /// <summary>
    /// Start or stop the specified hooks in Hookbase.
    /// </summary>
    public void StartOrStopHooks(params HookBase[] hooks)
    {
        foreach (var hook in hooks)
            StartOrStopHook(hook);
    }
    /// <summary>
    /// Hook files and recreate file watcher with specified parameters.
    /// </summary>
    public void HookFiles(FileHook hook)
    {
        foreach (var p in hook.Paths.Where(p => Directory.Exists(p) || File.Exists(p)))
            _files.Add(Path.GetFullPath(p));
        RecreateFileWatcher(hook.Filter, hook.Filters, hook.IncludeCreated, hook.IncludeChanged,
            hook.IncludeDeleted, hook.IncludeRenamed, hook.IncludeError, hook.IncludeSubdirectories);
    }
    /// <summary>
    /// Unhook files and recreate file watcher.
    /// </summary>
    public void UnhookFiles(params string[] paths)
    {
        foreach (var p in paths)
            _files.Remove(Path.GetFullPath(p));
        RecreateFileWatcher();
    }
    /// <summary>
    /// Clear all hooked files and recreate file watcher.
    /// </summary>
    public void ClearHookedFiles()
    {
        _files.Clear();
        RecreateFileWatcher();
    }
    /// <summary>
    /// Filter specified virtual keys.
    /// </summary>
    public void FilterKeys(params VirtualKey[] keys)
    {
        foreach (var key in keys)
            _filteredKeys.Add(key);
    }
    /// <summary>
    /// Unfilter specified virtual keys.
    /// </summary>
    public void UnfilterKeys(params VirtualKey[] keys)
    {
        foreach (var key in keys)
            _filteredKeys.Remove(key);
    }
    /// <summary>
    /// Filter specified mouse types.
    /// </summary>
    public void FilterMice(params MouseType[] mice)
    {
        foreach (var mouse in mice)
            _filteredMice.Add(mouse);
    }
    /// <summary>
    /// Unfilter specified mouse types.
    /// </summary>
    public void UnfilterMice(params MouseType[] mice)
    {
        foreach (var mouse in mice)
            _filteredMice.Remove(mouse);
    }
    /// <summary>
    /// Filter or unfilter all valid keys or all mouse types.
    /// </summary>
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
    /// <summary>
    /// Dispose and unhook all hooks and file watchers.
    /// </summary>
    public void Dispose()
    {
        foreach (var w in _watchers.Values)
            w?.Dispose();
        _files.Clear();
        _watchers.Clear();
        _fileWatcher?.Dispose();
        GC.SuppressFinalize(this);
    }
}