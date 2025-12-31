using UnityEngine;

public class HeroView : CombatantView
{

    public virtual bool IsProtagonist { get => true; }

    public void Setup(HeroData heroData, SlotView slot)
    {
        SetupBase(heroData.StartingMaxHealth, heroData.Image, slot);
    }

    public override void Die()
    {
        
    }
}
