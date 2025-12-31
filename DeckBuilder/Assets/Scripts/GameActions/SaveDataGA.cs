using UnityEngine;

public class SaveDataGA : GameAction
{
    public EffectContext Context { get; private set; }
    public string Key { get; private set; }
    public object Data { get; private set; }

    public SaveDataGA(EffectContext context,string key, object data)
    {
        Context = context;
        Key = key;
        Data = data;
    }
    public void SimulatedPerform()
    {
        Context.AddData(Key, Data);
    }
}
