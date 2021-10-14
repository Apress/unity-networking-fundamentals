using System;

[Serializable]
public class ResponseContainer
{
    public string cod;
    public float message;
    public int cnt;
    public ResponseItem[] list;
}