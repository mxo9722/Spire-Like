using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalAutoTargetEffect : AutoTargetEffect
{
    [SerializeReference, SR] private List<Condition> _conditions;
    [SerializeReference, SR] private AutoTargetEffect _successEffect;
    //[SerializeReference, SR] private AutoTargetEffect _successATEffect;

    public AutoTargetEffect SuccessEffect { get; }


    public List<Condition> Conditions { get => _conditions; }

    public override Effect Effect => _successEffect.Effect;

    public override List<StatusEffectType> GetAllStatusEffects() => _successEffect.GetAllStatusEffects();

    public override string GetDynamicText(EffectContext context) => _successEffect.GetDynamicText(context);

    public override GameAction GetGameAction(EffectContext context)
    {
        if(_conditions.TrueForAll(c => c.TestCondition(context)))
        {
            return _successEffect.GetGameAction(context);
        }

        return null;
    }

    public override bool RequiresUserInput() => _successEffect.RequiresUserInput();

    public override IEnumerator WaitForUserInput() => _successEffect.WaitForUserInput();

    public bool ConditionIsMeetable(EffectContext context, Card card)
    {
        return _conditions.TrueForAll(c => c.IsConditionMeetable(context,card));
    }
}
