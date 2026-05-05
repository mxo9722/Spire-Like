using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class ClearStatusEffectEffect : CombatantTargetEffect
{

    [SerializeReference, SR] private SEIdentifier _identifier;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        RemoveAllStatusEffectGA removeAllStatusEffectGA = new RemoveAllStatusEffectGA(_identifier.GetSEInfo(), combatantTargets);
        return removeAllStatusEffectGA;
    }
}
