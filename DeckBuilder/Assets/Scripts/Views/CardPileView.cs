using System.Collections.Generic;
using UnityEngine;

public class CardPileView : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text _count;
    [SerializeField] private bool _hideOrder = false;

    private List<Card> _cards;

    public void SetUp(List<Card> drawPile)
    {
        _cards = drawPile;
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = new Vector3(1.2f,1.2f,1.2f);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = Vector3.one;
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = Vector3.one;
        CardCollectionSystem.Instance.Display(_cards, _hideOrder);
    }

    public void UpdateUI()
    {
        _count.text = _cards.Count.ToString();

        if(CardCollectionSystem.Instance.Opened && CardCollectionSystem.Instance.Cards == _cards)
        {
            CardCollectionSystem.Instance.Display(_cards, _hideOrder);
        }
    }
}
