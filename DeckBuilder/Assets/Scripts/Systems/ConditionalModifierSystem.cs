using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalModifierSystem : Singleton<ConditionalModifierSystem>
{

    public enum ModifierTiming { EARLY, MID, LATE}
    public delegate int ModifierDelegate(int oValue, ModifierKey modKey);

    private class SubDictionary : Dictionary<Type, Dictionary<object, ModifierDelegate>> { }

    private static SubDictionary _earlyModifierDictionary = new();
    private static SubDictionary _midModifierDictionary = new();
    private static SubDictionary _lateModifierDictionary = new();

    public static void Subscribe<T>(ModifierDelegate action, object subscriber, ModifierTiming timing) where T : ModifierKey
    {
        Type type = typeof(T);
        SubDictionary _subDictionary = GetSubscriptions(timing);

        if (!_subDictionary.ContainsKey(type))
        {
            _subDictionary.Add(type, new());
        }

        _subDictionary[type].Add(subscriber, action);
    }

    public static void Unsubscribe<T>(object subscriber, ModifierTiming timing)
    {
        Type type = typeof(T);
        SubDictionary _subDictionary = GetSubscriptions(timing);

        if (_subDictionary.ContainsKey(type) && _subDictionary[type].ContainsKey(subscriber))
        {
            _subDictionary[type].Remove(subscriber);
        }
    }

    private static SubDictionary GetSubscriptions(ModifierTiming timing)
    {
        switch (timing)
        {
            case ModifierTiming.EARLY:
                return _earlyModifierDictionary;
            case ModifierTiming.MID:
                return _midModifierDictionary;
            case ModifierTiming.LATE:
                return _lateModifierDictionary;
        }

        return null;
    }

    public int ModifyValue<T>(int oValue, T modKey) where T : ModifierKey
    {
        oValue = ModifyValue(oValue, modKey, ModifierTiming.EARLY);
        oValue = ModifyValue(oValue, modKey, ModifierTiming.MID);
        oValue = ModifyValue(oValue, modKey, ModifierTiming.LATE);

        return oValue;
    }
    
    public int ModifyValue<T>(int oValue, T modKey, ModifierTiming timing) where T : ModifierKey
    {
        Type type = typeof(T);
        SubDictionary _subDictionary = GetSubscriptions(timing);

        if (!_subDictionary.ContainsKey(type))
        {
            return oValue;
        }

        foreach(ModifierDelegate modDelegate in _subDictionary[type].Values)
        {
            oValue = modDelegate(oValue, modKey);
        }

        return oValue;
    }
}
