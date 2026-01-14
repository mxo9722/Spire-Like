using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;
using XNode;

public class AddRenownEA : EventAction
{
    [SerializeField, Min(1)] private int _amount;

    public override IEnumerator Invoke()
    {
        RenownSystem.Instance.Add(_amount);
        yield return null;
    }
}
