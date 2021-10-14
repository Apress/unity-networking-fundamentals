using System;
using UnityEngine.UI;

public class StartServerPanel : PanelBehaviour
{
    public Button _cancelButton;

    public event EventHandler CancelClicked;

    private void Awake()
    {
        _cancelButton.onClick.AddListener(delegate
        {
            Controller.ShowPanel(PanelType.Title);
            CancelClicked?.Invoke(this, EventArgs.Empty);
        });
    }
}
