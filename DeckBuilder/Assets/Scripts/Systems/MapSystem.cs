using UnityEngine;

public class MapSystem : Singleton<MapSystem>
{
    [field:SerializeField] private ScenePicker _combatScene = new();

    public void EnterCombat()
    {
        _combatScene.LoadScene();
    }

    public void EnterRoom(Room room)
    {
        RunSystem.Instance.RunData.SetRoom(room);
    }
}
