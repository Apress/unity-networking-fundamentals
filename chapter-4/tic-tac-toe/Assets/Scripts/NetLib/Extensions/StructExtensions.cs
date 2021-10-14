using System.Runtime.InteropServices;

/// <summary>
/// Struct extensions to convert a struct to and from a byte array.
/// </summary>
public static class StructExtensions
{
    /// <summary>
    /// Re-create a structure from a previously marshalled byte array.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="data">Marshalled byte array</param>
    /// <returns>Re-created structure</returns>
    public static T ToStruct<T>(this byte[] data) where T: struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, ptr, size);

        var copyData = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return copyData;
    }

    /// <summary>
    /// Create a byte array from a struct.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="data">Structure</param>
    /// <returns>Byte array</returns>
    public static byte[] ToArray<T>(this T data) where T: struct
    {
        var size = Marshal.SizeOf(data);
        byte[] buf = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(data, ptr, true);
        Marshal.Copy(ptr, buf, 0, size);

        Marshal.FreeHGlobal(ptr);
        return buf;
    }
}
