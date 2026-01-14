using System.Collections.Generic;
using UnityEngine;

public class EffectContext : IHoldData
{
    public CombatantView Caster { get; private set; }
    public LaneView TargetLane { get; private set; }
    public CombatantView TargetCombatant { get; private set; }

    private Dictionary<string, object> _data = null;

    public EffectContext(CombatantView caster = null, LaneView manualTargetLane = null, CombatantView manualTargetCombatant = null)
    {
        Caster = caster;
        TargetLane = manualTargetLane;
        TargetCombatant = manualTargetCombatant;
    }

    public void AddData(string key, object data)
    {
        if (_data == null)
            _data = new();

        if (_data.ContainsKey(key))
            _data[key] = data;
        else
            _data.Add(key, data);
    }

    public T GetData<T>(string key)
    {
        if (_data == null || !_data.ContainsKey(key))
            return default(T);

        if (_data[key] is T t)
            return t;

        return default(T);
    }

    public bool ContainsKey(string key)
    {
        if (_data == null)
            return false;

        return _data.ContainsKey(key);
    }

    #region CREATION_UTILITY
    public static EffectContext CreateHeroEC()
    {
        return new(HeroSystem.Instance.HeroView);
    }

    public static EffectContext CreateHeroEC(CombatantView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetCombatant: target);
    }

    public static EffectContext CreateHeroEC(LaneView target)
    {
        return new(HeroSystem.Instance.HeroView, manualTargetLane: target);
    }

    public static EffectContext CreateNpcEC(CombatantView caster)
    {
        return new(caster);
    }
    #endregion
}
