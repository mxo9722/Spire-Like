using System.Collections;
using UnityEngine;

public class SoakSystem : Singleton<SoakSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SetSoakGA>(SetSoakPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SetSoakGA>();
    }

    private IEnumerator SetSoakPerformer(SetSoakGA setSoakGA)
    {
        Coroutine coroutine = null;

        foreach(LaneView lane in setSoakGA.LaneViews)
        {
            if (lane.IsSoaked != setSoakGA.Soaked)
                coroutine = StartCoroutine(lane.SetSoaked(setSoakGA.Soaked));
        }

        if (coroutine != null)
            yield return new WaitForSeconds(0.5f);
    }
}
