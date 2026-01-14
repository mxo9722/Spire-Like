using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Perk
{
    public Sprite Image => _data.Image;
    public string Name => _data.name;
    public string Description => _data.Description;

    private readonly PerkData _data;
    private readonly List<PerkReaction> _reactions;

    public Perk(PerkData perkData)
    {
        _data = perkData;
        _reactions = perkData.PerkReactions;
    }

    public void OnAdd()
    {
        foreach(PerkReaction reaction in _reactions)
            reaction.SubscribeCondition(Reaction);
    }

    public void OnRemove()
    {
        foreach (PerkReaction reaction in _reactions)
            reaction.UnsubscribeCondition();
    }

    private void Reaction(PerkReaction reaction, GameAction gameAction)
    {
        if (reaction.PerkCondition.SubConditionIsMet(gameAction))
        {
            foreach (AutoCombatantTargetEffect autoTargetEffect in reaction.AutoTargetEffects)
            {
                List<CombatantView> targets = new();

                if (reaction.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
                {
                    targets.Add(haveCaster.Caster);
                }
                else
                {
                    EffectContext targetModeContext = new();

                    targets.AddRange(autoTargetEffect.TargetMode.GetTargets(targetModeContext));
                }

                GameAction perkEffectAction = autoTargetEffect.Effect.GetGameAction(EffectContext.CreateHeroEC(), combatantTargets: targets);
                ActionSystem.Instance.AddReaction(perkEffectAction);
            }
        }
    }

}
