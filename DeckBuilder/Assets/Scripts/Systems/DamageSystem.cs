using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    [SerializeField] private GameObject _damageVFX;
    [SerializeField] private GameObject _healVFX;
    [field: SerializeField] public Gradient HealthGradiant;

    public const float VULNERABLE_MULITPLIER = 1.5f;
    public const float WEAK_MULITPLIER = 0.75f;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
        ActionSystem.AttachPerformer<HealUnitsGA>(HealUnitsPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
        ActionSystem.DetachPerformer<HealUnitsGA>();
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA) 
    {
        int baseDamage = dealDamageGA.Amount;

        if (dealDamageGA.Caster != null && dealDamageGA.IsAttack)
        {
            baseDamage = Mathf.Max(dealDamageGA.Amount + dealDamageGA.Caster.GetStatusEffectStacks(StatusEffect.STRENGTH), 0);

            float multiplier = 1;

            if (dealDamageGA.Caster != null && dealDamageGA.Caster.GetStatusEffectStacks(StatusEffect.WEAK) > 0)
            {
                multiplier *= WEAK_MULITPLIER;
            }

            baseDamage = Mathf.FloorToInt(multiplier * baseDamage);
        }

        foreach (CombatantView target in dealDamageGA.Targets)
        {
            int individualDamage = baseDamage;

            if (target.CurrentHealth == 0)
                continue;

            if (target.GetStatusEffectStacks(StatusEffect.VULNERABLE) > 0)
                individualDamage = Mathf.FloorToInt(individualDamage * VULNERABLE_MULITPLIER);

            (int UnblockedDamage, int Overkill) result = target.Damage(individualDamage);

            dealDamageGA.SetUnblockedDamage(result.UnblockedDamage);
            dealDamageGA.SetOverkill(result.Overkill);

            Instantiate(_damageVFX, target.transform.position, Quaternion.Euler(0, 0, Random.value % 360));
        }

        foreach (CombatantView target in dealDamageGA.Targets)
        {
            yield return target.WaitForTweensComplete();
        }
    }

    private IEnumerator HealUnitsPerformer(HealUnitsGA healUnitsGA)
    {
        if (healUnitsGA.Amount <= 0)
            yield break;

        foreach(CombatantView target in healUnitsGA.Targets)
        {
            target.Heal(healUnitsGA.Amount);

            Instantiate(_healVFX, target.transform.position + new Vector3(0,0,-2), Quaternion.identity);
        }

        yield return new WaitForSeconds(1);
    }

    public static string CardDamageTextFromAttack(int baseDamage, EffectContext context, List<CombatantView> targets = null)
    {
        int damage = GetDamageFromAttack(baseDamage, context, targets);

        if(baseDamage > damage)
        {
            return "<color=\"red\">" + damage.ToString() + "</color>";
        }
        else if(baseDamage < damage)
        {
            return "<color=#4CBB17>" + damage.ToString() + "</color>";
        }

        return baseDamage.ToString();
    }
    
    public static string EnemyDamageTextFromAttack(int baseDamage, EffectContext context, List<CombatantView> targets)
    {
        int damage = GetDamageFromAttack(baseDamage, context, targets);

        if(targets.Count > 0)
        {
            return "<color=\"yellow\">" + damage.ToString() + "</color>";
        }

        return damage.ToString();
    }

    public static int GetDamageFromAttack(int damage, EffectContext context, List<CombatantView> targets = null)
    {
        if (context.Caster == null && context.PlayedCard != null)
            context.SetCaster(context.PlayedCard.GetOwnerView(context));
        if (context.Caster == null)
            return damage;

        damage = Mathf.Max(0, damage + context.Caster.GetStatusEffectStacks(StatusEffect.STRENGTH));

        float multiplier = 1.0f;

        if (context.Caster.GetStatusEffectStacks(StatusEffect.WEAK) > 0)
            multiplier *= WEAK_MULITPLIER;

        if (targets != null && targets.TrueForAll(e => e.GetStatusEffectStacks(StatusEffect.VULNERABLE) > 0) && targets.Count > 0)
            multiplier *= VULNERABLE_MULITPLIER;

        return Mathf.FloorToInt(damage * multiplier);
    }
}