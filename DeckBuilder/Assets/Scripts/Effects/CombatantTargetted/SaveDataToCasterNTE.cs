using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveDataToCasterNTE<T> : NoTargetEffect
{

    [SerializeField] private string _key = "DataKey";

    protected override GameAction GetGameAction(EffectContext context)
    {
        object value = GetData(context);
        SaveDataGA saveDataGA = new(context, _key, value, SaveDataLevel.COMBATANT);
        return saveDataGA;
    }

    public abstract object GetData(EffectContext context);
}

public class SaveBoolToCaster : SaveDataToCasterNTE<bool>
{
    [SerializeReference, SR] private List<Condition> _value;

    public override object GetData(EffectContext context)
    {
        return _value.TrueForAll(v => v.TestCondition(context));
    }
}

public class SaveIntToCaster : SaveDataToCasterNTE<int>
{
    [SerializeReference, SR] private Quantity _value = new SetQ();

    public override object GetData(EffectContext context)
    {
        return _value.GetAmount(context);
    }
}
