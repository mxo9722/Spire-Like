using SerializeReferenceEditor;
using UnityEngine;

public class SaveQuantToPlayedCardEffect : NoTargetEffect
{
    [SerializeField] private string _key;
    [SerializeReference, SR] private Quantity _quantity = new SetQ(); 

    protected override GameAction GetGameAction(EffectContext context)
    {
        int value = _quantity.GetAmount(context);

        SaveDataGA saveDataGA = new(context, _key, value, SaveDataLevel.CARD);

        return saveDataGA;
    }
}
