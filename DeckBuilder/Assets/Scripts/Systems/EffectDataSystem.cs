using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDataSystem : Singleton<EffectDataSystem>
{
    public Dictionary<string, object> EffectData { get; private set; } = null;

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
        saveDataGA.Context.AddData(saveDataGA.Key, saveDataGA.Data);
        yield return null;
    }

    public T GetData<T>(string key)
    {
        if (EffectData == null)
            return default(T);

        var data = EffectData[key];
        if (data is T t)
            return t;

        Debug.LogError("Error to convert data at " + key + " to type " + typeof(T).ToString() + "!");
        return default(T);
    }
}
