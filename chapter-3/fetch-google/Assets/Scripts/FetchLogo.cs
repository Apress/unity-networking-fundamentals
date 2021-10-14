using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FetchLogo : MonoBehaviour
{
    IEnumerator Start()
    {
        string url = "https://upload.wikimedia.org/wikipedia/commons/8/8a/Official_unity_logo.png";
        var request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var textureHandler = request.downloadHandler as DownloadHandlerTexture;
            Texture2D texture = textureHandler.texture;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            var rect = new Rect(0, 0, texture.width, texture.height);
            spriteRenderer.sprite = Sprite.Create(texture, rect, Vector2.zero);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
