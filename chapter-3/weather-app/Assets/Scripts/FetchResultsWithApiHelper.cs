using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fetch the weather forecast.
/// </summary>
public class FetchResultsWithApiHelper : MonoBehaviour
{
    /// <summary>
    /// The name of the default icon.
    /// </summary>
    private static readonly string DefaultIcon = "01d";

    private bool runningQuery;
    private Button button;
    private OpenWeatherRestfulApiClient api;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public GameObject loadingMessage;
    public TMP_InputField cityInputField;
    public DayCard[] days;

    public Sprite[] spriteIcons;

    public CanvasGroup panel;

    /// <summary>
    /// Set up the component.
    /// </summary>
    void Awake()
    {
        button = GetComponent<Button>();
        api = GetComponent<OpenWeatherRestfulApiClient>();

        // Create the dictionary that maps the name of the sprite to its image
        foreach (var s in spriteIcons)
        {
            sprites[s.name] = s;
        }

        // Add a listener to the button's click event to fetch the data from the
        // remote server
        button.onClick.AddListener(delegate {
            if (!string.IsNullOrEmpty(cityInputField.text.Trim()) && !runningQuery)
            {
                StartCoroutine(FetchData(cityInputField.text));
            }
        });
    }

    /// <summary>
    /// Fetch the data from the remote server.
    /// </summary>
    /// <param name="query">Search critera</param>
    /// <returns></returns>
    private IEnumerator FetchData(string query)
    {
        runningQuery = true;
        panel.alpha = 0;
        loadingMessage.SetActive(true);
        yield return api.GetForecast(query, FillDays);
        loadingMessage.SetActive(false);
        runningQuery = false;
    }

    /// <summary>
    /// Fill in the day cards with the newly returned data.
    /// </summary>
    /// <param name="response">Data received from the server</param>
    private void FillDays(ResponseContainer response)
    {
        panel.alpha = 1;

        for (var i = 0; i < days.Length; i++)
        {
            var icon = response.list[i].weather[0].icon;
            if (!sprites.ContainsKey(icon))
            {
                icon = DefaultIcon;
            }

            var sprite = sprites[icon];
            var day = new DayCardModel(response.list[i], sprite);
            var dayCard = days[i];
            dayCard.SetModel(day);
        }
    }
}
