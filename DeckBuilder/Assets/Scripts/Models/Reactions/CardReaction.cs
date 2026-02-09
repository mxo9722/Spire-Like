using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardReaction
{
    [SerializeReference, SR] private CardReactionCondition _reactionCondition;
    [SerializeReference, SR] private List<AutoTargetEffect> _effects;

    private Card _owner;

    public void SetUp(Card owner)
    {
        _owner = owner;
        _reactionCondition.SetUp(owner);
    }

    public void Subscribe()
    {
        _reactionCondition.SubscribeCondition(Reaction);
    }

    public void Unsubscribe()
    {
        _reactionCondition.UnsubscribeCondition(Reaction);
    }


    public virtual IDynamicEffectText[] GetDynamicTextEffects()
    {
        List<IDynamicEffectText> dets = new();

        foreach (AutoTargetEffect effect in _effects)
        {
            dets.AddRange(effect.GetDynamicTextEffects());
        }

        return dets.ToArray();
    }

    public virtual string ApplyDynamicTextEffect(string description, int startIndex, EffectContext context, Card card)
    {
        foreach (AutoTargetEffect effect in _effects)
        {
            description = effect.ApplyDynamicTextEffect(description, startIndex, context, card);
        }

        return description;
    }


    private void Reaction(GameAction gameAction)
    {
        if (_reactionCondition.SubConditionIsMet(gameAction))
        {
            int index = CardSystem.Instance.GetHandIndex(_owner);
            CardView cardView = null;

            if (index != -1)
            {
                cardView = CardSystem.Instance.GetViewFromHand(_owner, true);
                SpotlightCardGA spotLightGA = new(cardView);
                ActionSystem.Instance.AddReaction(spotLightGA);
            }

            EffectContext context = new(_owner.GetOwnerView(), playedCard: _owner);

            MultipleEffectsGA multipleEffectsGA = new(context, _effects);
            ActionSystem.Instance.AddReaction(multipleEffectsGA);

            index = CardSystem.Instance.GetHandIndex(_owner);

            if (index != -1 && cardView != null && SpotlightSystem.Instance.SpotlightCardViews.Contains(cardView))
            {
                UnspotlightCardGA unspotlightCardGA = new(cardView);
                ActionSystem.Instance.AddReaction(unspotlightCardGA);
            }
        }
    }

    public CardReaction Clone()
    {
        CardReaction cardReaction = new();

        cardReaction._reactionCondition = _reactionCondition.Clone();
        cardReaction._effects = _effects;

        return cardReaction;
    }
}
