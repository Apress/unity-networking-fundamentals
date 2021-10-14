using UnityEngine;

/// <summary>
/// The panel controller handles the visibility of the given panel set.
/// </summary>
public class PanelController : MonoBehaviour
{
    /// <summary>
    /// The panels.
    /// </summary>
    [Tooltip("The panels")]
    public PanelBehaviour[] _panels;

    [Tooltip("The start panel")]
    public PanelType _startPanel = PanelType.Title;

    public T GetPanel<T>() where T : PanelBehaviour
    {
        foreach (var panel in _panels)
        {
            if (panel.GetType() == typeof(T))
            { return (T)panel; }
        }

        return default(T);
    }


    /// <summary>
    /// Show the given panel. All others will be hidden.
    /// </summary>
    /// <param name="panelType">Panel type</param>
    public void ShowPanel(PanelType panelType)
    {
        var panelIdx = (int)panelType;

        for (var i = 0; i < _panels.Length; i++)
        {
            _panels[i].IsVisible = panelIdx == i;
        }
    }

    /// <summary>
    /// Show the title and turn off all other panels.
    /// </summary>
    private void Awake()
    {
        ShowPanel(_startPanel);
    }
}
