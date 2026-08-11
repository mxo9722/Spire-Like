using System.Collections.Generic;
using UnityEngine;

public class EffectContext : IHoldData
{
    public CombatantView Caster { get; private set; }
    public LaneView TargetLane { get; private set; }
    public CombatantView TargetCombatant { get; private set; }
    public Card PlayedCard { get; private set; }

    public int PlayedHandIndex { get; private set; }
    public int PlayedHandSize { get; private set; }

    private Dictionary<string, object> _data = null;

    public EffectContext(CombatantView caster = null, LaneView manualTargetLane = null, CombatantView manualTargetCombatant = null, Card playedCard = null, int playedHandIndex = -1, int playedHandSize = -1)
    {
        Caster = caster;
        TargetLane = manualTargetLane;
        TargetCombatant = manualTargetCombatant;
        PlayedCard = playedCard;
        PlayedHandIndex = playedHandIndex;
        PlayedHandSize = playedHandSize;
    }

    public EffectContext(EffectContext copySource)
    {
        Caster = copySource.Caster;
        TargetLane = copySource.TargetLane;
        TargetCombatant = copySource.TargetCombatant;
        PlayedCard = copySource.PlayedCard;
        PlayedHandIndex = copySource.PlayedHandIndex;
        PlayedHandSize = copySource.PlayedHandSize;

        if (copySource._data != null)
            _data = new(copySource._data);
    }

    public void SetCaster(CombatantView caster)
    {
        Caster = caster;
    }

    public void SetManualCombatant(CombatantView combatant)
    {
        TargetCombatant = combatant;
    }
    
    public void SetManualLane(LaneView combatant)
    {
        TargetLane = combatant;
    }

    public void SetPlayIndex(int index)
    {
        PlayedHandIndex = index;
    }

    public void SetPlaySize(int size)
    {
        PlayedHandSize = size;
    }

    public void SetData(string key, object data)
    {
        if (_data == null)
            _data = new();

        if (string.IsNullOrWhiteSpace(key))
            return;

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

    public EffectContext Clone()
    {
        EffectContext context = new(Caster, TargetLane, TargetCombatant, PlayedCard);

        return context;
    }
}
