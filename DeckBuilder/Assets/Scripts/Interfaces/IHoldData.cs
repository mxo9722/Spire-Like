using System.Collections.Generic;
using UnityEngine;

public interface IHoldData
{

    public abstract void AddData(string key, object data);
    public abstract T GetData<T>(string key);
    public abstract bool ContainsKey(string key);
    
    public static IHoldData GetDataHolder(EffectContext context, SaveDataLevel dataLevel)
    {
        switch (dataLevel)
        {
            case SaveDataLevel.CONTEXT:
                return context;
            case SaveDataLevel.COMBATANT:
                return context.Caster;
            default:
                return RunSystem.Instance;
        }
    }
}
