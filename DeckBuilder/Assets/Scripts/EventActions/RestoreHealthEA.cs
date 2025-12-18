using SerializeReferenceEditor;
using System.Collections;
using UnityEngine;

public class RestoreHealthEA : EventAction
{

    [SerializeReference, SR] private Quantity _amount = new SetQ();

    public override IEnumerator Invoke()
    {
        int health = HeroSystem.Instance.GetHealth();
        health += _amount.GetStaticAmount();
        RunSystem.Instance.SetHealth(health);
        yield return null;
    }
}
