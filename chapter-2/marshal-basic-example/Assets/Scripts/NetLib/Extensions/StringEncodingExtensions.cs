using System.Text;

/// <summary>
/// String encoding extensions.
/// </summary>
public static class StringEncodingExtensions
{
    /// <summary>
    /// Convert the given text to an ASCII buffer.
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <returns>The ASCII buffer</returns>
    public static byte[] ToAsciiArray(this string message)
    {
        return Encoding.ASCII.GetBytes(message);
    }
}
