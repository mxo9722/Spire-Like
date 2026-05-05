using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class ConvertStatusEffectEffect : CombatantTargetEffect
{
    [field: SerializeReference, SR] public StatusEffectInfo From { get; private set; }
    [field: SerializeReference, SR] public StatusEffectInfo To { get; private set; }
    [field: SerializeReference, SR] public Quantity UpTo { get; private set; } = new SetQ(-1);

    public ConvertStatusEffectEffect()
    {

    }
    
    public ConvertStatusEffectEffect(StatusEffectInfo from, StatusEffectInfo to, int upTo)
    {
        From = from;
        To = to;
        UpTo = new SetQ(upTo);
    }
    
    public ConvertStatusEffectEffect(StatusEffectInfo from, StatusEffectInfo to, Quantity upTo)
    {
        From = from;
        To = to;
        UpTo = upTo;
    }

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        ConvertStatusEffectGA convertStatusEffectGA = new(combatantTargets, From, To, UpTo.GetAmount(context));
        return convertStatusEffectGA;
    }
}
