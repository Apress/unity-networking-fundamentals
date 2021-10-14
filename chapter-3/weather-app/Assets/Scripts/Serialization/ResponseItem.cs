using System;

[Serializable]
public class ResponseItem
{
    public long dt;
    public ResponseTemperature temp;
    public WeatherItem[] weather;
    public long sunrise;
    public long sunset;
}