using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFromDeckCaTM : CardTargetMode, IUserInputTM
{
    [Tooltip("Leave min amount to null if min should be the same amount as the max")]
    [SerializeReference, SR] private Quantity _minAmount = null;
    [SerializeReference, SR] private Quantity _amount = new SetQ(1);

    private List<Card> _selected = new();

    public override List<Card> GetTargets(EffectContext targetModeContext)
    {
       return _selected;
    }

    public IEnumerator WaitForUserInput(EffectContext context)
    {
        List<Card> deck = CardSystem.Instance.GetDrawPile();

        int max = _amount.GetAmount(context);
        int min;

        if (_minAmount == null)
            min = max;
        else
            min = _minAmount.GetAmount(context);

        CardCollectionSystem.Instance.SelectionDisplay(deck, min, max, true);

        while (CardCollectionSystem.Instance.WaitingForSelection)
            yield return new WaitForSeconds(0.1f);

        _selected = CardCollectionSystem.Instance.GetCardSelections();
    }
}
