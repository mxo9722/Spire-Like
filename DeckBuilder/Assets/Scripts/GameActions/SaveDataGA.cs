using UnityEngine;

public class SaveDataGA : SimulatedGameAction
{
    public EffectContext Context { get; private set; }
    public string Key { get; private set; }
    public object Data { get; private set; }
    public SaveDataLevel DataLevel { get; private set; }

    public SaveDataGA(EffectContext context, string key, object data, SaveDataLevel dataLevel = SaveDataLevel.CONTEXT)
    {
        Context = context;
        Key = key;
        Data = data;
        DataLevel = dataLevel;
    }

    public override void SimulatedPerform(EffectContext context)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(context, DataLevel);
        dataHolder.SetData(Key, Data);
    }
}
