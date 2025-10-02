using Windows.UI.Input;
namespace ReisProduction.Winhook.Models;
public class HookHelper
{
    /// <summary>
    /// Windows.UI.Input.PointerPoint instance
    /// </summary>
    public static PointerPoint PointerPoint { get; set; } = null!;
    public static void GetPointerPoint(int x, int y)
    {
        PointerPoint = PointerPoint.GetCurrentPoint(0, new ());
    }
}