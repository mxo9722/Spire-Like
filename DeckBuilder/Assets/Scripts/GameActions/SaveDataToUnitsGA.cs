using System.Collections.Generic;
using UnityEngine;

public class SaveDataToUnitsGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    public string Key { get; private set; }
    public object Value { get; private set; }

    public SaveDataToUnitsGA(List<CombatantView> targets, string key, object value)
    {
        Targets = targets;
        Key = key;
        Value = value;
    }
}
