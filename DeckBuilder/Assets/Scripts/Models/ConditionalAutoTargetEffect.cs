using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionalAutoTargetEffect : AutoTargetEffect
{
    [SerializeReference, SR] private List<Condition> _conditions;
    [SerializeReference, SR] private List<AutoTargetEffect> _successEffects;
    [SerializeReference, SR] private List<AutoTargetEffect> _failEffects;
    //[SerializeReference, SR] private AutoTargetEffect _successATEffect;

    public AutoTargetEffect SuccessEffect { get; }


    public List<Condition> Conditions { get => _conditions; }

    public override Effect[] Effects => _successEffects.SelectMany(e => e.Effects).ToArray();

    public override List<StatusEffect> GetAllStatusEffects() => _successEffects.SelectMany(e => e.GetAllStatusEffects()).ToList();

    public override GameAction GetGameAction(EffectContext context)
    {
        if(_conditions.TrueForAll(c => c.TestCondition(context)))
        {
            MultipleEffectsGA successEffectsGA = new(context, _successEffects);
            return successEffectsGA;
        }

        MultipleEffectsGA failureEffectsGA = new(context, _failEffects);
        return failureEffectsGA;
    }

    public override bool RequiresUserInput() => _successEffects.Any(e => e.RequiresUserInput()) || _failEffects.Any(e => e.RequiresUserInput());

    public override IEnumerator WaitForUserInput()
    {
        //This probably won't work correctly under specific fail conditions
        foreach(AutoTargetEffect effect in _successEffects)
        {
            yield return effect.WaitForUserInput();
        }
    }

    public bool ConditionIsMeetable(EffectContext context, Card card)
    {
        return _conditions.TrueForAll(c => c.IsConditionMeetable(context,card));
    }
}
