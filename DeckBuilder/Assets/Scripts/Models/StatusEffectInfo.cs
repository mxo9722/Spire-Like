using System;
using UnityEngine;

public enum StatusEffectModification
{
    NONE,
    REMOVE_ALL,
    REMOVE_ONE
}

[Serializable]
public class StatusEffectInfo
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public bool Stackable { get; private set; } = true;
    [field: SerializeField] public StatusEffectModification PreTurnModification { get; private set; } = StatusEffectModification.NONE;
    [field: SerializeField] public StatusEffectModification PostTurnModification { get; private set; } = StatusEffectModification.NONE;
    [field: SerializeField,TextArea(2, 4)] public string Description { get; private set; }
    [field: SerializeField,TextArea(2, 4)] public string DescriptionNoStacks { get; private set; }

}
