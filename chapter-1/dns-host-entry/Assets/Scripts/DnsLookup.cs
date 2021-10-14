using System.Net;
using UnityEngine;

public class DnsLookup : MonoBehaviour
{
    public string _url = "www.google.com";

    void Start()
    {
        var addresses = Dns.GetHostAddresses(_url);
        foreach (var address in addresses)
        {
            print(address);
        }
    }
}
