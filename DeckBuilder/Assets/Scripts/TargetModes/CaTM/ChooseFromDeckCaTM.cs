using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFromDeckCaTM : CardTargetMode, IUserInputTM
{

    [SerializeField, Min(1)] private int _amount;

    private List<Card> _selected = new();

    public override List<Card> GetTargets(EffectContext targetModeContext)
    {
       return _selected;
    }

    public IEnumerator WaitForUserInput()
    {
        List<Card> deck = CardSystem.Instance.GetDrawPile();
        CardCollectionSystem.Instance.SelectionDisplay(deck, _amount,true);

        while (CardCollectionSystem.Instance.WaitingForSelection)
            yield return new WaitForSeconds(0.1f);

        _selected = CardCollectionSystem.Instance.GetCardSelections();
    }
}
