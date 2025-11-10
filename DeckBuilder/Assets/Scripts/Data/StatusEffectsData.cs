using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectsData", menuName = "Data/StatusEffectsData")]
public class StatusEffectsData : ScriptableObject
{
    [SerializedDictionary("Status Effect Type", "Properties")]
    [SerializeField] public SerializedDictionary<StatusEffectType, StatusEffectInfo> Map = new();

    private void OnValidate()
    {

        Array keys = Enum.GetValues(typeof(StatusEffectType));

        foreach(StatusEffectType key in keys)
        {
            if (!Map.ContainsKey(key))
            {
                Map.TryAdd(key, new StatusEffectInfo());
            }
        }
    }
}
