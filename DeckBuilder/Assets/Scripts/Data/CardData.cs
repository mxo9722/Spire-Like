using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField,TextArea(2, 4), Tooltip("\"{vt}\" is for the manual target value. \"{v#}\" is for any other dynamic value where # is the dynamic value number in the .")] 
    public string Description { get; private set; }
    [field: SerializeField, Min(0)] public int Mana { get; private set; }
    [field: SerializeField] public bool Unplayable = false;
    [field: SerializeReference, SR] public List<Condition> RequiredConditions { get; private set; } = null;
    [field: SerializeReference, SR] public List<Condition> HighlightConditions { get; private set; } = null;
    [field: SerializeField] public bool ExhuastOnUse { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public ManualTargetType ManualTargetType { get; private set; } = ManualTargetType.COMBATANT;
    [field: SerializeReference, SR] public List<CombatantFilter> CombatantFilters { get; private set; } = null;
    [field: SerializeReference, SR] public List<LaneFilter> LaneFilters { get; private set; } = null;
    [field: SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;
    [field: SerializeReference, SR] public List<AutoTargetEffect> OtherEffects { get; private set; } = null;
    [field: SerializeReference, SR] public List<AutoTargetEffect> TurnEndEffects { get; private set; } = null;
}
