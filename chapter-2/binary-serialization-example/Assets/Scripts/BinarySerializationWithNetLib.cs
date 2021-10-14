using UnityEngine;

public class BinarySerializationWithNetLib : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Create an instance of the data to be serialized/ deserialized
        var data = new MyData
        {
            shield = 100,
            health = 50,
            name = "Sven The Destroyer",
            position = new Vector3(1, 2, 3)
        };

        Debug.Log($"Original: {data}");

        // Make a copy of the data
        byte[] bytes = data.ToArray();
        MyData copy = bytes.ToStruct<MyData>();

        Debug.Log($"Copy: {copy}");

        // And now some JSON
        // Make a copy of the data and serialize it to JSON and
        // back again
        byte[] jsonBytes = data.ToJsonBinary();
        MyData jsonCopy = jsonBytes.FromJsonBinary<MyData>();
        Debug.Log($"Json Copy: {jsonCopy}");
    }
}
