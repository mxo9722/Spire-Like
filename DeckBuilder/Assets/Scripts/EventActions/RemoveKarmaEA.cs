using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;
using XNode;

public class RemoveKarmaEA : EventAction
{
    [SerializeField, Min(1)] private int _credits;

    public override IEnumerator Invoke()
    {
        KarmaSystem.Instance.RemoveCredits(_credits);
        yield return null;
    }

}
