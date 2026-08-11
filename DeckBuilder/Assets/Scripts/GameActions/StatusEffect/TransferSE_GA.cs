using System.Collections.Generic;
using UnityEngine;

public class TransferSE_GA : GameAction
{

    public CombatantView From { get; private set; }
    public List<CombatantView> To { get; private set; }
    public StatusEffectInfo SEType { get; private set; }
    public int MaxTransferAmount { get; private set; }

    public TransferSE_GA(CombatantView from, List<CombatantView> to, StatusEffectInfo sEType, int maxTransferAmount)
    {
        From = from;
        To = to;
        SEType = sEType;
        MaxTransferAmount = maxTransferAmount;
    }
}
