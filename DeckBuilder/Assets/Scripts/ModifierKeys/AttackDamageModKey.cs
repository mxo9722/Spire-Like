using UnityEngine;

public class AttackDamageModKey : ModifierKey
{

    public CombatantView Attacker { get; private set; }
    public CombatantView Target { get; private set; }

    public AttackDamageModKey(CombatantView attacker, CombatantView target, EffectContext context) : base(context)
    {
        Attacker = attacker;
        Target = target;
    }
}
