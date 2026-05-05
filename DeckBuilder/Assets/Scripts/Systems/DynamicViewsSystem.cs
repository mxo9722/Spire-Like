using UnityEngine;

public class DynamicViewsSystem : Singleton<DynamicViewsSystem>
{

    public void UpdateDynamicValues()
    {
        CardSystem.Instance.UpdateCardViews();
        BoardSystem.Instance.UpdateView();
        EnemySystem.Instance.UpdateEnemiesBehaviorUI();
    }
}
