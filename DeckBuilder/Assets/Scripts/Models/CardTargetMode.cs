using System.Collections.Generic;
using UnityEngine;

public abstract class CardTargetMode : TargetMode<Card>
{
    public override NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.FOCUS_HERO;
    }
}
