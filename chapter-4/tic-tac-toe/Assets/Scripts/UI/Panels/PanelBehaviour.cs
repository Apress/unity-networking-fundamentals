using UnityEngine;

public abstract class PanelBehaviour : MonoBehaviour
{
    private PanelController _controller;
    private CanvasGroup _canvasGroup;

    protected PanelController Controller
    {
        get
        {
            if (_controller == null)
            {
                _controller = transform.GetComponentInParent<PanelController>();
            }
            return _controller;
        }
    }

    public bool IsVisible
    {
        get { return CanvasGroup.alpha > 0; }
        set
        {
            CanvasGroup.alpha = value ? 1 : 0;
            CanvasGroup.blocksRaycasts = value;
        }
    }

    private CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            return _canvasGroup;
        }
    }

}
