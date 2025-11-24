using UnityEngine;

public class TargetModeContext
{
    public CombatantView Caster { get; private set; }
    public LaneView TargetLane { get; private set; }
    public CombatantView TargetCombatant { get; private set; }

    public TargetModeContext(CombatantView caster,LaneView manualTargetLane = null,CombatantView manualTargetCombatant = null)
    {
        Caster = caster;
        TargetLane = manualTargetLane;
        TargetCombatant = manualTargetCombatant;
    }

    #region CREATION_UTILITY
    public static TargetModeContext CreateHeroTMC()
    {
        return new(HeroSystem.Instance.HeroView);
    }

    public static TargetModeContext CreateHeroTMC(CombatantView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetCombatant: target);
    }

    public static TargetModeContext CreateHeroTMC(LaneView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetLane: target);
    }

    public static TargetModeContext CreateEnemyTMC(CombatantView caster)
    {
        return new(caster);
    }
    #endregion
}
