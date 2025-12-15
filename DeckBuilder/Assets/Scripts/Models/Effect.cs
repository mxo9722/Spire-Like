using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null);

    public virtual List<StatusEffectType> GetAllStatusEffects() { return null; }
}
