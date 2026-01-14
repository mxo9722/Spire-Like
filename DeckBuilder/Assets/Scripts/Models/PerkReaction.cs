using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PerkReaction
{
    [field: SerializeReference, SR] public PerkCondition PerkCondition { get; private set; }

    [field: SerializeField] public bool UseActionCasterAsTarget { get; private set; } = true;
    [field: SerializeReference, SR] public List<AutoCombatantTargetEffect> AutoTargetEffects { get; private set; }

    private Action<PerkReaction, GameAction> _reaction;

    public void SubscribeCondition(Action<PerkReaction, GameAction> reaction)
    {
        PerkCondition.SubscribeCondition(Reaction);
    }

    public void UnsubscribeCondition()
    {
        _reaction = null;
        PerkCondition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        _reaction?.Invoke(this, gameAction);
    }
}
