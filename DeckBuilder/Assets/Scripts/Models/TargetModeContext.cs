using UnityEngine;

public class TargetModeContext
{
    public CombatantView Caster { get; private set; }

    public TargetModeContext(CombatantView caster)
    {
        Caster = caster;
    }
}
