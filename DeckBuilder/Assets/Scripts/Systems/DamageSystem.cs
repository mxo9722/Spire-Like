using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    [SerializeField] private GameObject _damageVFX;

    public const float VULNERABLE_MULITPLIER = 1.5f;
    public const float WEAK_MULITPLIER = 0.75f;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA) 
    {
        int baseDamage = dealDamageGA.Amount;

        if (dealDamageGA.Caster != null && dealDamageGA.IsAttack)
        {
            baseDamage = Mathf.Max(dealDamageGA.Amount + dealDamageGA.Caster.GetStatusEffectStacks(StatusEffectType.STRENGTH), 0);

            float multiplier = 1;

            if (dealDamageGA.Caster != null && dealDamageGA.Caster.GetStatusEffectStacks(StatusEffectType.WEAK) > 0)
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

            if (target.GetStatusEffectStacks(StatusEffectType.VULNERABLE) > 0)
                individualDamage = Mathf.FloorToInt(individualDamage * VULNERABLE_MULITPLIER);

            target.Damage(individualDamage);
            Instantiate(_damageVFX, target.transform.position, Quaternion.Euler(0, 0, Random.value % 360));
            yield return new WaitForSeconds(0.15f);
        }

    }

    public static string CardDamageTextFromAttack(int baseDamage, CombatantView attacker, List<CombatantView> targets = null)
    {
        int damage = GetDamageFromAttack(baseDamage, attacker, targets);

        if(baseDamage > damage)
        {
            return "<color=\"red\">" + damage.ToString() + "</color>";
        }
        else if(baseDamage < damage)
        {
            return "<color=\"green\">" + damage.ToString() + "</color>";
        }

        return baseDamage.ToString();
    }
    
    public static string EnemyDamageTextFromAttack(int baseDamage, CombatantView attacker, List<CombatantView> targets)
    {
        int damage = GetDamageFromAttack(baseDamage, attacker, targets);

        if(targets.Count > 0)
        {
            return "<color=\"yellow\">" + damage.ToString() + "</color>";
        }

        return damage.ToString();
    }

    public static int GetDamageFromAttack(int damage, CombatantView attacker, List<CombatantView> targets = null)
    {
        damage = Mathf.Max(0, damage + attacker.GetStatusEffectStacks(StatusEffectType.STRENGTH));

        float multiplier = 1.0f;

        if (attacker.GetStatusEffectStacks(StatusEffectType.WEAK) > 0)
            multiplier *= WEAK_MULITPLIER;

        if (targets != null && targets.TrueForAll(e => e.GetStatusEffectStacks(StatusEffectType.VULNERABLE) > 0) && targets.Count > 0)
            multiplier *= VULNERABLE_MULITPLIER;

        return Mathf.FloorToInt(damage * multiplier);
    }
}