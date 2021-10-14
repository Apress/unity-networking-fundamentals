using System;
using UnityEngine;

/// <summary>
/// This class is used to create a friendly data model for the DayCard script.
/// </summary>
class DayCardModel
{
    public float MinimumTemp { get; }

    public float MaximumTemp { get; }

    public string Day { get; }

    public string Date { get; }

    public Sprite Icon { get; }

    public string Header { get; }

    public string Details { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="item">Response item</param>
    /// <param name="icon">Sprite</param>
    public DayCardModel(ResponseItem item, Sprite icon)
    {
        MaximumTemp = item.temp.max;
        MinimumTemp = item.temp.min;

        var date = UnixTimeToDateTime(item.dt);
        Day = date.DayOfWeek.ToString();
        Date = $"{date.Month}/{date.Day}";
        Header = item.weather[0].main;
        Details = item.weather[0].description;
        Icon = icon;
    }

    /// <summary>
    /// Convert from UNIX time to date time.
    /// </summary>
    /// <param name="unixTime">Unix time (seconds since Epoch)</param>
    /// <returns>Date and time</returns>
    private DateTime UnixTimeToDateTime(long unixTime)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        time = time.AddSeconds(unixTime).ToLocalTime();
        return time;
    }
}