using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDataSystem : Singleton<EffectDataSystem>
{

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SaveDataGA>(SaveDataPerformer);
        ActionSystem.AttachPerformer<SaveDataToUnitsGA>(SaveDataToUnitsPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SaveDataGA>();
        ActionSystem.DetachPerformer<SaveDataToUnitsGA>();
    }

    public IEnumerator SaveDataPerformer(SaveDataGA saveDataGA)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(saveDataGA.Context, saveDataGA.DataLevel);

        dataHolder.AddData(saveDataGA.Key, saveDataGA.Data);

        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }

    private IEnumerator SaveDataToUnitsPerformer(SaveDataToUnitsGA saveDataToUnitsGA)
    {
        foreach (CombatantView target in saveDataToUnitsGA.Targets)
        {
            target.AddData(saveDataToUnitsGA.Key, saveDataToUnitsGA.Value);
        }

        yield return null;
    }
}
