using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;
using XNode;

public class RemoveRenownEA : EventAction
{
    [SerializeField, Min(1)] private int _renown;

    public override IEnumerator Invoke(EffectContext context)
    {
        RenownSystem.Instance.RemoveRenown(_renown);
        yield return null;
    }

}
