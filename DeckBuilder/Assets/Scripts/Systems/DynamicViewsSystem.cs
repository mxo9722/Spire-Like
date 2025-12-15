using UnityEngine;

public class DynamicViewsSystem : Singleton<DynamicViewsSystem>
{

    public void UpdateDynamicValues()
    {
        CardSystem.Instance.UpdateCardViews();
        EnemySystem.Instance.UpdateEnemiesBehaviorUI();
    }
}
