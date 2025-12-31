using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionSymbolData", menuName = "Data/EnemyActionSymbolData")]
public class EnemyActionData : ScriptableObject
{
    [SerializedDictionary("Enemy Action Type", "Properties")]
    [SerializeField] public SerializedDictionary<NPCActionType, EnemyIntent> EnemyActionTypes = new();
    
    [SerializedDictionary("Enemy Target Type", "Properties")]
    [SerializeField] public SerializedDictionary<NPCTargetTypes, EnemyIntent> EnemyTargetTypes = new();

    private void OnValidate()
    {
        Array keys = Enum.GetValues(typeof(NPCActionType));

        foreach (NPCActionType key in keys)
        {
            if (!EnemyActionTypes.ContainsKey(key))
            {
                EnemyActionTypes.TryAdd(key, null);
            }
        }
        
        keys = Enum.GetValues(typeof(NPCTargetTypes));

        foreach (NPCTargetTypes key in keys)
        {
            if (!EnemyTargetTypes.ContainsKey(key))
            {
                EnemyTargetTypes.TryAdd(key, null);
            }
        }
    }
}
