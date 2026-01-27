using UnityEngine;

public class MatchEndSystem : Singleton<MatchEndSystem>
{
    [SerializeField] private ScenePicker _mapScene;

    public bool GameOver { get; private set; } = false;

    public void EndCombat()
    {
        GameOver = true;
        CombatRoom combatRoom = (CombatRoom)RunSystem.Instance.GetRoom();

        if (combatRoom == null)
        {
            combatRoom = MatchSetUpSystem.Instance.Room;
        }
        
        combatRoom?.SetCompleted();
        RewardSystem.Instance.DisplayRewards(combatRoom.Rewards, ReturnToMap);

        RunSystem.Instance.Hero1.SetCurrentHealth(HeroSystem.Instance.HeroViews[0].CurrentHealth);
        RunSystem.Instance.Hero2.SetCurrentHealth(HeroSystem.Instance.HeroViews[1].CurrentHealth);
        RunSystem.Instance.SaveRun();
    }

    public void ReturnToMap()
    {
        
        _mapScene.LoadScene();
    }
}
