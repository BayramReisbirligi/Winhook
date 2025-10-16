using ReisProduction.Windelay.Utilities.Enums;
namespace ReisProduction.Winhooks.Models;
public partial class Winhooks
{
    /// <summary>
    /// How to handle delays for mouse hold (Down...Up) events.
    /// </summary>
    public DelayType MouseHoldDelayType { get; set; } = DelayType.TaskDelay;
    /// <summary>
    /// How to handle delays for key hold (Down...Up) events.
    /// </summary>
    public DelayType KeyHoldDelayType { get; set; } = DelayType.TaskDelay;
    /// <summary>
    /// Allow mouse click event to be raised on double click.
    /// </summary>
    public bool AllowClickOnDoubleClick { get; set; } = true;
    /// <summary>
    /// Allow key press event to be raised on double press.
    /// </summary>
    public bool AllowPressOnDoublePress { get; set; } = true;
    /// <summary>
    /// Accept injected keyboard input for hook.
    /// </summary>
    public bool AcceptInjectedKeyboard { get; set; } = true;
    /// <summary>
    /// Include subdirectories when watching files.
    /// </summary>
    public bool IncloudeSubdirectories { get; set; } = false;
    /// <summary>
    /// Enable or disable raising events for file watcher.
    /// </summary>
    public bool EnableRaisingEvents { get; set; } = true;
    /// <summary>
    /// Accept injected mouse input for hook.
    /// </summary>
    public bool AcceptInjectedMouse { get; set; } = true;
    /// <summary>
    /// Accept None input type for hook.
    /// </summary>
    public bool AcceptNoneInput { get; set; } = false;
    /// <summary>
    /// Accept system key down events (e.g., Alt key).
    /// </summary>
    public bool AcceptSYSDown { get; set; } = true;
    /// <summary>
    /// Accept system key up events (e.g., Alt key).
    /// </summary>
    public bool AcceptSYSUp { get; set; } = true;
    /// <summary>
    /// Allow key press event to be raised.
    /// </summary>
    public bool AllowKeyPress { get; set; } = false;
    /// <summary>
    /// Allow key hold (Down...Up) event to be raised.
    /// </summary>
    public bool AllowKeyHold { get; set; } = false;
    /// <summary>
    /// Allow mouse click event to be raised.
    /// </summary>
    public bool AllowClick { get; set; } = false;
    /// <summary>
    /// Allow mouse hold (Down...Up) event to be raised.
    /// </summary>
    public bool AllowHold { get; set; } = false;
    /// <summary>
    /// Double click threshold in milliseconds.
    /// </summary>
    public int DoubleClickThresholdMs { get; set; } = 250;
    /// <summary>
    /// Double press threshold in milliseconds.
    /// </summary>
    public int DoublePressThresholdMs { get; set; } = 250;
    /// <summary>
    /// Movement threshold in pixels to raise mouse move event.
    /// </summary>
    public int MovementThreshold { get; set; } = 1;
    /// <summary>
    /// Key hold interval in milliseconds.
    /// </summary>
    public int KeyHoldIntervalMs { get; set; } = 50;
    /// <summary>
    /// Mouse Hold interval in milliseconds.
    /// </summary>
    public int HoldIntervalMs { get; set; } = 50;
    /// <summary>
    /// Mouse move event threshold in milliseconds.
    /// Default is calculated as 200ms divided by the number of processors,
    /// Minimum is 25ms and maximum is 100ms.
    /// </summary>
    public int MoveThresholdMs { get; set; } =
        Math.Clamp(200 / Environment.ProcessorCount, 25, 100);
}