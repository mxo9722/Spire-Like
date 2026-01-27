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
        foreach (PerkReaction reaction in _reactions)
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
            CombatantView targets = null;

            if (reaction.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets = haveCaster.Caster;
            }

            EffectContext context = new(manualTargetCombatant: targets);

            reaction.PerkCondition.SaveTargetData(context, gameAction);

            foreach (AutoTargetEffect autoTargetEffect in reaction.AutoTargetEffects)
            {
                GameAction perkEffectAction = autoTargetEffect.GetGameAction(context);
                ActionSystem.Instance.AddReaction(perkEffectAction);
            }
        }
    }

}
