using System.Collections.Generic;
using UnityEngine;

public class KillNpcGA : CombinableGameAction<KillNpcGA>
{
    public List<NPCView> NPCViews { get; private set; }

    public KillNpcGA(List<NPCView> enemyViews)
    {
        NPCViews = enemyViews;
    }
    
    public KillNpcGA(NPCView enemyView)
    {
        NPCViews = new() { enemyView };
    }

    public override void Combine(KillNpcGA other)
    {
        NPCViews.AddRange(other.NPCViews);
    }
}
