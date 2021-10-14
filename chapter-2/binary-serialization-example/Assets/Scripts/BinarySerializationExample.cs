using System.Runtime.InteropServices;
using UnityEngine;

public class BinarySerializationExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var data = new MyData
        {
            shield = 100,
            health = 50,
            name = "Sven The Destroyer",
            position = new Vector3(1, 2, 3)
        };

        Debug.Log($"Original: {data}");

        byte[] bytes = ToBytes(data);
        MyData copy = ToObject<MyData>(bytes);

        Debug.Log($"Copy: {copy}");
    }

    /// <summary>
    /// Deserialize an array of bytes and return an
    /// instance of object type T with the serialized data.
    /// </summary>
    /// <typeparam name="T">Class or Struct type to be created</typeparam>
    /// <param name="data">Array of bytes containing serialized data</param>
    /// <returns>An instance of object type T</returns>
    private T ToObject<T>(byte[] data)
    {
        // Create an area of memory to store the byte array and then copy
        // it to memory
        var size = Marshal.SizeOf(typeof(T));
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, ptr, size);

        // Using the PtrToStructure method, copy the bytes out into the
        // Message structure
        var copyData = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return copyData;
    }

    /// <summary>
    /// Serialize an object to an array of bytes.
    /// </summary>
    /// <param name="data">The object to be serialized</param>
    /// <returns>The serialized object as an array of bytes</returns>
    private byte[] ToBytes(object data)
    {
        // Create a pointer in memory and allocate the size of the structure
        var size = Marshal.SizeOf(data);
        byte[] buf = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        // Copy the structure to the newly created memory space and then
        // copy it to the byte buffer
        Marshal.StructureToPtr(data, ptr, true);
        Marshal.Copy(ptr, buf, 0, size);

        // Always free your pointers!
        Marshal.FreeHGlobal(ptr);
        return buf;
    }
}
