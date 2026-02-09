using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView _cardViewPrefab;
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation, bool treatAsButton = false, Transform parent = null)
    {
        CardView cardView = Instantiate(_cardViewPrefab, position, rotation);

        if (parent != null)
            transform.parent = parent;

        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);

        cardView.Setup(card, treatAsButton);

        if (!treatAsButton)
        {
            cardView.SetSideUp(false);
            cardView.SetSideUp(true, 0.3f);
        }

        return cardView;
    }
}
