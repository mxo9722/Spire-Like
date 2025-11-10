using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionSymbolData", menuName = "Data/EnemyActionSymbolData")]
public class EnemyActionSymbolData : ScriptableObject
{
    [SerializedDictionary("Status Effect Type", "Properties")]
    [SerializeField] public SerializedDictionary<EnemyActionSymbolType, Sprite> Map = new();

    private void OnValidate()
    {

        Array keys = Enum.GetValues(typeof(EnemyActionSymbolType));

        foreach (EnemyActionSymbolType key in keys)
        {
            if (!Map.ContainsKey(key))
            {
                Map.TryAdd(key, null);
            }
        }
    }
}
