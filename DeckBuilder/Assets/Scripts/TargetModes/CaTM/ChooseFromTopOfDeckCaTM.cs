using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFromTopOfDeckCaTM : CardTargetMode, INeedsUserInput
{

    [SerializeReference, SR] private Quantity _numberFromTop = new SetQ(1);
    [Tooltip("Leave min amount to null if min should be the same amount as the max")]
    [SerializeReference, SR] private Quantity _minAmount = null;
    [SerializeReference, SR] private Quantity _amount = new SetQ(1);

    [SerializeField] private string _selectedCountKey = "";
    private List<Card> _selected = new();

    public override List<Card> GetTargets(EffectContext targetModeContext)
    {
       return _selected;
    }

    public IEnumerator WaitForUserInput(EffectContext context)
    {
        int fromTop = _numberFromTop.GetAmount(context);
        int max = _amount.GetAmount(context);
        int min;

        List<Card> deck = CardSystem.Instance.GetDrawPile();

        if (deck.Count > fromTop)
            deck = deck.GetRange(0, fromTop);

        if (_minAmount == null)
            min = max;
        else
            min = _minAmount.GetAmount(context);

        CardCollectionSystem.Instance.SelectionDisplay(deck, min, max, true);

        while (CardCollectionSystem.Instance.WaitingForSelection)
            yield return new WaitForSeconds(0.1f);

        _selected = CardCollectionSystem.Instance.GetCardSelections();

        if (!string.IsNullOrEmpty(_selectedCountKey))
            context.SetData(_selectedCountKey, _selected.Count);
    }
}
