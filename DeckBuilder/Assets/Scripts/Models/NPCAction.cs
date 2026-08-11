using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NPCAction
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public NPCActionType Symbol { get; private set; } = default;
    [field: SerializeField] public NPCActionType Symbol2 { get; private set; } = NPCActionType.NONE;
    [field: SerializeField, Min(0)] public float Weight { get; private set; } = 1.0f;
    [field: SerializeField, Min(0)] public int Priority { get; private set; } = 0;
    [field: SerializeField, Min(0)] public int ConsecutiveMax { get; private set; } = 0;
    [field: SerializeReference, SR] public List<Condition> Conditions { get; private set; } = null;
    [field: SerializeReference, SR] public List<AutoTargetEffect> Effects { get; private set; } = null;

    public int GetDamage(EffectContext context)
    {
        int damage = 0;

        foreach (AutoTargetEffect autoTargetEffect in Effects)
        {
            if (autoTargetEffect is AutoCombatantTargetEffect acte)
            {
                List<CombatantView> targets = acte.TargetMode.GetTargets(context);

                foreach (Effect effect in autoTargetEffect.Effects)
                {
                    GameAction ga = effect.GetGameAction(context, targets);

                    if (ga is SimulatedGameAction simulatedGA)
                    {
                        simulatedGA.SimulatedPerform(context);
                    }

                    else if (effect is AttackHeroEffect attackHeroEffect)
                    {
                        damage += DamageSystem.GetDamageFromAttack(attackHeroEffect.Damage, context, targets);
                    }
                    else if (effect is MultiAttackFoeEffect multiAttackFoeEffect)
                    {
                        int baseDamage = multiAttackFoeEffect.Damage.GetAmount(context);
                        int count = multiAttackFoeEffect.AttackCount.GetAmount(context);

                        damage += baseDamage * count;
                    }
                }
            }
        }

        return damage;
    }
}
