using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;
using XNode;

public class AddGoldEA : EventAction
{
    [SerializeField, Min(1)] private int _credits;

    public override IEnumerator Invoke()
    {
        CreditSystem.Instance.Add(_credits);
        yield return null;
    }
}
