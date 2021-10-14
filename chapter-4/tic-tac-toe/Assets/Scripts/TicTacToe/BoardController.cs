using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    private bool _cellButtonsEnabled;

    private bool _actionButtonsEnabled;

    public CellClick[] _cells;

    public Sprite _blankShape;

    public Sprite[] _shapes;

    public Button _playAgain;

    public Button _returnToTitle;

    public CanvasGroup _winnersPodium;

    public Image _winnerSlot;

    public TMPro.TextMeshProUGUI _podiumTop;

    public TMPro.TextMeshProUGUI _podiumBottom;

    public event EventHandler<CellClickedEventArgs> CellClicked;

    public event EventHandler PlayAgainClicked;

    public event EventHandler ReturnToTitleClicked;

    void Start()
    {
        _actionButtonsEnabled = true;
        _cellButtonsEnabled = true;

        _playAgain.onClick.AddListener(delegate
        {
            if (_actionButtonsEnabled)
                PlayAgainClicked?.Invoke(this, EventArgs.Empty);
        });

        _returnToTitle.onClick.AddListener(delegate
        {
            if (_actionButtonsEnabled)
                ReturnToTitleClicked?.Invoke(this, EventArgs.Empty);
        });

        var count = 0;
        foreach (var cell in _cells)
        {
            var temp = count;
            cell.Clicked += (o, e) => 
            { 
                if (_cellButtonsEnabled)
                    CellClicked(this, new CellClickedEventArgs(temp));
            };
            count++;
        }
    }

    public void UpdateBoard(int[] cells)
    {
        for (var i = 0; i < _cells.Length; i++)
        {
            var cell = _cells[i];
            var cellIndex = cells[i] - 1;
            cell.SetShape(cellIndex < 0 ? _blankShape : _shapes[cellIndex], cellIndex < 0);
        }
    }

    public void ResetBoard(int[] cells)
    {
        UpdateBoard(cells);
        _winnersPodium.alpha = 0;
        _playAgain.interactable = false;
    }

    public void BoardWinner(int player)
    {
        var sprite = _shapes[player - 1];
        StartCoroutine(ShowPodium("Congratulations!", "Has Won!", sprite));
    }

    public void CatsGame()
    {
        StartCoroutine(ShowPodium("Sorry!", "Tied Game!", _blankShape));
    }

    private IEnumerator ShowPodium(string title, string subTitle, Sprite sprite)
    {
        _podiumTop.text = title;
        _podiumBottom.text = subTitle;

        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2);
        _winnersPodium.alpha = 1;
        _winnerSlot.sprite = sprite;
        _playAgain.interactable = true;
    }

    public void ToggleCellButtons(bool buttonsEnabled)
    {
        _cellButtonsEnabled = buttonsEnabled;
    }

    public void ToggleActionButtons(bool buttonsEnabled)
    {
        _actionButtonsEnabled = buttonsEnabled;
    }
}
