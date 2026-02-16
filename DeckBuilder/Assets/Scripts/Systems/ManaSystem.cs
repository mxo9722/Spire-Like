using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI _manaUI;
    private const int MAX_MANA = 5;
    public int CurrentMana { get; private set; } = MAX_MANA;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<GainManaGA>(GainManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<NPCTurnGA>(this, EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<GainManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<NPCTurnGA>(this, EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void UpdateManaText()
    {
        _manaUI.UpdateManaText(CurrentMana);
    }

    public bool HasEnoughMana(int mana)
    {
        return CurrentMana >= mana;
    }

    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        CurrentMana -= spendManaGA.Amount;
        _manaUI.UpdateManaText(CurrentMana);
        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }
    
    private IEnumerator GainManaPerformer(GainManaGA gainManaGA)
    {
        CurrentMana += gainManaGA.Amount;
        _manaUI.UpdateManaText(CurrentMana);
        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }
    
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        CurrentMana = MAX_MANA;
        _manaUI.UpdateManaText(CurrentMana);
        CardSystem.Instance.UpdateCardViews();
        yield return null;
    }

    private void EnemyTurnPostReaction(NPCTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
        RefillDashesGA refillDashesGA = new();
        ActionSystem.Instance.AddReaction(refillDashesGA);
    }
}
