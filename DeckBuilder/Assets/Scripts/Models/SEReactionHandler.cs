using System.Collections.Generic;
using UnityEngine;

public class SEReactionHandler
{
    public StatusEffectInfo Info { get; private set; }

    public SEReactionHandler(StatusEffectInfo info)
    {
        Info = info;
    }

    public void HandleReaction(GameAction gameAction)
    {
        foreach (CombatantView Owner in BoardSystem.Instance.GetAllCombatants()) 
        {
            if (!Owner.HasStatusEffectUI(Info))
                continue;

            foreach (StatusEffectReaction reaction in Info.Reactions)
            {
                int count = reaction.SubConditionIsMet(Owner, gameAction);

                EffectContext context = new(Owner);

                context.SetData("stacks", Owner.GetStatusEffectStacks(Info));
                reaction.SaveTargetData(context, gameAction);

                for (int i = 0; i < count; i++)
                {
                    reaction.InvokeEffects(context);
                }
            }
        }
    }
}
