using UnityEngine;

public class HeroView : CombatantView
{
    public void Setup(HeroData heroData, SlotView slot)
    {
        SetupBase(heroData.StartingMaxHealth, heroData.Image, slot);
    }

    public override void Die()
    {
        
    }
}
