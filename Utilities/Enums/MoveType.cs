namespace ReisProduction.Winhook.Utilities.Enums;
public enum MoveType : ushort
{
    None = 0x0000,
    MouseNavigateLeft = 0xFF04,
    MouseNavigateLeftSmooth = 0xFF05,
    MouseNavigateRight = 0xFF06,
    MouseNavigateRightSmooth = 0xFF07,
    MouseNavigateUp = 0xFF08,
    MouseNavigateUpSmooth = 0xFF09,
    MouseNavigateDown = 0xFF10,
    MouseNavigateDownSmooth = 0xFF11,
    MouseNavigateToXY = 0xFF12,
    MouseNavigateToXYSmooth = 0xFF13,
}