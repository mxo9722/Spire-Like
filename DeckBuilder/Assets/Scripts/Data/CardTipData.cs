using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardTipData", menuName = "Data/CardTipData")]
public class CardTipData : ScriptableObject
{
    [SerializedDictionary("Key Word", "Description")]
    [field: SerializeField] public SerializedDictionary<string, string> Map { get; private set; } = new();

    private void OnValidate()
    {

        Array keys = Enum.GetValues(typeof(StatusEffectType));

        foreach (StatusEffectType key in keys)
        {
            string keyString = key.ToString();

            if (!Map.ContainsKey(keyString))
            {
                Map.TryAdd(keyString, "");
            }
        }
    }
}
