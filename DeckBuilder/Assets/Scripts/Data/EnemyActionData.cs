using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionSymbolData", menuName = "Data/EnemyActionSymbolData")]
public class EnemyActionData : ScriptableObject
{
    [SerializedDictionary("Enemy Action Type", "Properties")]
    [SerializeField] public SerializedDictionary<EnemyActionType, EnemyIntent> EnemyActionTypes = new();
    
    [SerializedDictionary("Enemy Target Type", "Properties")]
    [SerializeField] public SerializedDictionary<EnemyTargetTypes, EnemyIntent> EnemyTargetTypes = new();

    private void OnValidate()
    {
        Array keys = Enum.GetValues(typeof(EnemyActionType));

        foreach (EnemyActionType key in keys)
        {
            if (!EnemyActionTypes.ContainsKey(key))
            {
                EnemyActionTypes.TryAdd(key, null);
            }
        }
        
        keys = Enum.GetValues(typeof(EnemyTargetTypes));

        foreach (EnemyTargetTypes key in keys)
        {
            if (!EnemyTargetTypes.ContainsKey(key))
            {
                EnemyTargetTypes.TryAdd(key, null);
            }
        }
    }
}
