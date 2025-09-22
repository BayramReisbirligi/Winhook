using ReisProduction.Winhook.Utilities.Enums;
using System.Runtime.CompilerServices;
using System.Management;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class Winhook
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnProcessStarted(object __, EventArrivedEventArgs e) => ProcessStarted.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnProcessStopped(object __, EventArrivedEventArgs e) => ProcessStopped.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnServiceStarted(object __, EventArrivedEventArgs e) => ServiceStarted.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnServiceStopped(object __, EventArrivedEventArgs e) => ServiceStopped.Invoke(e);
    private void OnThreadStarted(object __, EventArrivedEventArgs e) => ThreadStarted.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnThreadStopped(object __, EventArrivedEventArgs e) => ThreadStopped.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnModuleLoaded(object __, EventArrivedEventArgs e) => ModuleLoaded.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnModuleUnloaded(object __, EventArrivedEventArgs e) => ModuleUnloaded.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnSessionChanged(object __, EventArrivedEventArgs e) => SessionChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnDeviceChanged(object __, EventArrivedEventArgs e) => DeviceChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnVolumeChanged(object __, EventArrivedEventArgs e) => VolumeChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnPowerChanged(object __, EventArrivedEventArgs e) => PowerChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnSystemConfigChanged(object __, EventArrivedEventArgs e) => SystemConfigChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTimeChanged(object __, EventArrivedEventArgs e) => TimeChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnSystemTrace(object __, EventArrivedEventArgs e) => SystemTrace.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnIP4RouteChanged(object __, EventArrivedEventArgs e) => IP4RouteChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnIP6RouteChanged(object __, EventArrivedEventArgs e) => IP6RouteChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnNetworkAdapterConfigChanged(object __, EventArrivedEventArgs e) => NetworkAdapterConfigChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnProcessTrace(object __, EventArrivedEventArgs e) => ProcessTrace.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnProcessTraceStarted(object __, EventArrivedEventArgs e) => ProcessTraceStarted.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnProcessTraceStopped(object __, EventArrivedEventArgs e) => ProcessTraceStopped.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnThreadTrace(object __, EventArrivedEventArgs e) => ThreadTrace.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnModuleTrace(object __, EventArrivedEventArgs e) => VolumeChanged.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnBatchJobStarted(object __, EventArrivedEventArgs e) => BatchJobStarted.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnBatchJobStopped(object __, EventArrivedEventArgs e) => BatchJobStopped.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowSound(nint hWnd) => WindowSound(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowAlert(nint hWnd) => WindowAlert(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowForegroundChanged(nint hWnd) => WindowForegroundChanged(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMenuStart(nint hWnd) => WindowMenuStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMenuEnd(nint hWnd) => WindowMenuEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMenuPopupStart(nint hWnd) => WindowMenuPopupStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMenuPopupEnd(nint hWnd) => WindowMenuPopupEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowCaptureStart(nint hWnd) => WindowCaptureStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowCaptureEnd(nint hWnd) => WindowCaptureEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMoveSizeStart(nint hWnd) => WindowMoveSizeStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMoveSizeEnd(nint hWnd) => WindowMoveSizeEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowContextHelpStart(nint hWnd) => WindowContextHelpStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowContextHelpEnd(nint hWnd) => WindowContextHelpEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowDragDropStart(nint hWnd) => WindowDragDropStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowDragDropEnd(nint hWnd) => WindowDragDropEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowDialogStart(nint hWnd) => WindowDialogStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowDialogEnd(nint hWnd) => WindowDialogEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowScrollingStart(nint hWnd) => WindowScrollingStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowScrollingEnd(nint hWnd) => WindowScrollingEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowSwitchStart(nint hWnd) => WindowSwitchStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowSwitchEnd(nint hWnd) => WindowSwitchEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMinimizeStart(nint hWnd) => WindowMinimizeStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowMinimizeEnd(nint hWnd) => WindowMinimizeEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnWindowDesktopSwitch(nint hWnd) => WindowDesktopSwitch(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleCaret(nint hWnd) => ConsoleCaret(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleUpdateRegion(nint hWnd) => ConsoleUpdateRegion(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleUpdateSimple(nint hWnd) => ConsoleUpdateSimple(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleUpdateScroll(nint hWnd) => ConsoleUpdateScroll(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleLayout(nint hWnd) => ConsoleLayout(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleStartApplication(nint hWnd) => ConsoleStartApplication(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnConsoleEndApplication(nint hWnd) => ConsoleEndApplication(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnUiaPropertyChange(nint hWnd) => UiaPropertyChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnUiaPatternChange(nint hWnd) => UiaPatternChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnUiaStructureChange(nint hWnd) => UiaStructureChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnUiaEventIdStart(nint hWnd) => UiaEventIdStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnUiaEventIdEnd(nint hWnd) => UiaEventIdEnd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectCreate(nint hWnd) => ObjectCreate(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDestroy(nint hWnd) => ObjectDestroy(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectShow(nint hWnd) => ObjectShow(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectHide(nint hWnd) => ObjectHide(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectReorder(nint hWnd) => ObjectReorder(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectFocus(nint hWnd) => ObjectFocus(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectSelection(nint hWnd) => ObjectSelection(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectSelectionAdd(nint hWnd) => ObjectSelectionAdd(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectSelectionRemove(nint hWnd) => ObjectSelectionRemove(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectSelectionWithin(nint hWnd) => ObjectSelectionWithin(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectStateChange(nint hWnd) => ObjectStateChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectLocationChange(nint hWnd) => ObjectLocationChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectNameChange(nint hWnd) => ObjectNameChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDescriptionChange(nint hWnd) => ObjectDescriptionChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectValueChange(nint hWnd) => ObjectValueChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectParentChange(nint hWnd) => ObjectParentChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectHelpChange(nint hWnd) => ObjectHelpChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDefActionChange(nint hWnd) => ObjectDefActionChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectAcceleratorChange(nint hWnd) => ObjectAcceleratorChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectInvoked(nint hWnd) => ObjectInvoked(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectTextSelectionChanged(nint hWnd) => ObjectTextSelectionChanged(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectContentScrolled(nint hWnd) => ObjectContentScrolled(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectArrangementPreview(nint hWnd) => ObjectArrangementPreview(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectCloaked(nint hWnd) => ObjectCloaked(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectUncloaked(nint hWnd) => ObjectUncloaked(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectLiveRegionChanged(nint hWnd) => ObjectLiveRegionChanged(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectHostedObjectsInvalidated(nint hWnd) => ObjectHostedObjectsInvalidated(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragStart(nint hWnd) => ObjectDragStart(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragCancel(nint hWnd) => ObjectDragCancel(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragComplete(nint hWnd) => ObjectDragComplete(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragEnter(nint hWnd) => ObjectDragEnter(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragLeave(nint hWnd) => ObjectDragLeave(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectDragDropped(nint hWnd) => ObjectDragDropped(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectImeShow(nint hWnd) => ObjectImeShow(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectImeHide(nint hWnd) => ObjectImeHide(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectImeChange(nint hWnd) => ObjectImeChange(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnObjectTextEditConversionTargetChanged(nint hWnd) => ObjectTextEditConversionTargetChanged(hWnd);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFileCreated(object __, FileSystemEventArgs e) => FileCreated.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFileChanged(object __, FileSystemEventArgs e) => FileCreated.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFileDeleted(object __, FileSystemEventArgs e) => FileCreated.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFileRenamed(object __, RenamedEventArgs e) => FileCreated.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFileWatcherError(object __, ErrorEventArgs e) => FileWatcherError.Invoke(e);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyDown(VirtualKey key)
    {
        InputDown?.Invoke((InputType)key);
        KeyDown?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyHold(VirtualKey key)
    {
        InputHold?.Invoke((InputType)key);
        KeyHold?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyPress(VirtualKey key)
    {
        InputPress?.Invoke((InputType)key);
        KeyPress?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyDoublePress(VirtualKey key)
    {
        InputDoublePress?.Invoke((InputType)key);
        KeyDoublePress?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyUp(VirtualKey key)
    {
        InputUp?.Invoke((InputType)key);
        KeyUp?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseDown(ButtonType button)
    {
        InputDown?.Invoke((InputType)button);
        MouseDown?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseClick(ButtonType button)
    {
        InputPress?.Invoke((InputType)button);
        MouseClick?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseHold(ButtonType button)
    {
        InputPress?.Invoke((InputType)button);
        MouseHold?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseDoubleClick(ButtonType button)
    {
        InputDoublePress?.Invoke((InputType)button);
        MouseDoubleClick?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseUp(ButtonType button)
    {
        InputUp?.Invoke((InputType)button);
        MouseUp?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseScroll(ScrollType scroll)
    {
        MouseScroll?.Invoke(scroll);
        switch (scroll)
        {
            case ScrollType.MouseScrollUp:
                MouseScrollUp?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollDown:
                MouseScrollDown?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollLeft:
                MouseScrollLeft?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollRight:
                MouseScrollRight?.Invoke(scroll);
                break;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseMove(int deltaX, int deltaY)
    {
        MouseMove?.Invoke(deltaX, deltaY);
        if (deltaX is not 0)
        {
            MouseMoveHorizontal?.Invoke(deltaX, deltaY);
            if (deltaX > 0)
                MouseMoveRight?.Invoke(deltaX, deltaY);
            else
                MouseMoveLeft?.Invoke(deltaX, deltaY);
        }
        if (deltaY is not 0)
        {
            MouseMoveVertical?.Invoke(deltaX, deltaY);
            if (deltaY > 0)
                MouseMoveDown?.Invoke(deltaX, deltaY);
            else
                MouseMoveUp?.Invoke(deltaX, deltaY);
        }
    }
}