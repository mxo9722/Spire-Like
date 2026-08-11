using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        foreach (CombatantView target in dealDamageGA.Targets)
        {
            int individualDamage = dealDamageGA.Amount;

            if (dealDamageGA.IsAttack)
                individualDamage = GetDamageFromAttack(dealDamageGA.Amount, dealDamageGA.Context, new() { target });
            
            if (target.CurrentHealth == 0)
                continue;

            //if (target.GetStatusEffectStacks(StatusEffect.VULNERABLE) > 0)
            //    individualDamage = Mathf.FloorToInt(individualDamage * VULNERABLE_MULITPLIER);

            (int UnblockedDamage, int Overkill) result = target.Damage(individualDamage);

            if(dealDamageGA.IsAttack)
                target.ReportAttacked();

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
        int damage = GetDamageFromAttack(baseDamage, context, targets, false);

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
    
    public static string EnemyDamageTextFromAttack(int baseDamage, EffectContext context, List<CombatantView> targets, bool applyIndirect)
    {
        int damage = GetDamageFromAttack(baseDamage, context, new(), false);

        return damage.ToString();
    }

    public static int GetDamageFromAttack(int damage, EffectContext context, List<CombatantView> targets = null, bool applyIndirect = true)
    {
        float multiplier = 1.0f;

        AttackDamageModKey modKey = new(context.Caster, null, context);

        if (targets != null && targets.Count == 1)
            modKey = new(context.Caster, targets[0], context);

        if (context.PlayedCard != null)
        {
            ExtraAttackDamageCM extraAttackDamageCM = context.PlayedCard.GetCardModifier<ExtraAttackDamageCM>();

            if (extraAttackDamageCM != null)
                damage += extraAttackDamageCM.DamageBonus;
        }

        if (context.Caster == null && context.PlayedCard != null)
            context.SetCaster(context.PlayedCard.GetOwnerView(context));

        if (context.Caster != null)
        {
            damage += context.Caster.GetStatusEffectStacks(StatusEffect.STRENGTH);
            damage += context.Caster.GetStatusEffectStacks(StatusEffect.BOLD);

            if (context.Caster.GetStatusEffectStacks(StatusEffect.WEAK) > 0)
                multiplier *= WEAK_MULITPLIER;
        }

        if (targets != null)
        {
            if (targets.TrueForAll(e => e.GetStatusEffectStacks(StatusEffect.BRUISED) > 0) && targets.Count > 0)
                damage += targets.Min(t => t.GetStatusEffectStacks(StatusEffect.BRUISED));
            
            if (targets.TrueForAll(e => e.GetStatusEffectStacks(StatusEffect.VULNERABLE) > 0) && targets.Count > 0)
                multiplier *= VULNERABLE_MULITPLIER;
        }


        bool indirect = false;

        if (applyIndirect && context.Caster != null && targets != null) 
        {

            if (context.Caster is NPCView && targets.All(t => t.GetLaneDistance(context.Caster) > 0))
                indirect = true;
            else if (context.Caster.GetStatusEffectStacks(StatusEffect.FEINT) > 0)
                indirect = true;
            else if (targets.All(t => t.GetStatusEffectStacks(StatusEffect.ZIGZAG) > 0))
                indirect = true;

            if (indirect)
                multiplier *= EnemySystem.Instance.IndirectReduction;

        }

        damage = Mathf.FloorToInt(damage * multiplier);

        damage = ConditionalModifierSystem.Instance.ModifyValue(damage, modKey);

        if (indirect)
        {
            IndirectAttackModKey indirectAttackModKey = new(modKey.Attacker, modKey.Target, context);
            damage = ConditionalModifierSystem.Instance.ModifyValue(damage, indirectAttackModKey);
        }

        return Mathf.Max(damage, 0);
    }
}