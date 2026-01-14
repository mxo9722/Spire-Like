using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDataSystem : Singleton<EffectDataSystem>
{

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SaveDataGA>(SaveDataPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SaveDataGA>();
    }

    public IEnumerator SaveDataPerformer(SaveDataGA saveDataGA)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(saveDataGA.Context, saveDataGA.DataLevel);

        dataHolder.AddData(saveDataGA.Key, saveDataGA.Data);
        yield return null;
    }
}
