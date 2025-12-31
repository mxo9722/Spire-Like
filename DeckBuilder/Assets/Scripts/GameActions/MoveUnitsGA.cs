using System.Collections.Generic;
using UnityEngine;

public class MoveUnitsGA : CombinableGameAction<MoveUnitsGA>, IHaveCaster
{
    public Dictionary<CombatantView, LaneView> Moves { get; private set; }
    public float AnimationDuration { get; private set; } = 0.15f;
    public float JumpValue { get; private set; } = 0;

    public CombatantView Caster { get; private set; } = null;

    public MoveUnitsGA(CombatantView caster)
    {
        Caster = caster;
        Moves = new();
    }

    public MoveUnitsGA(LaneView destinationLane, CombatantView target, CombatantView caster)
    {
        Moves = new();
        Moves.Add(target, destinationLane);
        Caster = caster;
    }

    public void AddMove(CombatantView combatantView, LaneView laneView)
    {
        if (Moves.ContainsKey(combatantView))
            Moves[combatantView] = laneView;
        else
            Moves.Add(combatantView, laneView);
    }

    public void SetAnimationDuration(float animDuration)
    {
        AnimationDuration = animDuration;
    }

    public void SetJumpValue(float jumpValue)
    {
        JumpValue = jumpValue;
    }

    public override void Combine(MoveUnitsGA other)
    {
        foreach(KeyValuePair<CombatantView, LaneView> move in other.Moves)
        {
            AddMove(move.Key, move.Value);
        }
    }
}
