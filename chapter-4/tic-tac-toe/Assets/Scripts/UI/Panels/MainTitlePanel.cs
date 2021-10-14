using System;
using UnityEngine.UI;

public class MainTitlePanel : PanelBehaviour
{
    public Button _startServer;
    public Button _joinServer;
    public TMPro.TMP_InputField _serverAddress;

    public event EventHandler JoinServerClicked;

    public event EventHandler StartServerClicked;

    public string ServerAddress
    {
        get { return _serverAddress.text; }
    }

    private void Awake()
    {
        _startServer.onClick.AddListener(delegate
        {
            Controller.ShowPanel(PanelType.StartServer);
            StartServerClicked?.Invoke(this, EventArgs.Empty);
        });

        _joinServer.onClick.AddListener(delegate
        {
            Controller.ShowPanel(PanelType.JoinServer);
            JoinServerClicked?.Invoke(this, EventArgs.Empty);
        });
    }
}
