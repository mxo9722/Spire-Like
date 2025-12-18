using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Perk
{
    public Sprite Image => _data.Image;

    private readonly PerkData _data;
    private readonly PerkCondition _condition;
    private readonly AutoCombatantTargetEffect _effect;

    public Perk(PerkData perkData)
    {
        _data = perkData;
        _condition = perkData.PerkCondition;
        _effect = perkData.AutoTargetEffect;
    }

    public void OnAdd()
    {
        _condition.SubscribeCondition(Reaction);
    }

    public void OnRemove()
    {
        _condition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        if (_condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();

            if(_data.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }
            if (_data.UseAutoTarget)
            {
                EffectContext targetModeContext = EffectContext.CreateHeroEC();

                targets.AddRange(_effect.TargetMode.GetTargets(targetModeContext));
            }

            GameAction perkEffectAction = _effect.Effect.GetGameAction(EffectContext.CreateHeroEC(), combatantTargets:targets);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }

}
