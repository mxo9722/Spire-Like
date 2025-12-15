using System.Collections.Generic;
using UnityEngine;

public class CardPileView : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text _count;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private bool _hideOrder = false;
    [SerializeField] private bool _hideWhenEmpty = false;

    protected virtual List<Card> _cards { get; set; }

    public void SetUp(List<Card> drawPile)
    {
        _cards = drawPile;
        UpdateUI();
    }

    protected void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
            transform.localScale = new Vector3(1.2f,1.2f,1.2f);
    }

    protected void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
            transform.localScale = Vector3.one;
    }

    protected void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = Vector3.one;
        CardCollectionSystem.Instance.Display(_cards, _hideOrder);
    }

    public void UpdateUI()
    {
        if(_count != null)
            _count.text = _cards.Count.ToString();

        if(_hideWhenEmpty) 
        {
            _wrapper.SetActive(_cards.Count > 0);
        }

        if(CardCollectionSystem.Instance.Opened && CardCollectionSystem.Instance.Cards == _cards)
        {
            CardCollectionSystem.Instance.Display(_cards, _hideOrder);
        }
    }
}
