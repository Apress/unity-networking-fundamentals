using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
public class ConectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ConnectionPanelUI;
    public string ipAddress = "127.0.0.1";

    UNetTransport transport;


    public void StartHost()
    {
        ConnectionPanelUI.gameObject.SetActive(false);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(SpawnCharacter(), Quaternion.identity);
    }

    private void ApprovalCheck(byte[] connectionData, ulong ClientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
        callback(true, null, approve, SpawnCharacter(), Quaternion.identity);
    }

    public void Join()
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;
        ConnectionPanelUI.gameObject.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
        NetworkManager.Singleton.StartClient();
    }
    Vector3 SpawnCharacter()
    {

        return Vector3.zero;
    }

    // Update is called once per frame
    public void ChangeIpAddress(string newAddress)
    {
        ipAddress = newAddress;
    }
}
