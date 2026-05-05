using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefinitionSEInfo : StatusEffectInfo
{
    [field: SerializeField] public override StatusEffect EnumKey { get; protected set; } = StatusEffect.UNIQUE;
    [field: SerializeField] public override string Name { get; protected set; }
    [field: SerializeField] public override Sprite Sprite { get; protected set; }
    [field: SerializeField] public override bool Stackable { get; protected set; } = true;
    [field: SerializeField] public override bool RemoveAtZero { get; protected set; } = true;
    [field: SerializeField] public override StatusEffectType Type { get; protected set; } = StatusEffectType.OTHER;
    [field: SerializeField] public override StatusEffectModification PreTurnModification { get; protected set; } = StatusEffectModification.NONE;
    [field: SerializeField] public override StatusEffectModification PostTurnModification { get; protected set; } = StatusEffectModification.NONE;

    [field: SerializeReference, SR] public override CombatantTargetEffect Effect { get; protected set; } = null;
    [field: SerializeField] public override List<ConditionalModifierPair> Modifiers { get; protected set; } = null;
    [field: SerializeReference, SR] public override List<StatusEffectReaction> Reactions { get; protected set; } = null;

    [field: SerializeField,TextArea(2, 4)] public override string Description { get; protected set; }
}
