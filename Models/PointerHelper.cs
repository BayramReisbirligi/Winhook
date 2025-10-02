#if WINUI || WINDOWS_APP || WINRT
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
namespace ReisProduction.Winhook.Models;
public static class PointerHelper
{
    /// <inheritdoc cref="Windows.UI.Input.PointerPoint"/>
    public static PointerPoint PointerPoint { get; set; } = null!;
    /// <inheritdoc cref="PointerPoint.Properties"/>
    public static PointerPointProperties PointerProperties => PointerPoint.Properties;
    /// <inheritdoc cref="PointerPoint.Position"/>
    public static Point Position => PointerPoint.Position;
    /// <inheritdoc cref="PointerPoint.PointerDevice"/>
    public static PointerDevice PointerDevice => PointerPoint.PointerDevice;
    /// <inheritdoc cref="PointerDevice.PointerDeviceType"/>
    public static PointerDeviceType DeviceType => PointerDevice.PointerDeviceType;
    public static uint PointerId => PointerPoint.PointerId;
    /// <inheritdoc cref="PointerPoint.FrameId"/>
    public static uint FrameId => PointerPoint.FrameId;
    /// <inheritdoc cref="PointerPoint.Timestamp"/>
    public static ulong Timestamp => PointerPoint.Timestamp;
    /// <inheritdoc cref="PointerPointProperties.Twist"/>
    public static float Twist => PointerProperties.Twist;
    /// <inheritdoc cref="PointerPointProperties.XTilt"/>
    public static float XTilt => PointerProperties.XTilt;
    /// <inheritdoc cref="PointerPointProperties.YTilt"/>
    public static float YTilt => PointerProperties.YTilt;
    /// <summary>
    /// Gets the tilt of the pointer as a tuple (XTilt, YTilt).
    /// </summary>
    public static (float XTilt, float YTilt) GetTilt() => (PointerProperties.XTilt, PointerProperties.YTilt);
    /// <inheritdoc cref="PointerPointProperties.Pressure"/>
    public static float Pressure => PointerProperties.Pressure;
    /// <summary>
    /// Standard wheel delta used by Windows for one mouse wheel detent (default = 120).
    /// </summary>
    public static double WhellDelta { get; set; } = 120.0;
    /// <inheritdoc cref="PointerPointProperties.IsLeftButtonPressed"/>
    public static bool IsLeftButtonPressed => PointerProperties.IsLeftButtonPressed;
    /// <inheritdoc cref="PointerPointProperties.IsRightButtonPressed"/>
    public static bool IsRightButtonPressed => PointerProperties.IsRightButtonPressed;
    /// <inheritdoc cref="PointerPointProperties.IsMiddleButtonPressed"/>
    public static bool IsMiddleButtonPressed => PointerProperties.IsMiddleButtonPressed;
    /// <inheritdoc cref="PointerPointProperties.IsXButton1Pressed"/>
    public static bool IsXButton1Pressed => PointerProperties.IsXButton1Pressed;
    /// <inheritdoc cref="PointerPointProperties.IsXButton2Pressed"/>
    public static bool IsXButton2Pressed => PointerProperties.IsXButton2Pressed;
    /// <inheritdoc cref="PointerPointProperties.IsBarrelButtonPressed"/>
    public static bool IsBarrelButtonPressed => PointerProperties.IsBarrelButtonPressed;
    /// <inheritdoc cref="PointerPointProperties.IsEraser"/>
    public static bool IsEraser => PointerProperties.IsEraser;
    /// <inheritdoc cref="PointerPointProperties.IsInRange"/>
    public static bool IsInRange => PointerProperties.IsInRange;
    /// <inheritdoc cref="PointerPointProperties.IsInverted"/>
    public static bool IsInverted => PointerProperties.IsInverted;
    /// <inheritdoc cref="PointerPointProperties.IsCanceled"/>
    public static bool IsCanceled => PointerProperties.IsCanceled;
    /// <inheritdoc cref="PointerPointProperties.TouchConfidence"/>
    public static bool TouchConfidence => PointerProperties.TouchConfidence;
    /// <inheritdoc cref="PointerPointProperties.ZDistance"/>
    public static float? ZDistance => PointerProperties.ZDistance;
    /// <inheritdoc cref="PointerPointProperties.ContactRect"/>
    public static Rect ContactRect => PointerProperties.ContactRect;
    /// <inheritdoc cref="PointerPointProperties.ContactRectRaw"/>
    public static Rect ContactRectRaw => PointerProperties.ContactRectRaw;
    /// <summary>
    /// Gets the mouse wheel delta and orientation.
    /// </summary>
    /// <inheritdoc cref="PointerPointProperties.MouseWheelDelta" path="/summary"/>
    /// <inheritdoc cref="PointerPointProperties.IsHorizontalMouseWheel" path="/summary"/>
    /// <returns>(Delta, IsHorizontal)</returns>
    public static (int Delta, bool IsHorizontal) GetMouseWheelDelta()
    {
        var properties = PointerProperties;
        if (properties is null) return (0, false);
        return (properties.MouseWheelDelta, properties.IsHorizontalMouseWheel);
    }
    /// <inheritdoc cref="PointerPointProperties.MouseWheelDelta"/>
    public static int GetVerticalWheelDelta()
    {
        var (delta, isHorizontal) = GetMouseWheelDelta();
        return isHorizontal ? 0 : delta;
    }
    /// <inheritdoc cref="PointerPointProperties.MouseWheelDelta"/>
    public static int GetHorizontalWheelDelta()
    {
        var (delta, isHorizontal) = GetMouseWheelDelta();
        return isHorizontal ? delta : 0;
    }
    /// <summary>
    /// Get the normalized scroll delta, where 1.0 represents one standard scroll step.
    /// </summary>
    public static double GetNormalizedScrollDelta()
    {
        var (delta, _) = GetMouseWheelDelta();
        return delta / WhellDelta;
    }
    /// <summary>
    /// Calculates the Euclidean distance between two PointerPoint positions.
    /// </summary>
    public static double CalculateDistance(PointerPoint point1, PointerPoint point2)
    {
        double deltaX = point2.Position.X - point1.Position.X,
               deltaY = point2.Position.Y - point1.Position.Y;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
#endif