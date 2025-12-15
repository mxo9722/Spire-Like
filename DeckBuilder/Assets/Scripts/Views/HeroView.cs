using UnityEngine;

public class HeroView : CombatantView
{
    public void Setup(HeroData heroData)
    {
        SetupBase(heroData.StartingMaxHealth, heroData.Image);
    }

    public override void Die()
    {
        
    }
}
