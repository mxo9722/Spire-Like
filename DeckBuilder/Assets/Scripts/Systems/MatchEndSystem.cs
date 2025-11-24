using UnityEngine;

public class MatchEndSystem : Singleton<MatchEndSystem>
{
    [SerializeField] private ScenePicker _mapScene;

    public void EndCombat()
    {
        CombatRoom combatRoom = (CombatRoom)RunSystem.Instance.GetRoom();

        RewardSystem.Instance.Display(combatRoom.Rewards);
    }

    public void ReturnToMap()
    {
        Room room = RunSystem.Instance.GetRoom();
        room.SetCompleted();
        _mapScene.LoadScene();
    }
}
