using System.Collections;
using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    [SerializeField] private GameObject _damageVFX;

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
        foreach(CombatantView target in dealDamageGA.Targets)
        {
            if (target.CurrentHealth == 0)
                continue;

            target.Damage(dealDamageGA.Amount);
            Instantiate(_damageVFX, target.transform.position, Quaternion.Euler(0, 0, Random.value % 360));
            yield return new WaitForSeconds(0.15f);
        }

    }
}