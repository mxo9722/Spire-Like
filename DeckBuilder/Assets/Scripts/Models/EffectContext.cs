using UnityEngine;

public class EffectContext
{
    public CombatantView Caster { get; private set; }
    public LaneView TargetLane { get; private set; }
    public CombatantView TargetCombatant { get; private set; }

    public EffectContext(CombatantView caster, LaneView manualTargetLane = null, CombatantView manualTargetCombatant = null)
    {
        Caster = caster;
        TargetLane = manualTargetLane;
        TargetCombatant = manualTargetCombatant;
    }

    #region CREATION_UTILITY
    public static EffectContext CreateHeroEC()
    {
        return new(HeroSystem.Instance.HeroView);
    }

    public static EffectContext CreateHeroEC(CombatantView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetCombatant: target);
    }

    public static EffectContext CreateHeroEC(LaneView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetLane: target);
    }

    public static EffectContext CreateEnemyEC(CombatantView caster)
    {
        return new(caster);
    }
    #endregion
}
