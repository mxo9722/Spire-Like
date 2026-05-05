using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnSEInfo : StatusEffectInfo
{
    private static Dictionary<StatusEffectInfo, Sprite> _modifiedSprites = new();

    [field: SerializeReference, SR] private SEIdentifier _futureStatusEffect;

    public override StatusEffect EnumKey { get => StatusEffect.UNIQUE; protected set { } }
    public override string Name { get => "Pre "+_futureStatusEffect.GetSEInfo().Name; protected set { } }
    public override Sprite Sprite { get => _futureStatusEffect.GetSEInfo().Sprite; protected set { } }
    public override bool Stackable { get => _futureStatusEffect.GetSEInfo().Stackable; protected set { } }
    public override bool RemoveAtZero { get => _futureStatusEffect.GetSEInfo().RemoveAtZero; protected set { } }
    public override StatusEffectType Type { get => _futureStatusEffect.GetSEInfo().Type; protected set { } }
    public override StatusEffectModification PreTurnModification { get => StatusEffectModification.APPLY_EFFECT; protected set { } }
    public override StatusEffectModification PostTurnModification { get => StatusEffectModification.NONE; protected set { } }
    public override CombatantTargetEffect Effect { get => new ConvertStatusEffectEffect(this, _futureStatusEffect.GetSEInfo(), -1); protected set { } }
    public override List<ConditionalModifierPair> Modifiers { get => new(); protected set { } }
    public override List<StatusEffectReaction> Reactions { get => new(); protected set { } }
    public override string Description { get => "Becomes "+ _futureStatusEffect.GetSEInfo()+" at the start of their next turn."; protected set { } }

    public NextTurnSEInfo() { }
    public NextTurnSEInfo(SEIdentifier futureStatusEffect)
    {
        _futureStatusEffect = futureStatusEffect;
    }
}
