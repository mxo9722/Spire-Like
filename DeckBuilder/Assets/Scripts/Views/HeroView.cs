using DG.Tweening;
using UnityEngine;

public class HeroView : CombatantView
{

    public virtual bool IsProtagonist { get => true; }
    public Hero Hero { get; private set; }

    public HeroData HeroData => Hero.Data;

    public void Setup(Hero hero, SlotView slot)
    {
        SetupBase(hero.StartingMaxHealth, hero.Image, slot);

        Hero = hero;
        SetHealth(hero.CurrentHealth);

        OnHealthChanged += TopBarUI.Instance.UpdateHealth;
    }

    private void OnDisable()
    {

    }

    public override void Die()
    {
        
    }
}
