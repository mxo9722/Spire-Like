using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnitsToLaneEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private LaneTargetMode _destination;
    [SerializeField] private bool _newDestinationPerTarget = false;

    [SerializeField, Min(0.01f)] private float _animationDuration = 0.15f;
    [SerializeField] private float _jumpValue = 0;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        MoveUnitsGA moveUnitsGA = new(context.Caster);

        moveUnitsGA.SetAnimationDuration(_animationDuration);
        moveUnitsGA.SetJumpValue(_jumpValue);

        List<LaneView> lanes = _destination.GetTargets(context);

        if (lanes.Count == 0)
            return null;

        LaneView lane = lanes[0];

        foreach (CombatantView target in combatantTargets)
        {
            moveUnitsGA.AddMove(target, lane);

            if (_newDestinationPerTarget)
                lane = _destination.GetTargets(context)[0];
        }

        return moveUnitsGA;
    }
}
