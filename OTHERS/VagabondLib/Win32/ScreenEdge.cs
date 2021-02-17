
namespace VagabondLib.Win32
{
    /// <summary>
    /// Application Bar Edge
    /// </summary>
    public enum ScreenEdge : uint
    {
        /// <summary>
        /// Application Bar is defined to the left edge
        /// </summary>
        Left = 0,
        /// <summary>
        /// Application Bar is defined to the Top edge
        /// </summary>
        Top = 1,
        /// <summary>
        /// Application Bar is defined to the Right edge
        /// </summary>
        Right = 2,
        /// <summary>
        /// Application Bar is defined to the Bottom edge
        /// </summary>
        Bottom = 3,
        /// <summary>
        /// If this is set to None, we are probably not doing so hot...
        /// </summary>
        None = 0xFFFFFF
    }
}
