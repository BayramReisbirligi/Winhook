namespace ReisProduction.Winhook.Utilities.Enums;
public enum MouseType : ushort
{
    None = 0x00,
    LeftButton = 0x01,
    RightButton = 0x02,
    MiddleButton = 0x04,
    XButton1 = 0x05,
    XButton2 = 0x06,
    MouseScrollLeft = 0xFF00,
    MouseScrollRight = 0xFF01,
    MouseScrollUp = 0xFF02,
    MouseScrollDown = 0xFF03,
}