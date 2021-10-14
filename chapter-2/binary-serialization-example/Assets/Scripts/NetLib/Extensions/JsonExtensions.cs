using System.Text;
using UnityEngine;

public static class JsonExtensions
{
    /// <summary>
    /// Convert an object to a JSON string encoded in a byte array.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="data">Object to serialize</param>
    /// <returns>Byte array</returns>
    public static byte[] ToJsonBinary<T>(this T data) where T: new()
    {
        var json = JsonUtility.ToJson(data);
        return Encoding.ASCII.GetBytes(json);
    }

    /// <summary>
    /// Convert the data stored in the byte array to an object instance.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="data">Byte array containing the object state</param>
    /// <returns>The object</returns>
    public static T FromJsonBinary<T>(this byte[] data) where T: new()
    {
        var json = Encoding.ASCII.GetString(data);
        return JsonUtility.FromJson<T>(json);
    }
}
