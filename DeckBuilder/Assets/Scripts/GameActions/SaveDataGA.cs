using UnityEngine;

public class SaveDataGA : GameAction
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

    public void SimulatedPerform()
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(Context, DataLevel);
        dataHolder.SetData(Key, Data);
    }
}
