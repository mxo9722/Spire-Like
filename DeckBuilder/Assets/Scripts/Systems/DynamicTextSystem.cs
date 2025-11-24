using UnityEngine;

public class DynamicTextSystem : Singleton<DynamicTextSystem>
{
    public void UpdateDynamicValues()
    {
        CardSystem.Instance.UpdateDynamicDescriptions();
        EnemySystem.Instance.UpdateEnemiesBehaviorUI();
    }
}
