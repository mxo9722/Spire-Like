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
    [SerializeField] private TMPro.TMP_Text _selectionText;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject _upgradeWrapper;
    [SerializeField] private CardUI _beforeUpgradeUI;
    [SerializeField] private CardUI _afterUpgradeUI;

    public bool Opened { get => _wrapper.activeSelf; }
    public bool WaitingForSelection { get; private set; } = false;

    public List<CardUI> CardUIs { get; private set; } = new();

    private int _selectionsNeeded = 0;
    private List<Card> _selections = null;

    public void SetUp(List<Card> cardCollection)
    {
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(true);
        _selectionText.gameObject.SetActive(false);

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card);
            CardUIs.Add(cardUI);
        }
    }

    public void SetUpSelection(List<Card> cardCollection, int amountNeeded)
    {
        if (amountNeeded >= cardCollection.Count)
        {
            _selections = new(cardCollection);
            return;
        }

        WaitingForSelection = true;
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(false);
        _upgradeWrapper.SetActive(false);

        _selectionsNeeded = amountNeeded;
        _selections = new();
        _selectionText.gameObject.SetActive(true);

        if(_selectionsNeeded == 1)
            _selectionText.text = "Select " + _selectionsNeeded + " card";
        else
            _selectionText.text = "Select " + _selectionsNeeded + " cards";

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card, OnSelected);
            CardUIs.Add(cardUI);
        }
    }
    
    public void SetUpUpgradeChoice()
    {
        List<Card> cardCollection = RunSystem.Instance.Deck.Where(c => c.Upgrade != null).Select(c => new Card(c)).ToList();

        if (1 >= cardCollection.Count)
        {
            cardCollection.ForEach(c => RunSystem.Instance.UpgradeCard(c.data));
            return;
        }

        WaitingForSelection = true;
        _wrapper.SetActive(true);
        _returnButton.gameObject.SetActive(false);
        _upgradeWrapper.SetActive(false);

        _selections = new();
        _selectionText.gameObject.SetActive(true);

        _selectionText.text = "Select a card to upgrade";

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card, OpenUpgradeUI);
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
        if (_selections.Count == _selectionsNeeded)
            return;

        if (_selections.Contains(selected))
            _selections.Remove(selected);
        else
            _selections.Add(selected);

        if(_selections.Count == _selectionsNeeded)
        {
            WaitingForSelection = false;
            Close();
        }
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
        RunSystem.Instance.UpgradeCard(_beforeUpgradeUI.Card.data);

        _upgradeWrapper.SetActive(false);
        Close();
        WaitingForSelection = false;
    }

    public void CancelUpgrade()
    {
        _upgradeWrapper.SetActive(false);
    }
}
