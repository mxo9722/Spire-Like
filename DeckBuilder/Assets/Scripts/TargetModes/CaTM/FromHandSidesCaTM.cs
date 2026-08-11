using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class FromHandSidesCaTM : CardTargetMode
{
    private enum Side { LEFT, RIGHT }

    [SerializeReference, SR] private Quantity _amount = new SetQ(1);
    [SerializeField] private Side _side;

    public override List<Card> GetTargets(EffectContext context)
    {
        int amount = _amount.GetAmount(context);

        List<Card> hand = CardSystem.Instance.GetHand();

        if (amount >= hand.Count)
            return hand;

        switch (_side)
        {
            case Side.LEFT:
                hand.RemoveRange(amount, hand.Count - amount);
                break;
            case Side.RIGHT:
                hand.RemoveRange(0, hand.Count - amount);
                break;
        }

        return hand;
    }
}
