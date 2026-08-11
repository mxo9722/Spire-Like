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

        EffectContext context = new(card.GetOwnerView());

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if (effect is ConditionalAutoTargetEffect conditional && !conditional.HideHighlight)
                highlights.AddRange(GetTargets(effect, context, card));
            else
                targets.AddRange(GetTargets(effect, context, card));
        }

        highlights.ForEach(h => h.SetTargetPreview(_highlightColor));
        targets.ForEach(t => t.SetTargetPreview(_defaultColor));

        if (card.IsChaotic())
        {
            switch (card.ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    _randomCoroutines.AddRange(GetRandomTargetCoroutines(card.GetChaosTargetMode<CombatantView>(), card.GetOwnerView(), _defaultColor));
                    break;
                case ManualTargetType.LANE:
                    _randomCoroutines.AddRange(GetRandomTargetCoroutines(card.GetChaosTargetMode<LaneView>(), card.GetOwnerView(), _defaultColor));
                    break;
            }
        }

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            _randomCoroutines.AddRange(GetRandomTargetCoroutines(effect, card.GetOwnerView()));
        }
    }

    public void SetTargetPreviewsManual(Card card, CombatantView manualTarget)
    {
        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        EffectContext context = new(card.GetOwnerView(new(null, manualTargetCombatant: manualTarget, playedCard: card)), manualTargetCombatant: manualTarget, playedCard: card);
        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> hTargets = new();

        targets.Add(manualTarget);

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if (effect is ConditionalAutoTargetEffect conditional && !conditional.HideHighlight)
                hTargets.AddRange(GetTargets(effect, context, card));
            else
                targets.AddRange(GetTargets(effect, context, card));
        }

        hTargets.ForEach(t => t.SetTargetPreview(_highlightColor));
        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
    }

    public void SetTargetPreviewsManual(Card card, LaneView manualTarget)
    {
        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        EffectContext context = new(card.GetOwnerView(new(null, manualTargetLane: manualTarget, playedCard: card)), manualTarget, playedCard: card);
        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> hTargets = new();

        targets.Add(manualTarget);

        foreach (AutoTargetEffect effect in card.OtherEffects)
        {
            if (effect is ConditionalAutoTargetEffect conditional && !conditional.HideHighlight)
                hTargets.AddRange(GetTargets(effect, context, card));
            else
                targets.AddRange(GetTargets(effect, context, card));

        }

        hTargets.ForEach(t => t.SetTargetPreview(_highlightColor));
        targets.ForEach(t => t.SetTargetPreview(_defaultColor));
    }

    public void SetTargetPreviewsManual<T, F>(Card card, List<F> filters, List<ConditionalAutoTargetEffect> highlightConditionals, EffectContext context) where T : ITargetPreviewable where F : TargetFilter<T>
    {
        if(context.Caster == null)
        {
            SetTargetPreviewsManualNoCaster<T, F>(filters, highlightConditionals, context);
            return;
        }

        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        IEnumerable<T> allTargets = BoardSystem.Instance.GetAllViews<T>().Where(l => filters.TrueForAll(f => f.TestTarget(context, l)));

        foreach (T target in allTargets)
        {
            context = new(context);

            if (target is CombatantView combatant)
            {
                context.SetManualCombatant(combatant);

                if (!combatant.IsSelectable())
                    combatant.SetImageAlpha(0.5f, 0.75f);
            }
            else if (target is LaneView laneView)
                context.SetManualLane(laneView);

            if (!target.IsSelectable())
                continue;

            bool highlight = highlightConditionals.Any(h => h.AllConditionsMet(context) && !h.HideHighlight) || (!card.JustHighlightCard && card.IsHighlighted(context));

            if (highlight)
                target.SetTargetPreview(_highlightColor);
            else
                target.SetTargetPreview(_possibleColor);
        }
    }

    private void SetTargetPreviewsManualNoCaster<T, F>(List<F> filters, List<ConditionalAutoTargetEffect> highlightConditionals, EffectContext context) where T : ITargetPreviewable where F : TargetFilter<T>
    {
        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        foreach (HeroView caster in HeroSystem.Instance.HeroViews)
        {
            EffectContext nContext = new(context);
            nContext.SetCaster(caster);

            IEnumerable<T> allTargets = BoardSystem.Instance.GetAllViews<T>().Where(l => filters.TrueForAll(f => f.TestTarget(nContext, l)));

            foreach (T target in allTargets)
            {
                nContext = new(caster);

                if (target is CombatantView combatantView)
                {
                    nContext = new(caster, manualTargetCombatant: combatantView);

                    if (!combatantView.IsSelectable())
                        combatantView.SetImageAlpha(0.5f, 0.75f);
                }
                else if (target is LaneView laneView)
                    nContext = new(caster, manualTargetLane: laneView);

                if (!target.IsSelectable())
                    continue;

                bool highlight = highlightConditionals.Any(h => h.AllConditionsMet(context) && !h.HideHighlight);

                if (highlight)
                    target.SetTargetPreview(_highlightColor);
                else
                    target.SetTargetPreview(_possibleColor);
            }
        }
    }

    public void SetTargetPreviews(NPCView npc, NPCAction action)
    {
        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        List<ITargetPreviewable> targets = new();
        List<ITargetPreviewable> highlights = new();

        EffectContext context = new(npc);

        foreach (AutoTargetEffect effect in action.Effects)
        {
            if (effect is ConditionalAutoTargetEffect conditional && !conditional.HideHighlight)
                highlights.AddRange(GetTargets(effect, context, null));
            else
                targets.AddRange(GetTargets(effect, context, null));
        }

        highlights.ForEach(h => h.SetTargetPreview(_highlightColor));
        targets.ForEach(t => t.SetTargetPreview(_defaultColor));

        foreach (AutoTargetEffect effect in action.Effects)
        {
            _randomCoroutines.AddRange(GetRandomTargetCoroutines(effect, npc));
        }
    }

    public void SetEnemyLaneAttackPreview(LaneView lane)
    {
        if (HighLighted)
        {
            HideTargetPreviews(false);
        }

        HighLighted = true;

        List<NPCView> enemyViews = BoardSystem.Instance.GetAllEnemies();

        enemyViews.RemoveAll(e => !e.IsHostileTargetLane(lane));

        enemyViews.ForEach(t => t.SetTargetPreview(_defaultColor));
    }

    public void HideTargetPreviews(bool temporary)
    {
        if (!HighLighted)
            return;

        HighLighted = false;

        foreach (Coroutine coroutine in _randomCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        _randomCoroutines.Clear();

        BoardSystem.Instance.GetAllCombatants().ForEach(
            c =>
            {
                c.HideTargetPreview();
                if (temporary)
                    c.SetImageAlpha(1, 0.75f);
            }
            );
        BoardSystem.Instance.GetAllLanes().ForEach(l => l.HideTargetPreview());
    }

    private List<ITargetPreviewable> GetTargets(AutoTargetEffect effect, EffectContext context, Card card)
    {
        if (effect == null)
            return new();

        List<ITargetPreviewable> targets = new();

        if (context == null)
            context = new(card.GetOwnerView(), playedCard: card);

        if (effect is AutoCombatantTargetEffect cEffect && (!cEffect.TargetMode.IsRandom || !_randomCycleTargets))
        {
            targets.AddRange(cEffect.TargetMode.AllPossibleTargets(context, card));
        }
        else if (effect is AutoLaneTargetEffect lEffect && (!lEffect.TargetMode.IsRandom || !_randomCycleTargets))
        {
            targets.AddRange(lEffect.TargetMode.AllPossibleTargets(context, card));
        }
        else if (effect is ConditionalAutoTargetEffect conditional)
        {
            if (conditional.GetGameAction(context) != null)
            {
                return GetTargets(conditional.SuccessEffect, context, card);
            }
        }

        AutoTargetEffect[] nestedEffects = effect.GetNestedEffects();

        foreach(AutoTargetEffect nestedEffect in nestedEffects)
        {
            targets.AddRange(GetTargets(nestedEffect, context, card));
        }

        return targets;
    }

    private List<Coroutine> GetRandomTargetCoroutines<T>(TargetMode<T> targetMode, CombatantView caster, Color color = default) where T : ITargetPreviewable
    {
        if (!_randomCycleTargets)
            return new();

        if (color == default)
            color = _defaultColor;

        List<Coroutine> coroutines = new();

        coroutines.Add(StartCoroutine(RandomTargetPreview(targetMode, caster, color)));

        return coroutines;
    }

    private List<Coroutine> GetRandomTargetCoroutines(AutoTargetEffect effect, CombatantView caster, Color color = default)
    {
        if (!_randomCycleTargets)
            return new();

        if (color == default)
            color = _defaultColor;

        List<Coroutine> coroutines = new();

        EffectContext context = new(caster);

        if (effect is AutoCombatantTargetEffect cEffect && cEffect.TargetMode.IsRandom)
        {
            coroutines.Add(StartCoroutine(RandomTargetPreview(cEffect.TargetMode, caster, color)));
        }
        else if (effect is AutoLaneTargetEffect lEffect && lEffect.TargetMode.IsRandom)
        {
            coroutines.Add(StartCoroutine(RandomTargetPreview(lEffect.TargetMode, caster, color)));
        }
        else if (effect is ConditionalAutoTargetEffect conditional)
        {
            if (conditional.AllConditionsMet(context) && !conditional.HideHighlight)
            {
                return GetRandomTargetCoroutines(conditional.SuccessEffect, caster, _highlightColor);
            }
            else
            {
                return GetRandomTargetCoroutines(conditional.SuccessEffect, caster, color);
            }
        }

        if (effect != null)
        {
            AutoTargetEffect[] nestedEffects = effect.GetNestedEffects();

            foreach (AutoTargetEffect nestedEffect in nestedEffects)
            {
                coroutines.AddRange(GetRandomTargetCoroutines(nestedEffect, caster, color));
            }
        } 

        return coroutines;
    }

    private IEnumerator RandomTargetPreview<T>(TargetMode<T> tm, CombatantView caster, Color color) where T : ITargetPreviewable
    {
        EffectContext context = new(caster);
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
