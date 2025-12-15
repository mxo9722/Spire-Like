using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPileUI : CardPileView, IPointerEnterHandler, IPointerExitHandler
{
    private List<CardData> _cardDatas; 

    protected override List<Card> _cards { get => _cardDatas.Select(c => new Card(c)).ToList(); }

    public void SetUp(List<CardData> cardDatas)
    {
        _cardDatas = cardDatas;
    }

    public void OnButtonPressed()
    {
        if (CardCollectionSystem.Instance.Opened)
            CardCollectionSystem.Instance.Close();
        else
            OnMouseDown();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }
}
