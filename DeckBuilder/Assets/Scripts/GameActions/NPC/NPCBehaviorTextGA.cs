using UnityEngine;

public class NPCBehaviorTextGA : GameAction
{
    public NPCView Target { get; private set; }
    public string BehaviorName { get; private set; }

    public NPCBehaviorTextGA(NPCView target, string behaviorName)
    {
        Target = target;
        BehaviorName = behaviorName;
    }
}
