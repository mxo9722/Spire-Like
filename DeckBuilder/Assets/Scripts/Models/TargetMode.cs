using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class TargetMode<T>
{
    public abstract List<T> GetTargets(EffectContext context);
    public virtual List<T> AllPossibleTargets(EffectContext context, Card card = null)
    {
        if(context.Caster != null)
            return GetTargets(context);
        else
        {
            List<T> targets = new();
            
            foreach(HeroView hero in HeroSystem.Instance.HeroViews)
            {
                EffectContext heroContext = new(hero, context.TargetLane, context.TargetCombatant, context.PlayedCard);
                targets.AddRange(GetTargets(heroContext));
            }

            return targets.Distinct().ToList();
        }
    }

    public virtual List<T> GetTargetsTrivial(EffectContext context) => GetTargets(context);

    public virtual bool IsRandom => false;

    public virtual IDynamicEffectText[] GetDynamicTextEffects()
    {
        if (this is IDynamicEffectText dynamicEffectText)
            return new[] { dynamicEffectText };
        return new IDynamicEffectText[0];
    }

    public virtual NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.NONE;
    }
}
