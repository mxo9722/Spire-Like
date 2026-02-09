using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [field: SerializeField] public CardView CardViewHover { get; private set; }
    [SerializeField] private HelpBoxesUI _helpBoxesUI;

    public void Show(Card card, Vector3 pos)
    {
        CardViewHover.gameObject.SetActive(true);
        CardViewHover.Setup(card);
        _helpBoxesUI.Hide();

        _helpBoxesUI.Populate(card);

        List<Tween> tweens = DOTween.TweensByTarget(CardViewHover.transform);

        if(tweens != null)
        foreach (Tween tween in tweens)
            tween.Kill(false);

        CardViewHover.transform.position = pos;
    }

    public void TweenToPosition(Vector3 pos)
    {
        List<Tween> tweens = DOTween.TweensByTarget(CardViewHover.transform);

        if (tweens != null)
            foreach (Tween tween in tweens)
                tween.Kill(false);

        CardViewHover.transform.DOMove(pos, 0.15f);
        _helpBoxesUI.Hide();
    }

    public void Hide()
    {
        CardViewHover.gameObject.SetActive(false);
        _helpBoxesUI.Hide();
    }

    public void UpdateDynamicDescription(Card card)
    {
        CardViewHover.UpdateDynamicDescription(new(card.GetOwnerView()));
    }
    
    public void UpdateDynamicDescription(EffectContext context)
    {
        CardViewHover.UpdateDynamicDescription(context);
    }
}
