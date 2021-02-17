
namespace VagabondLib.Win32
{
    /// <summary>
    /// Flags to use with the Set Window Position
    /// </summary>
    public enum SetWindowsPositionFlags : uint
    {
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOACTIVATE = 0x0010
    }
}
