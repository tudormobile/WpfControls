using System.Runtime.InteropServices;

namespace Tudormobile.Wpf.Helpers;

/// <summary>
/// Help methods for Mouse operations.
/// </summary>
internal static partial class MouseHelpers
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public extern static bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);


    // Define the TRACKMOUSEEVENT structure
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENT
    {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;
    }

    // Define the flags for dwFlags
    public const uint TME_HOVER = 0x00000001;
    public const uint TME_LEAVE = 0x00000002;
    public const uint TME_NONCLIENT = 0x00000010;
    public const uint TME_QUERY = 0x40000000;
    public const uint TME_CANCEL = 0x80000000;
}
