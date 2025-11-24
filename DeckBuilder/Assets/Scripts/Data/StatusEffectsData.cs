using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectsData", menuName = "Data/StatusEffectsData")]
public class StatusEffectsData : ScriptableObject
{
    [SerializedDictionary("Status Effect Type", "Properties")]
    [SerializeField] public SerializedDictionary<StatusEffectType, StatusEffectInfo> Map = new();
}
