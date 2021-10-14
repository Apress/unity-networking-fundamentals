using System.Collections;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script to access the 5-day forecast from OpenWeatherMap.
/// </summary>
public class OpenWeatherMapAPI : MonoBehaviour
{
    /// <summary>
    /// The base URL used to access the RESTful end-point that gets the 5-day forecast.
    /// It can be used to return 1-16 days.
    /// </summary>
    private static readonly string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/forecast/daily?q={0}&cnt=5&appid={1}";

    /// <summary>
    /// The key that allows access to the OpenWeatherMap API
    /// </summary>
    [Tooltip("The key that allows access to the OpenWeatherMap API")]
    public string apiKey;

    public ResponseContainer Response { get; private set; }

    /// <summary>
    /// Get the forcast for the given city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator GetForecast(string city)
    {
        Response = null;
        string urlEncodedCity = HttpUtility.UrlEncode(city);
        string url = string.Format(ApiBaseUrl, urlEncodedCity, apiKey);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            var json = webRequest.downloadHandler.text;
            Response = JsonUtility.FromJson<ResponseContainer>(json);
        }
        else
        {
            Debug.Log(webRequest.error);
        }
    }
}
    