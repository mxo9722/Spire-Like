using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionUI : MonoBehaviour
{
    [SerializeField] private CardUI _cardUIPrefab;

    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _confirmSelectionButton;
    [SerializeField] private TMPro.TMP_Text _selectionText;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject _upgradeWrapper;
    [SerializeField] private CardUI _beforeUpgradeUI;
    [SerializeField] private CardUI _afterUpgradeUI;

    public bool Opened { get => _wrapper.activeSelf; }
    public bool WaitingForSelection { get; private set; } = false;

    public List<CardUI> CardUIs { get; private set; } = new();

    private int _selectionsNeededMin = 0;
    private int _selectionsNeededMax = 0;
    private List<Card> _selections = null;

    public void SetUp(List<Card> cardCollection)
    {
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(true);
        _selectionText.gameObject.SetActive(false);
        UpdateConfirmSelectionUI();

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card);
            CardUIs.Add(cardUI);
        }
    }

    public void SetUpSelection(List<Card> cardCollection, int minAmount, int maxAmount, string selectionText = "")
    {
        if (minAmount >= cardCollection.Count)
        {
            _selections = new(cardCollection);
            return;
        }

        WaitingForSelection = true;
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(false);
        _upgradeWrapper.SetActive(false);

        _selectionsNeededMin = minAmount;
        _selectionsNeededMax = maxAmount;
        _selections = new();
        _selectionText.gameObject.SetActive(true);

        UpdateConfirmSelectionUI();

        if (string.IsNullOrEmpty(selectionText))
        {
            string cardText = "card";
            if (_selectionsNeededMax > 1)
                cardText = "cards";

            if(_selectionsNeededMin == _selectionsNeededMax)
                _selectionText.text = "Select " + _selectionsNeededMax + " "+ cardText;
            else if(_selectionsNeededMin == 0)
                _selectionText.text = "Select up to " + _selectionsNeededMax + " "+ cardText;
            else
                _selectionText.text = "Select up between " + _selectionsNeededMin + " and " + _selectionsNeededMax + " " + cardText;
        }
        else
        {
            _selectionText.text = selectionText.Replace("X", _selectionsNeededMax.ToString());
        }


        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card, OnSelected, true);
            CardUIs.Add(cardUI);
        }
    }
    
    public void SetUpUpgradeChoice()
    {
        List<Card> cardCollection = RunSystem.Instance.Deck.Where(c => c.Upgrade != null).ToList();

        if (1 >= cardCollection.Count)
        {
            cardCollection.ForEach(c => RunSystem.Instance.UpgradeCard(c));
            return;
        }

        WaitingForSelection = true;
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(false);
        _confirmSelectionButton.gameObject.SetActive(false);
        _upgradeWrapper.SetActive(false);

        _selections = new();
        _selectionText.gameObject.SetActive(true);

        _selectionText.text = "Select a card to upgrade";

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card, OpenUpgradeUI, false);
            CardUIs.Add(cardUI);
        }
    }

    public void Close()
    {
        foreach(CardUI cardUI in CardUIs)
        {
            Destroy(cardUI.gameObject);
        }

        CardUIs.Clear();
        _wrapper.SetActive(false);
    }

    public void OnSelected(Card selected)
    {
        if (_selections.Count == _selectionsNeededMax && !_selections.Contains(selected))
        {
            CardUIs.Find(c => c.Card == _selections.First()).OnButtonPressed();
        }

        if (_selections.Contains(selected))
            _selections.Remove(selected);
        else
            _selections.Add(selected);

        UpdateConfirmSelectionUI();
        //return true;
    }

    public void UpdateConfirmSelectionUI()
    {
        if(_selections == null)
        {
            _confirmSelectionButton.gameObject.SetActive(false);
            return;
        }

        bool active = WaitingForSelection;
        int count = _selections.Count;
        active = active && count >= _selectionsNeededMin && count <= _selectionsNeededMax;

        _confirmSelectionButton.gameObject.SetActive(active);
    }

    public void ConfirmSelection()
    {
        WaitingForSelection = false;
        Close();
    }

    public List<Card> GetCardSelections()
    {
        List<Card> cardSelections = _selections;

        _selections = null;

        return cardSelections;
    }

    public void OpenUpgradeUI(Card selected)
    {
        _upgradeWrapper.SetActive(true);
        _beforeUpgradeUI.SetUp(selected);
        _afterUpgradeUI.SetUp(new(selected.Upgrade));
    }

    public void ConfirmUpgrade()
    {
        RunSystem.Instance.UpgradeCard(_beforeUpgradeUI.Card);

        _upgradeWrapper.SetActive(false);
        Close();
        WaitingForSelection = false;
    }

    public void CancelUpgrade()
    {
        _upgradeWrapper.SetActive(false);
    }
}
