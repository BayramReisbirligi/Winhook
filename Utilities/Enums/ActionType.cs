namespace ReisProduction.Winhook.Utilities.Enums;
public enum EventType : ushort
{
    None = 0x00,
    Foreground = 0x01,
    Minimize = 0x02,
    Switch = 0x03,
    MoveSize = 0x04,
    Menu = 0x05,
    MenuPopup = 0x06,
    Capture = 0x07,
    ContextHelp = 0x08,
    DragDrop = 0x09,
    Dialog = 0x0A,
    Scrolling = 0x0B,
    DesktopSwitch = 0x0C,
    Console = 0x0D,
    Uia = 0x0E,
    Object = 0x0F,
    Session = 0x10,
    Keyboard = 0x11,
    Mouse = 0x12
}