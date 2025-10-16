using ReisProduction.Winhooks.Utilities.Enums;
using System.Management;
using Windows.System;
namespace ReisProduction.Winhooks.Models;
public partial class Winhooks
{
    /// <summary>
    /// System.Management event watcher events
    /// </summary>
    public event Action<EventArrivedEventArgs>
        ProcessStarted = delegate { },
        ProcessStopped = delegate { },
        ServiceStarted = delegate { },
        ServiceStopped = delegate { },
        ThreadStarted = delegate { },
        ThreadStopped = delegate { },
        ModuleLoaded = delegate { },
        ModuleUnloaded = delegate { },
        SessionChanged = delegate { },
        DeviceChanged = delegate { },
        VolumeChanged = delegate { },
        PowerChanged = delegate { },
        SystemConfigChanged = delegate { },
        TimeChanged = delegate { },
        SystemTrace = delegate { },
        IP4RouteChanged = delegate { },
        IP6RouteChanged = delegate { },
        NetworkAdapterConfigChanged = delegate { },
        ProcessTrace = delegate { },
        ProcessTraceStarted = delegate { },
        ProcessTraceStopped = delegate { },
        ThreadTrace = delegate { },
        ModuleTrace = delegate { },
        BatchJobStarted = delegate { },
        BatchJobStopped = delegate { };
    /// <summary>
    /// SetWinEventHook event callbacks
    /// </summary>
    public event Action<nint>
        WindowSound = delegate { },
        WindowAlert = delegate { } ,
        WindowForegroundChanged = delegate { },
        WindowMenuStart = delegate { },
        WindowMenuEnd = delegate { },
        WindowMenuPopupStart = delegate { },
        WindowMenuPopupEnd = delegate { },
        WindowCaptureStart = delegate { },
        WindowCaptureEnd = delegate { },
        WindowMoveSizeStart = delegate { },
        WindowMoveSizeEnd = delegate { },
        WindowContextHelpStart = delegate { },
        WindowContextHelpEnd = delegate { },
        WindowDragDropStart = delegate { },
        WindowDragDropEnd = delegate { },
        WindowDialogStart = delegate { },
        WindowDialogEnd = delegate { },
        WindowScrollingStart = delegate { },
        WindowScrollingEnd = delegate { },
        WindowSwitchStart = delegate { },
        WindowSwitchEnd = delegate { },
        WindowMinimizeStart = delegate { },
        WindowMinimizeEnd = delegate { },
        WindowDesktopSwitch = delegate { },
        ConsoleCaret = delegate { },
        ConsoleUpdateRegion = delegate { },
        ConsoleUpdateSimple = delegate { },
        ConsoleUpdateScroll = delegate { },
        ConsoleLayout = delegate { },
        ConsoleStartApplication = delegate { },
        ConsoleEndApplication = delegate { },
        UiaPropertyChange = delegate { },
        UiaPatternChange = delegate { },
        UiaStructureChange = delegate { },
        UiaEventIdStart = delegate { },
        UiaEventIdEnd = delegate { },
        ObjectCreate = delegate { },
        ObjectDestroy = delegate { },
        ObjectShow = delegate { },
        ObjectHide = delegate { },
        ObjectReorder = delegate { },
        ObjectFocus = delegate { },
        ObjectSelection = delegate { },
        ObjectSelectionAdd = delegate { },
        ObjectSelectionRemove = delegate { },
        ObjectSelectionWithin = delegate { },
        ObjectStateChange = delegate { },
        ObjectLocationChange = delegate { },
        ObjectNameChange = delegate { },
        ObjectDescriptionChange = delegate { },
        ObjectValueChange = delegate { },
        ObjectParentChange = delegate { },
        ObjectHelpChange = delegate { },
        ObjectDefActionChange = delegate { },
        ObjectAcceleratorChange = delegate { },
        ObjectInvoked = delegate { },
        ObjectTextSelectionChanged = delegate { },
        ObjectContentScrolled = delegate { },
        ObjectArrangementPreview = delegate { },
        ObjectCloaked = delegate { },
        ObjectUncloaked = delegate { },
        ObjectLiveRegionChanged = delegate { },
        ObjectHostedObjectsInvalidated = delegate { },
        ObjectDragStart = delegate { },
        ObjectDragCancel = delegate { },
        ObjectDragComplete = delegate { },
        ObjectDragEnter = delegate { },
        ObjectDragLeave = delegate { },
        ObjectDragDropped = delegate { },
        ObjectImeShow = delegate { },
        ObjectImeHide = delegate { },
        ObjectImeChange = delegate { },
        ObjectTextEditConversionTargetChanged = delegate { };
    /// <summary>
    /// System.IO.FileSystemWatcher events
    /// </summary>
    public event Action<FileSystemEventArgs>
        FileCreated = delegate { },
        FileChanged = delegate { },
        FileDeleted = delegate { },
        FileRenamed = delegate { };
    /// <summary>
    /// System.IO.FileSystemWatcher error event
    /// </summary>
    public event Action<ErrorEventArgs>
        FileWatcherError = delegate { };
    /// <summary>
    /// LowLevel hook events
    /// </summary>
    public event Action<InputType>
        InputDown = delegate { },
        InputHold = delegate { },
        InputPress = delegate { },
        InputDoublePress = delegate { },
        InputUp = delegate { };
    /// <summary>
    /// LowLevel keyboard hook events
    /// </summary>
    public event Action<VirtualKey>
        KeyDown = delegate { },
        KeyHold = delegate { },
        KeyPress = delegate { },
        KeyDoublePress = delegate { },
        KeyUp = delegate { };
    /// <summary>
    /// LowLevel mouse hook events
    /// </summary>
    public event Action<ButtonType>
        MouseDown = delegate { },
        MouseClick = delegate { },
        MouseHold = delegate { },
        MouseDoubleClick = delegate { },
        MouseUp = delegate { };
    /// <summary>
    /// LowLevel mouse scroll event
    /// </summary>
    public event Action<ScrollType>
        MouseScroll = delegate { },
        MouseScrollLeft = delegate { },
        MouseScrollRight = delegate { },
        MouseScrollUp = delegate { },
        MouseScrollDown = delegate { };
    /// <summary>
    /// LowLevel mouse move events
    /// </summary>
    public event Action<int, int>
        MouseMove = delegate { },
        MouseMoveLeft = delegate { },
        MouseMoveRight = delegate { },
        MouseMoveUp = delegate { },
        MouseMoveDown = delegate { },
        MouseMoveHorizontal = delegate { },
        MouseMoveVertical = delegate { };
}