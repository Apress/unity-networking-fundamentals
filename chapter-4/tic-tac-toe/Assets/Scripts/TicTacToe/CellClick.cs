using System;
using UnityEngine;
using UnityEngine.UI;

public class CellClick : MonoBehaviour
{
    private bool _enabled = true;

    public event EventHandler Clicked;

    public Image _shape;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            if (_enabled) Clicked?.Invoke(this, EventArgs.Empty);
        });
    }

    public void SetShape(Sprite sprite, bool enabled = true)
    {
        _enabled = enabled;
        _shape.sprite = sprite;
    }

    public void Reset()
    {
        _enabled = true;
    }
}
