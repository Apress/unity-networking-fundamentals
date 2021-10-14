using System.Text;

/// <summary>
/// Byte array encoding extensions.
/// </summary>
public static class ByteArrayEncodingExtensions
{
    /// <summary>
    /// Convert the given buffer to ASCII text.
    /// </summary>
    /// <param name="array">Byte array</param>
    /// <returns>The ASCII text contained in the buffer</returns>
    public static string ToAscii(this byte[] array)
    {
        return Encoding.ASCII.GetString(array);
    }
}
