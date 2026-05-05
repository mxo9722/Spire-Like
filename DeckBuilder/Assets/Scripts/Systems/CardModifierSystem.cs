using System.Collections;

public class CardModifierSystem : Singleton<CardModifierSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddCardModifierGA>(AddCardModifierGAPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddCardModifierGA>();
    }

    private IEnumerator AddCardModifierGAPerformer(AddCardModifierGA addCardModifierGA)
    {
        foreach(Card card in addCardModifierGA.Targets)
        {
            if(CardSystem.Instance.GetHand().Contains(card))
            {
                yield return CardSystem.Instance.HandCardThrob(card);
            }

            card.AddCardModifier(addCardModifierGA.CardModifier);
        }

        DynamicViewsSystem.Instance.UpdateDynamicValues();
    }
}
