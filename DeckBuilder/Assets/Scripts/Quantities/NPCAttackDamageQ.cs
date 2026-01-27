using UnityEngine;

public class NPCAttackDamageQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        if(effectContext.Caster is NPCView npcView)
        {
            return npcView.CurrentAction.GetDamage(effectContext);
        }

        return 0;
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
