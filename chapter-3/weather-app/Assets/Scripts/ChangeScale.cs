using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change scale from celcius to fahrenheit and vice versa.
/// </summary>
public class ChangeScale : MonoBehaviour
{
    private bool _showMetric = true;

    public Sprite _celcius;
    public Sprite _fahrenheit;

    public Image _image;
    public DayCard[] _days;
    
    void Awake()
    {
        // Attach a listener to the click event of the button this script is 
        // attached. When clicked, the button will negate the show metric flag
        // and inform all the DayCard instances to show the correct temperature
        // scale.
        var button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            _showMetric = !_showMetric;
            foreach (var day in _days)
            {
                day.ShowMetric(_showMetric);
            }

            _image.sprite = _showMetric ? _celcius : _fahrenheit;
        });
    }
}
