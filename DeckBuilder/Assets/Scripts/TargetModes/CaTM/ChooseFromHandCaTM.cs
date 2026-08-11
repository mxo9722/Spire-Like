using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFromHandCaTM : CardTargetMode, INeedsUserInput, IDynamicEffectText
{
    [Tooltip("Leave min amount to null if min should be the same amount as the max")]
    [SerializeReference, SR] private Quantity _minAmount = null;
    [SerializeReference, SR] private Quantity _amount = new SetQ(1);
    [SerializeReference, SR] private List<CardFilter> _filters = new();

    [SerializeField] private bool _excludePlayedCard = true;
    [SerializeField] private string _cardCountKey = "";

    private List<Card> _selected = new();

    public override List<Card> GetTargets(EffectContext targetModeContext)
    {
        return _selected;
    }

    public override List<Card> AllPossibleTargets(EffectContext context, Card card = null)
    {
        if (_amount.GetAmount(context) == 0)
            new List<Card>();

        List<Card> cards = CardSystem.Instance.GetHand();

        if(_filters.Count > 0)
            cards = new(cards.ApplyFilters(_filters));

        if (context.PlayedCard != null)
            cards.Remove(context.PlayedCard);

        return cards;
    }

    public IEnumerator WaitForUserInput(EffectContext context)
    {
        List<Card> hand = CardSystem.Instance.GetHand();

        if (_filters.Count > 0)
            hand = new(hand.ApplyFilters(_filters));

        if (_excludePlayedCard)
            hand.Remove(context.PlayedCard);

        int max = _amount.GetAmount(context);
        int min;

        if (_minAmount == null)
            min = max;
        else
            min = _minAmount.GetAmount(context);

        CardCollectionSystem.Instance.SelectionDisplay(hand, min, max, false);

        while (CardCollectionSystem.Instance.WaitingForSelection)
            yield return new WaitForSeconds(0.1f);

        _selected = CardCollectionSystem.Instance.GetCardSelections();
        if (!string.IsNullOrEmpty(_cardCountKey))
            context.SetData(_cardCountKey, _selected.Count);
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        int val = _amount.GetAmount(context);
        return val.ToString();
    }

    public string GetStaticText()
    {
        int val = _amount.GetStaticAmount();
        return val.ToString();
    }
}
