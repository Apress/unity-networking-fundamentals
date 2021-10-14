using System;
using System.Collections;
using System.Web;
using UnityEngine;

/// <summary>
/// Script to access the 5-day forecast from OpenWeatherMap.
/// </summary>
class OpenWeatherRestfulApiClient : MonoBehaviour
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

    /// <summary>
    /// Get the forcast for the given city
    /// </summary>
    /// <param name="city">City</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator GetForecast(string city, Action<ResponseContainer> onSuccess)
    {
        string urlEncodedCity = HttpUtility.UrlEncode(city);
        string url = string.Format(ApiBaseUrl, urlEncodedCity, apiKey);
        yield return RestfulHelper.Fetch(url, onSuccess, print);
    }
}
