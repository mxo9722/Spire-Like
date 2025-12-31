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

    public bool HighLighted { get; private set; } = false;

    private List<Coroutine> _randomCoroutines = new();

    public void SetTargetPreviews(Card card)
    {
        if (HighLighted)
        {
            return;
        }

        HighLighted = true;

        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> highlights = new();

        EffectContext context = EffectContext.CreateHeroEC();

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if (effect is ConditionalAutoTargetEffect)
                highlights.AddRange(GetTargets(effect, context));
            else
                targets.AddRange(GetTargets(effect, context));
        }

        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
        highlights.ForEach(h => h.SetTargetPreview(_highlightColor));

        if (card.IsChaotic())
        {
            switch (card.ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    _randomCoroutines.AddRange(GetRandomTargetCoroutines(card.GetChaosTargetMode<CombatantView>(), _defaultColor));
                    break;
                case ManualTargetType.LANE:
                    _randomCoroutines.AddRange(GetRandomTargetCoroutines(card.GetChaosTargetMode<LaneView>(), _defaultColor));
                    break;
            }
        }

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            _randomCoroutines.AddRange(GetRandomTargetCoroutines(effect));
        }
    }

    public void SetTargetPreviewsManual(Card card, CombatantView manualTarget)
    {
        if (HighLighted)
        {
            HideTargetPreviews();
        }

        HighLighted = true;

        EffectContext context = EffectContext.CreateHeroEC(manualTarget);
        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> hTargets = new();

        targets.Add(manualTarget);

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if(effect is ConditionalAutoTargetEffect)
                hTargets.AddRange(GetTargets(effect, context));
            else
                targets.AddRange(GetTargets(effect, context));
        }

        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
        hTargets.ForEach(t => t.SetTargetPreview(_highlightColor));
    }
    
    public void SetTargetPreviewsManual(Card card, LaneView manualTarget)
    {
        if (HighLighted)
        {
            HideTargetPreviews();
        }

        HighLighted = true;

        EffectContext context = EffectContext.CreateHeroEC(manualTarget);
        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> hTargets = new();

        targets.Add(manualTarget);

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if(effect is ConditionalAutoTargetEffect)
                hTargets.AddRange(GetTargets(effect, context));
            else
                targets.AddRange(GetTargets(effect, context));

        }

        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
        hTargets.ForEach(t => t.SetTargetPreview(_highlightColor));
    }

    public void SetTargetPreviewsManual<T, F>(List<F> filters, List<ConditionalAutoTargetEffect> highlightConditionals) where T : ITargetPreviewable where F : TargetFilter<T>
    {
        if (HighLighted)
        {
            HideTargetPreviews();
        }

        HighLighted = true;

        EffectContext context = EffectContext.CreateHeroEC();

        IEnumerable<T> allTargets = BoardSystem.Instance.GetAllViews<T>().Where(l => filters.TrueForAll(f => f.TestTarget(context, l)));

        foreach (T target in allTargets)
        {
            EffectContext specificContext = EffectContext.CreateHeroEC();

            if (target is NPCView enemyView)
                specificContext = EffectContext.CreateHeroEC(enemyView);
            else if (target is LaneView laneView)
                specificContext = EffectContext.CreateHeroEC(laneView);

            bool highlight = highlightConditionals.Any(h => h.Conditions.TrueForAll(c => c.TestCondition(specificContext)));

            if (highlight)
                target.SetTargetPreview(_highlightColor);
            else
                target.SetTargetPreview(_possibleColor);
        }

    }

    public void SetTargetPreviews(NPCView npc,NPCAction action)
    {
        if (HighLighted)
        {
            HideTargetPreviews();
        }

        HighLighted = true;

        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> highlights = new();

        EffectContext context = EffectContext.CreateNpcEC(npc);

        foreach (AutoTargetEffect effect in action.Effects)
        {
            if (effect is ConditionalAutoTargetEffect)
                highlights.AddRange(GetTargets(effect, context));
            else
                targets.AddRange(GetTargets(effect, context));
        }

        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
        highlights.ForEach(h => h.SetTargetPreview(_highlightColor));

        foreach (AutoTargetEffect effect in action.Effects)
        {
            _randomCoroutines.AddRange(GetRandomTargetCoroutines(effect));
        }
    }

    public void HideTargetPreviews()
    {
        if (!HighLighted)
            return;

        HighLighted = false;

        foreach (Coroutine coroutine in _randomCoroutines)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        _randomCoroutines.Clear();

        BoardSystem.Instance.GetAllCombatants().ForEach(c => c.HideTargetPreview());
        BoardSystem.Instance.GetAllLanes().ForEach(l => l.HideTargetPreview());
    }

    private List<ITargetPreviewable> GetTargets(AutoTargetEffect effect, EffectContext context)
    {
        List<ITargetPreviewable> targets = new();

        if(context == null)
            context = EffectContext.CreateHeroEC();

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
                return GetTargets(conditional.SuccessEffect, context);
            }
        }

        return targets;
    }

    private List<Coroutine> GetRandomTargetCoroutines<T>(TargetMode<T> targetMode, Color color = default) where T : ITargetPreviewable
    {
        if (!_randomCycleTargets)
            return new();

        if (color == default)
            color = _defaultColor;

        List<Coroutine> coroutines = new();

        coroutines.Add(StartCoroutine(RandomTargetPreview(targetMode, color)));

        return coroutines;
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
                return GetRandomTargetCoroutines(conditional.SuccessEffect, _highlightColor);
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
            while (targets.Except(randomTry).Count() == 0 && targets.Count != 0);

            targets = randomTry;

            targets.ForEach(t => t.SetTargetPreview(color));
            allPossible.ForEach(t => t.SetTargetPreview(_possibleColor));
        }
    }
}
