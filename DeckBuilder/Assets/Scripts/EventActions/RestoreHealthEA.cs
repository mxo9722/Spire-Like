using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthEA : HeroTargettedEventAction
{

    [SerializeReference, SR] private Quantity _amount = new SetQ();

    protected override IEnumerator Invoke(EffectContext context,List<Hero> targets)
    {
        foreach (var target in targets)
        {
            int health = target.CurrentHealth;
            health += _amount.GetStaticAmount();
            target.SetCurrentHealth(health);
            TopBarUI.Instance.UpdateHealth();
        }
        yield return null;
    }
}
