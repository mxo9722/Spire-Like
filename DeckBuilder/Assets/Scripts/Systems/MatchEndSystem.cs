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

        RunSystem.Instance.RunData.SetHealth(HeroSystem.Instance.HeroView.CurrentHealth);
        RunSystem.Instance.SaveRun();
    }

    public void ReturnToMap()
    {
        
        _mapScene.LoadScene();
    }
}
