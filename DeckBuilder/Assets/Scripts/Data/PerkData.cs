using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Perk")]
public class PerkData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public List<PerkReaction> PerkReactions { get; private set; }
    [field: SerializeField, TextArea(2,4)] public string Description { get; private set; }

}