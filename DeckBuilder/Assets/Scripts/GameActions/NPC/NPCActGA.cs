using UnityEngine;

public class NPCActGA : GameAction
{
    public EffectContext Context { get; private set; }
    public NPCView NPC { get; private set; }

    public NPCActGA(EffectContext context,NPCView npc)
    {
        Context = context;
        NPC = npc;
    }
}
