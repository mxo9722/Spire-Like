using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [TextArea(2, 4)]
    [SerializeField] private string _description;
    public string Description { get => _description; }
    [Min(0)]
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public bool ExhuastOnUse { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;
    [field: SerializeReference, SR] public List<AutoTargetEffect> OtherEffects { get; private set; } = null;
}
