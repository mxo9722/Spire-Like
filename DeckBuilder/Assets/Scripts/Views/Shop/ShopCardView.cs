using DG.Tweening;
using System;
using UnityEngine;

public class ShopCardView : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;
    [SerializeField] private TMPro.TMP_Text _costText;

    public Card Card { get; private set; } = null;
    
    public int Cost { get; private set; }
    public CardView CardView { get; private set; } = null;
    public Action<ShopCardView> OnButtonPressed;

    public void SetUp(Card card, int cost, Vector3 cardSpawnPos)
    {
        Card = card;
        Cost = cost;

        if (card != null)
        {
            CardView = CardViewCreator.Instance.CreateCardView(card, cardSpawnPos, Quaternion.identity, true);
            CardView.transform.parent = _cardParent;
            CardView.transform.DOLocalMove(Vector3.zero, 0.15f);
            CardView.OnButtonPressed += OnCardPressed;

            _costText.text = cost.ToString();
        }
        else if (CardView != null)
        {
            Destroy(CardView.gameObject);
            _costText.text = "";
        }
    }

    public void AddSaleBanner()
    {
        _costText.text = "<b><color=\"red\">Sale!</color></b> " + _costText.text;
    }

    private void OnCardPressed(CardView cardView)
    {
        OnButtonPressed?.Invoke(this);
    }
}
