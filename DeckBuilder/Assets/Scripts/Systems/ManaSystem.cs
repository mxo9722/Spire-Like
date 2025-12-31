using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI _manaUI;
    private const int MAX_MANA = 3;
    private int _currentMana = 3;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<NPCTurnGA>(this, EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<NPCTurnGA>(this, EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void UpdateManaText()
    {
        _manaUI.UpdateManaText(_currentMana);
    }

    public bool HasEnoughMana(int mana)
    {
        return _currentMana >= mana;
    }

    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        _currentMana -= spendManaGA.Amount;
        _manaUI.UpdateManaText(_currentMana);
        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        _currentMana = MAX_MANA;
        _manaUI.UpdateManaText(_currentMana);
        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }

    private void EnemyTurnPostReaction(NPCTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
