using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView _cardViewHover;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;

    public void Show(Card card, Vector3 pos)
    {
        _cardViewHover.gameObject.SetActive(true);
        _cardViewHover.Setup(card);
        _helpBoxesUI.Hide();

        List<string> keyWords = card.GetAllKeyWords();

        foreach(string keyWord in keyWords)
            _helpBoxesUI.AddHelpBoxFromKeyWord(keyWord);

        List<Tween> tweens = DOTween.TweensByTarget(_cardViewHover.transform);

        if(tweens != null)
        foreach (Tween tween in tweens)
            tween.Kill(false);

        _cardViewHover.transform.position = pos;
    }

    public void TweenToPosition(Vector3 pos)
    {
        List<Tween> tweens = DOTween.TweensByTarget(_cardViewHover.transform);

        if (tweens != null)
            foreach (Tween tween in tweens)
                tween.Kill(false);

        _cardViewHover.transform.DOMove(pos, 0.15f);
        _helpBoxesUI.Hide();
    }

    public void Hide()
    {
        _cardViewHover.gameObject.SetActive(false);
        _helpBoxesUI.Hide();
    }

    public void UpdateDynamicDescription()
    {
        _cardViewHover.UpdateDynamicDescription(TargetModeContext.CreateHeroTMC());
    }
    
    public void UpdateDynamicDescription(CombatantView targetCombatant)
    {
        _cardViewHover.UpdateDynamicDescription(TargetModeContext.CreateHeroTMC(targetCombatant));
    }
    
    public void UpdateDynamicDescription(LaneView targetLane)
    {
        _cardViewHover.UpdateDynamicDescription(TargetModeContext.CreateHeroTMC(targetLane));
    }
}
