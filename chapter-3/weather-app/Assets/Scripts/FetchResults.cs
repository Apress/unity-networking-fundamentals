using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fetch the weather forecast.
/// </summary>
public class FetchResults : MonoBehaviour
{
    /// <summary>
    /// The name of the default icon.
    /// </summary>
    private static readonly string DefaultIcon = "01d";

    private bool isRunningQuery;
    private Button button;
    private OpenWeatherMapAPI api;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public GameObject loadingMessage;
    public TMP_InputField cityInputField;
    public DayCard[] dayCards;

    public Sprite[] spriteIcons;

    public CanvasGroup panel;

    /// <summary>
    /// Set up the component.
    /// </summary>
    void Awake()
    {
        button = GetComponent<Button>();
        api = GetComponent<OpenWeatherMapAPI>();

        // Create the dictionary that maps the name of the sprite to its image
        foreach (Sprite s in spriteIcons)
        {
            sprites[s.name] = s;
        }

        // Add a listener to the button's click event to fetch the data from the
        // remote server
        button.onClick.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(cityInputField.text.Trim()) && !isRunningQuery)
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
        isRunningQuery = true;
        panel.alpha = 0;
        loadingMessage.SetActive(true);
        yield return api.GetForecast(query);
        loadingMessage.SetActive(false);
        isRunningQuery = false;

        if (api.Response != null)
        {
            FillDays(api.Response);
            panel.alpha = 1;
        }
    }

    /// <summary>
    /// Fill in the day cards with the newly returned data.
    /// </summary>
    /// <param name="response">Data received from the server</param>
    private void FillDays(ResponseContainer response)
    {
        panel.alpha = 1;

        for (int i = 0; i < dayCards.Length; i++)
        {
            var icon = response.list[i].weather[0].icon;
            if (!sprites.ContainsKey(icon))
            {
                icon = DefaultIcon;
            }

            Sprite sprite = sprites[icon];
            DayCardModel day = new DayCardModel(response.list[i], sprite);
            DayCard dayCard = dayCards[i];
            dayCard.SetModel(day);
        }
    }
}
