using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPC")]
public class NPCData : ScriptableObject
{
    [field:SerializeField] public Sprite Image { get; private set; }
    [field:SerializeField] public int Health { get; private set; }
    [field: SerializeField, SerializedDictionary("Type", "Amount")] public SerializedDictionary<StatusEffectType, int> StatusEffects { get; private set; }
    [field: SerializeField] public List<NPCAction> ActionPattern { get; private set; }
}
