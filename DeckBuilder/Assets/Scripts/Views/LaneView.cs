using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneView : MonoBehaviour
{
    [field: SerializeField] public Transform HeroSlot { get; private set; }
    [field: SerializeField] public List<Transform> EnemySlots { get; private set; }

    public List<EnemyView> EnemyViews { get; private set; } = new();
    public HeroView HeroView { get; private set; } = null;
    public bool Dead { get; private set; } = false;

    public Transform FirstAvailableSlot()
    {
        foreach (Transform slot in EnemySlots)
        {
            if (slot.childCount == 0)
                return slot;
        }

        return null;
    }

    public IEnumerator SlideEnemiesLeft(float duration,bool pauseForMove = false)
    {
        Tween tween = null;

        //for (int x = 0; x < EnemySlots.Count - 1; x++)
        //{
        //    Transform pullSlot = EnemySlots[x];

        //    if (pullSlot.childCount > 0)
        //        continue;

        //    for(int y = x + 1; y < EnemySlots.Count; y++)
        //    {
        //        Transform pushSlot = EnemySlots[y];

        //        if(pushSlot.childCount == 1)
        //        {
        //            Transform movedChild = pushSlot.GetChild(0);
        //            movedChild.parent = pullSlot;

        //            List<Tween> tweens = DOTween.TweensByTarget(movedChild);

        //            if(tweens != null)
        //            foreach (Tween t in tweens)
        //                yield return t.WaitForCompletion();

        //            tween = movedChild.DOLocalMove(Vector3.zero, duration);
        //            break;
        //        }
        //    }
        //}

        for(int i=0;i<EnemyViews.Count;i++)
        {
            EnemyView enemyView = EnemyViews[i];
            Transform enemySlot = EnemySlots[i];

            if (enemyView.transform.parent != enemySlot)
            {
                enemyView.transform.parent = enemySlot;

                List<Tween> tweens = DOTween.TweensByTarget(enemyView.transform);

                if(tweens != null)
                {
                    foreach (Tween t in tweens)
                        yield return t;
                }

                tween = enemyView.transform.DOLocalMove(Vector3.zero,duration);
            }
        }

        if (tween != null && pauseForMove)
            yield return tween.WaitForCompletion();
    }

    public void AddEnemy(EnemyView enemyView, LaneView originalLaneView = null)
    {
        Transform slot = FirstAvailableSlot();

        if (slot == null)
            return;

        enemyView.transform.parent = slot;
        EnemyViews.Add(enemyView);

        if (originalLaneView != null)
        {
            MoveEnemyGA moveEnemyGA = new(this, enemyView);
            ActionSystem.Instance.AddReaction(moveEnemyGA);
        }
        else
        {
            enemyView.transform.localPosition = Vector3.zero;
        }
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        if (!EnemyViews.Contains(enemyView))
            yield break;

        EnemyViews.Remove(enemyView);

        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enemyView.gameObject);
        yield return SlideEnemiesLeft(0.2f);
    }

    public void SetHero(HeroView heroView, LaneView originalLaneView = null)
    {
        if (HeroView != null)
        {
            //TODO: Add swap hero functionality
            return;
        }

        heroView.transform.parent = HeroSlot;
        HeroView = heroView;

        if (originalLaneView == null)
            heroView.transform.localPosition = Vector3.zero;
        else
        {
            //TODO: Add move hero functionality
        }
    }

    public IEnumerator SwapHero(HeroView heroView, float duration)
    {
        HeroView = heroView;

        if (heroView != null)
        {
            heroView.transform.parent = HeroSlot;
            yield return heroView.transform.DOLocalMove(Vector3.zero, duration).WaitForCompletion();
        }
    }
}
