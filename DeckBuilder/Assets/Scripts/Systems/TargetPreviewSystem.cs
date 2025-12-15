using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetPreviewSystem : Singleton<TargetPreviewSystem>
{

    [SerializeField, Min(0.0001f)] private float _randomJumpTime;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _possibleColor;
    [SerializeField] private bool _randomCycleTargets = false;


    private bool _highLighted = false;

    private List<Coroutine> _randomCoroutines = new();

    public void SetTargetPreviews(Card card)
    {
        if (_highLighted)
            return;

        _highLighted = true;

        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> highlights = new();

        EffectContext context = EffectContext.CreateHeroEC();

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if (effect is ConditionalAutoTargetEffect)
                highlights.AddRange(GetTargets(effect));
            else
                targets.AddRange(GetTargets(effect));
        }

        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
        highlights.ForEach(h => h.SetTargetPreview(Color.yellow));

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            _randomCoroutines.AddRange(GetRandomTargetCoroutines(effect));
        }
    }

    public void SetTargetPreviewsManual<T, F>(List<F> filters, List<ConditionalAutoTargetEffect> highlightConditionals) where T : ITargetPreviewable where F : TargetFilter<T>
    {
        if (_highLighted)
            return;

        _highLighted = true;

        EffectContext context = EffectContext.CreateHeroEC();

        IEnumerable<T> allTargets = BoardSystem.Instance.GetAllViews<T>().Where(l => filters.TrueForAll(f => f.TestTarget(context, l)));

        foreach (T target in allTargets)
        {
            EffectContext specificContext = EffectContext.CreateHeroEC();

            if (target is EnemyView enemyView)
                specificContext = EffectContext.CreateHeroEC(enemyView);
            else if (target is LaneView laneView)
                specificContext = EffectContext.CreateHeroEC(laneView);

            bool highlight = highlightConditionals.Any(h => h.Conditions.TrueForAll(c => c.TestCondition(specificContext)));

            if (highlight)
                target.SetTargetPreview(Color.yellow);
            else
                target.SetTargetPreview(_defaultColor);
        }

    }

    public void HideTargetPreviews()
    {
        if (!_highLighted)
            return;

        _highLighted = false;

        foreach (Coroutine coroutine in _randomCoroutines)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        _randomCoroutines.Clear();

        BoardSystem.Instance.GetAllCombatants().ForEach(c => c.HideTargetPreview());
        BoardSystem.Instance.GetAllLanes().ForEach(l => l.HideTargetPreview());
    }

    private List<ITargetPreviewable> GetTargets(AutoTargetEffect effect)
    {
        List<ITargetPreviewable> targets = new();

        EffectContext context = EffectContext.CreateHeroEC();

        if (effect is AutoCombatantTargetEffect cEffect && (!cEffect.TargetMode.IsRandom || !_randomCycleTargets))
        {
            targets.AddRange(cEffect.TargetMode.AllPossibleTargets(context));
        }
        else if (effect is AutoLaneTargetEffect lEffect && (!lEffect.TargetMode.IsRandom || !_randomCycleTargets))
        {
            targets.AddRange(lEffect.TargetMode.AllPossibleTargets(context));
        }
        else if (effect is ConditionalAutoTargetEffect conditional)
        {
            if (conditional.GetGameAction(context) != null)
            {
                return GetTargets(conditional.SuccessEffect);
            }
        }

        return targets;
    }

    private List<Coroutine> GetRandomTargetCoroutines(AutoTargetEffect effect, Color color = default)
    {
        if (!_randomCycleTargets)
            return new();

        if (color == default)
            color = _defaultColor;

        List<Coroutine> coroutines = new();

        EffectContext context = EffectContext.CreateHeroEC();

        if (effect is AutoCombatantTargetEffect cEffect && cEffect.TargetMode.IsRandom)
        {
            coroutines.Add(StartCoroutine(RandomTargetPreview(cEffect.TargetMode, color)));
        }
        else if (effect is AutoLaneTargetEffect lEffect && lEffect.TargetMode.IsRandom)
        {
            coroutines.Add(StartCoroutine(RandomTargetPreview(lEffect.TargetMode, color)));
        }
        else if (effect is ConditionalAutoTargetEffect conditional)
        {
            if (conditional.GetGameAction(context) != null)
            {
                return GetRandomTargetCoroutines(conditional.SuccessEffect, Color.yellow);
            }
        }

        return coroutines;
    }

    private IEnumerator RandomTargetPreview<T>(TargetMode<T> tm, Color color) where T : ITargetPreviewable
    {
        EffectContext context = EffectContext.CreateHeroEC();
        List<T> targets = tm.GetTargetsTrivial(context);
        targets.RemoveAll(t => t.TargetPreviewActive);
        List<T> allPossible = tm.AllPossibleTargets(context);
        allPossible.RemoveAll(t => t.TargetPreviewActive);

        targets.ForEach(t => t.SetTargetPreview(color));
        allPossible.ForEach(t => t.SetTargetPreview(_possibleColor));

        while (targets.Count < tm.AllPossibleTargets(context).Count)
        {
            yield return new WaitForSeconds(_randomJumpTime);

            allPossible.ForEach(t => t.HideTargetPreview());
            targets.ForEach(t => t.HideTargetPreview());

            List<T> randomTry;

            do
            {
                randomTry = tm.GetTargetsTrivial(context);
                randomTry.RemoveAll(t => t.TargetPreviewActive);
            }
            while (targets.Except(randomTry).Count() == 0);

            targets = randomTry;

            targets.ForEach(t => t.SetTargetPreview(color));
            allPossible.ForEach(t => t.SetTargetPreview(_possibleColor));
        }
    }
}
