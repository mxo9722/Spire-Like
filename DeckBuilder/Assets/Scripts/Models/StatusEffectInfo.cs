using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectModification
{
    NONE,
    REMOVE_ALL,
    REMOVE_ONE,
    APPLY_EFFECT
}

[Serializable]
public class StatusEffectInfo
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public bool Stackable { get; private set; } = true;
    [field: SerializeField] public StatusEffectModification PreTurnModification { get; private set; } = StatusEffectModification.NONE;
    [field: SerializeField] public StatusEffectModification PostTurnModification { get; private set; } = StatusEffectModification.NONE;

    [field: SerializeReference, SR] public CombatantTargetEffect Effect { get; private set; } = null;
    [field: SerializeReference, SR] public List<StatusEffectReaction> Reactions { get; private set; } = null;

    [field: SerializeField,TextArea(2, 4)] public string Description { get; private set; }
}
