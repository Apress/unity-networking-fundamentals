using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The DayCard is used in the day card prefab to set the values on the card.
/// </summary>
public class DayCard : MonoBehaviour
{
    private static string CelciusFormat = "{0}°C";
    private static string FahrenheitFormat = "{0}°F";

    private bool _showMetric = true;
    private DayCardModel _model;

    public TextMeshProUGUI _day;
    public TextMeshProUGUI _date;
    public TextMeshProUGUI _temperature;
    public TextMeshProUGUI _minTemperature;
    public TextMeshProUGUI _header;
    public TextMeshProUGUI _detail;
    public Image _icon;

    /// <summary>
    /// Set the model.
    /// </summary>
    /// <param name="model">The day card model containing the daily weather report</param>
    internal void SetModel(DayCardModel model)
    {
        _model = model;
        Fill();
    }

    /// <summary>
    /// Show the metric temperature.
    /// </summary>
    /// <param name="showMetric">True if metric is to be shown. Pass false for US.</param>
    public void ShowMetric(bool showMetric)
    {
        if (_showMetric == showMetric)
        {
            // Early out. No point in double-setting.
            return;
        }

        // Set this before the model valid check. We always want to set this to keep
        // the state consistent.
        _showMetric = showMetric;

        if (_model == null)
        {
            return;
        }

        _temperature.text = ToHumanTemperature(_model.MaximumTemp);
        _minTemperature.text = ToHumanTemperature(_model.MinimumTemp);
    }

    /// <summary>
    /// Fill in the day card model.
    /// </summary>
    private void Fill()
    {
        if (_model == null)
        {
            return;
        }

        _day.text = _model.Day;
        _date.text = _model.Date;
        _header.text = _model.Header;
        _detail.text = _model.Details;
        _icon.sprite = _model.Icon;

        _temperature.text = ToHumanTemperature(_model.MaximumTemp);
        _minTemperature.text = ToHumanTemperature(_model.MinimumTemp);
    }

    /// <summary>
    /// Set the current temperature view based on the metric flag.
    /// </summary>
    /// <param name="kelvin">Temperature in Kelvin</param>
    /// <returns></returns>
    private string ToHumanTemperature(float kelvin)
    {
        if (_showMetric)
        {
            var c = Mathf.RoundToInt(kelvin - 273.15f);
            return string.Format(CelciusFormat, c);
        }
        else
        {
            var f = (kelvin * 9f / 5f) - 459.67f;
            return string.Format(FahrenheitFormat, Mathf.RoundToInt(f));
        }
    }
}
