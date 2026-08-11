using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    BUFF,
    DEBUFF,
    POWER,
    OTHER
}

public enum StatusEffectModification
{
    NONE,
    REMOVE_ALL,
    REMOVE_ONE,
    APPLY_EFFECT
}

[Serializable]
public abstract class StatusEffectInfo
{
    public abstract StatusEffect EnumKey { get; protected set; }
    public abstract string Name { get; protected set; }
    public abstract Sprite Sprite { get; protected set; }
    public abstract bool Stackable { get; protected set; }
    public abstract bool RemoveAtZero { get; protected set; }
    public abstract StatusEffectType Type { get; protected set; }
    public abstract StatusEffectModification PreTurnModification { get; protected set; }
    public abstract StatusEffectModification PostTurnModification { get; protected set; }

    public abstract CombatantTargetEffect Effect { get; protected set; }
    public abstract List<ConditionalModifierPair> Modifiers { get; protected set; }
    public abstract List<StatusEffectReaction> Reactions { get; protected set; }

    public abstract string Description { get; protected set; }

    public override bool Equals(object obj)
    {
        if (base.Equals(obj))
            return true;

        if(obj is StatusEffectInfo other)
        {
            if (other.EnumKey != EnumKey) return false;
            if (other.Name != Name) return false;
            if (other.Sprite != Sprite) return false;
            if (other.Stackable != Stackable) return false;
            if (other.RemoveAtZero != RemoveAtZero) return false;
            if (other.Type != Type) return false;
            if (other.PreTurnModification != PreTurnModification) return false;
            if (other.PostTurnModification != PostTurnModification) return false;
            //if (other.Effect != Effect) return false;
            //if (other.Reactions != Reactions) return false;
            if (other.Description != Description) return false;

            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        //for (int i = 0; i < _allStatusEffects.Count; i++)
        //{
        //    StatusEffectInfo sei = _allStatusEffects[i];
        //    if (sei.Equals(this))
        //        return i;
        //}

        //_allStatusEffects.Add(this);
        //return _allStatusEffects.Count - 1;
        return Name.GetHashCode();
    }
}

public struct SEEqualityComparer : IEqualityComparer<StatusEffectInfo>
{
    public bool Equals(StatusEffectInfo x, StatusEffectInfo y)
    {
        return x.Equals(y);
    }

    public int GetHashCode(StatusEffectInfo obj)
    {
        return obj.Name.GetHashCode();
    }
}
