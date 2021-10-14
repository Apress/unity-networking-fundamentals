using System.Runtime.InteropServices;

/// <summary>
/// Basic message structure. For your structures you should make them as small
/// as you possibly can.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Message
{
    /// <summary>
    /// Message version number.
    /// </summary>
    public int Version;

    /// <summary>
    /// Message text. Note that it has a MarshalAs attribute so that the
    /// marshaller knows the maximum size for this field. This is required for
    /// strings.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Text;
}
