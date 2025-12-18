using System.Collections;
using UnityEngine;

public class UpgradeCardEA : EventAction
{
    public override IEnumerator Invoke()
    {
        CardCollectionSystem.Instance.UpgradeDisplay();
        yield return new WaitUntil(() => !CardCollectionSystem.Instance.WaitingForSelection);
    }
}
