using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPileUI : CardPileView, IPointerEnterHandler, IPointerExitHandler
{

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
