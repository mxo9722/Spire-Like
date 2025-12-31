using System.Collections.Generic;
using UnityEngine;

public class NPCTurnGA : GameAction
{
    public List<NPCView> Targets { get; private set; }

    public NPCTurnGA(List<NPCView> targets)
    {
        Targets = targets;
    }
}
