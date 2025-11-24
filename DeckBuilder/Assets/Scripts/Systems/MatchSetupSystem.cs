using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private PerkData _perkData;

    private void Start()
    {
        HeroSystem.Instance.Setup(_heroData);

        CombatRoom combatRoom = (CombatRoom) RunSystem.Instance.RunData.Room;

        EnemySystem.Instance.Setup(combatRoom.TopRow, 0);
        EnemySystem.Instance.Setup(combatRoom.MiddleRow, 1);
        EnemySystem.Instance.Setup(combatRoom.BottomRow, 2);

        CardSystem.Instance.SetUp(RunSystem.Instance.RunData.Deck);
        
        foreach(Perk perk in RunSystem.Instance.RunData.Perks)
            PerkSystem.Instance.AddPerk(new Perk(_perkData));
        
        DrawCardsGA drawCardsGA = new(5);
        ManaSystem.Instance.UpdateManaText();
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
