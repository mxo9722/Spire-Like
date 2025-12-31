using System.Collections.Generic;
using UnityEngine;

public class MoveCombatantInDirectionEffect : CombatantTargetEffect
{

    [SerializeField] private MovementDirection _direction;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        MoveUnitsGA moveUnitsGA = new(context.Caster);

        foreach(CombatantView target in combatantTargets)
        {
            LaneView oldLane = BoardSystem.Instance.GetCurrentLaneView(target);
            LaneView newLane = BoardSystem.Instance.GetLaneFromDirection(oldLane, _direction);
            if (oldLane != newLane)
                moveUnitsGA.AddMove(target, newLane);
        }

        return moveUnitsGA;
    }
}
