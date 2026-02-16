using AYellowpaper.SerializedCollections;
using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPC")]
public class NPCData : ScriptableObject
{
    [field:SerializeField] public Sprite Image { get; private set; }
    [field:SerializeField] public int Health { get; private set; }
    [field:SerializeField, Min(0)] public int RandomHealthMod { get; private set; }
    [field: SerializeField, SerializedDictionary("Type", "Amount")] public SerializedDictionary<StatusEffect, QuantityHolder> StatusEffects { get; private set; }
    [field: SerializeField] public List<NPCAction> ActionPattern { get; private set; }
}

[System.Serializable]
public class QuantityHolder
{
    [field:SerializeReference, SR] public Quantity Quantity { get; private set; }
}