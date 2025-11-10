using UnityEngine;

public class ConditionContext
{
    public CombatantView Caster { get; private set; }

    public ConditionContext(CombatantView combatantView)
    {
        Caster = combatantView;
    }
}
