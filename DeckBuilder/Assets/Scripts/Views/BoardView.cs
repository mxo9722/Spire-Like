using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private List<LaneView> _laneViews = new();
    [SerializeField] private Transform _wrapper;
    [SerializeField] private Transform _singleLaneTransform;

    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    private int _originalLaneCount;

    private List<Vector3> _lanePositions = new();

    public readonly int MAX_ENEMY_COUNT = 3;

    public void OnEnable()
    {
        _originalPosition = _wrapper.localPosition;
        _originalScale = _wrapper.localScale;
        _originalLaneCount = _laneViews.Count;

        foreach (LaneView laneView in _laneViews)
        {
            _lanePositions.Add(laneView.transform.localPosition);
        }
    }

    public HeroView CreateHero(HeroData heroData, int laneIndex = 0)
    {
        LaneView laneView = _laneViews[laneIndex];

        HeroView heroView = CombatantViewCreator.Instance.CreateHeroView(heroData, laneView.HeroSlot);

        laneView.SetHero(heroView);

        laneView.HeroSlot.AddCombatant(heroView);
        heroView.transform.localScale = Vector3.one;

        if(RunSystem.Instance.CurrentHealth > 0)
            heroView.SetHealth(RunSystem.Instance.CurrentHealth);

        return heroView;
    }

    public EnemyView CreateEnemy(EnemyData enemyData, int laneIndex)
    {
        LaneView laneView = _laneViews[laneIndex];

        SlotView slot = laneView.FirstAvailableEnemySlot();

        EnemyView enemyView = CombatantViewCreator.Instance.CreateEnemyView(enemyData, slot);
        enemyView.transform.localScale = Vector3.one;

        EnemySystem.DetermineEnemyBehaviour(enemyView);

        return enemyView;
    }

    public List<EnemyView> GetAllEnemies()
    {
        List<EnemyView> allEnemies = new();

        foreach (LaneView laneView in _laneViews)
            allEnemies.AddRange(laneView.EnemyViews);

        return allEnemies;
    }

    public List<CombatantView> GetAllCombatants()
    {
        List<CombatantView> allCombatants = new();

        foreach (LaneView laneView in _laneViews)
        {
            allCombatants.AddRange(laneView.EnemyViews);
            if (laneView.HeroView != null)
                allCombatants.Add(laneView.HeroView);
        }

        return allCombatants;
    }

    public List<LaneView> GetAllLanes() => _laneViews;

    public LaneView GetCurrentLaneView(EnemyView enemyView)
    {
        return _laneViews.Find(e => e.EnemyViews.Contains(enemyView));
    }

    public LaneView GetCurrentLaneView(HeroView heroView)
    {
        return _laneViews.Find(e => e.HeroView == heroView);
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        LaneView laneView = GetCurrentLaneView(enemyView);

        yield return laneView.RemoveEnemy(enemyView);
    }

    public IEnumerator RedistributeEnemies()
    {
        List<EnemyView> allEnemyViews = GetAllEnemies();

        int enemyCount = allEnemyViews.Count;
        int minCount = enemyCount / _laneViews.Count;

        if (minCount == 0)
        {
            CompressBoardGA compressBoardGA = new();

            ActionSystem.Instance.AddReaction(compressBoardGA);

            yield break;
        }

        int shortageIndex = _laneViews.FindIndex(e => e.EnemyViews.Count < minCount);

        if (shortageIndex != -1)
        {
            LaneView shortageLane = _laneViews[shortageIndex];

            LaneView excess = null;
            int bestDist = allEnemyViews.Count;
            int highestCount = 0;

            for (int excessIndex = 0; excessIndex < _laneViews.Count; excessIndex++)
            {
                LaneView laneView = _laneViews[excessIndex];

                int count = laneView.EnemyViews.Count;
                int dist = Mathf.Abs(excessIndex - shortageIndex);

                if (count > 0 && excessIndex != shortageIndex && (dist < bestDist || (count > highestCount && dist == bestDist)))
                {
                    bestDist = dist;
                    excess = laneView;
                    highestCount = count;
                }
            }

            MoveEnemyGA moveEnemyGA = new MoveEnemyGA(shortageLane, excess.EnemyViews.Last());
            ActionSystem.Instance.AddReaction(moveEnemyGA);

            ActionSystem.Instance.AddReaction(new RedistributeEnemiesGA());
        }
    }

    public IEnumerator MoveHero(MoveHeroGA moveHeroGA, float duration, bool pause = false)
    {
        HeroView heroView = moveHeroGA.DestinationLane.HeroView;

        LaneView originalLaneView = BoardSystem.Instance.GetCurrentLaneView(moveHeroGA.HeroView);

        IEnumerator tween1 = moveHeroGA.DestinationLane.SwapHero(moveHeroGA.HeroView, duration);
        IEnumerator tween2 = originalLaneView.SwapHero(heroView, duration);

        yield return tween1;
        yield return tween2;
        DynamicViewsSystem.Instance.UpdateDynamicValues();

    }

    public IEnumerator MoveEnemy(MoveEnemyGA moveEnemyGA, float duration, bool pause = false)
    {
        if (moveEnemyGA.EnemyView.CurrentHealth == 0)
            yield break;

        if (moveEnemyGA.DestinationLane.EnemyViews.Count >= MAX_ENEMY_COUNT)
            yield break;

        LaneView originalLaneView = GetCurrentLaneView(moveEnemyGA.EnemyView);

        originalLaneView.EnemyViews.Remove(moveEnemyGA.EnemyView);
        moveEnemyGA.DestinationLane.EnemyViews.Add(moveEnemyGA.EnemyView);

        var lane = moveEnemyGA.DestinationLane.FirstAvailableEnemySlot();
        lane.AddCombatant(moveEnemyGA.EnemyView, false);
        Coroutine wait = StartCoroutine(lane.PullCombatant(0.4f));

        yield return originalLaneView.SlideEnemiesLeft(duration);

        if (pause)
            yield return wait;

        DynamicViewsSystem.Instance.UpdateDynamicValues();
    }

    public IEnumerator CompressBoard()
    {
        if (_laneViews.Count == 1)
            yield break;

        List<int> emptyIndexes = new();

        int index = -1;

        do
        {
            index = _laneViews.FindIndex(index + 1, e => !e.EnemyViews.Any());
            if(index != -1)
                emptyIndexes.Add(index);
        } 
        while (index != -1 && index < _laneViews.Count - 1);

        int heroIndex = _laneViews.FindIndex(e => e.HeroView == HeroSystem.Instance.HeroView);

        foreach (int emptyIndex in emptyIndexes)
        {
            if (emptyIndex != -1 && emptyIndex == heroIndex)
            {
                RemoveLaneGA removeLaneGA;

                bool beginPull = false;

                if (heroIndex != _laneViews.Count - 1)
                {
                    for (int i = heroIndex; i < _laneViews.Count - 1; i++)
                    {
                        LaneView pullLaneView = _laneViews[i];
                        LaneView pushLaneView = _laneViews[i + 1];

                        if (!pullLaneView.EnemyViews.Any())
                            beginPull = true;

                        if (beginPull)
                        {
                            foreach (EnemyView enemyView in pushLaneView.EnemyViews)
                            {
                                MoveEnemyGA moveEnemyGA = new(pullLaneView, enemyView);
                                ActionSystem.Instance.AddReaction(moveEnemyGA);
                            }
                        }
                    }

                    removeLaneGA = new(_laneViews.Last());
                }
                else
                {
                    for (int i = heroIndex; i > 0; i--)
                    {
                        LaneView pullLaneView = _laneViews[i];
                        LaneView pushLaneView = _laneViews[i - 1];

                        if (!pullLaneView.EnemyViews.Any())
                            beginPull = true;

                        if (beginPull)
                        {
                            foreach (EnemyView enemyView in pushLaneView.EnemyViews)
                            {
                                MoveEnemyGA moveEnemyGA = new(pullLaneView, enemyView);
                                ActionSystem.Instance.AddReaction(moveEnemyGA);
                            }
                        }
                    }

                    removeLaneGA = new(_laneViews.First());
                }

                ActionSystem.Instance.AddReaction(removeLaneGA);
                ActionSystem.Instance.AddReaction(new CompressBoardGA());

                //RedistributeEnemiesGA redistributeEnemiesGA = new();
                //ActionSystem.Instance.AddReaction(redistributeEnemiesGA);
            }
            else if (emptyIndex == 0)
            {
                ActionSystem.Instance.AddReaction(new RemoveLaneGA(_laneViews.First()));
                ActionSystem.Instance.AddReaction(new CompressBoardGA());
            }
            else if (emptyIndex == _laneViews.Count - 1)
            {
                ActionSystem.Instance.AddReaction(new RemoveLaneGA(_laneViews.Last()));
                ActionSystem.Instance.AddReaction(new CompressBoardGA());
            }
        }

        yield return null;
    }

    public IEnumerator RemoveLane(LaneView laneView, float duration)
    {
        if (_laneViews.Count == 1)
            yield break;

        int removeIndex = _laneViews.IndexOf(laneView);

        HeroView heroView = laneView.HeroView;

        if (heroView != null)
        {
            int middleIndex = _laneViews.Count / 2;

            MoveHeroGA moveHeroGA;

            RemoveLaneGA removeLaneGA = new(laneView);

            if (removeIndex > middleIndex)
            {
                moveHeroGA = new(_laneViews[removeIndex + 1],heroView);
            }
            else
            {
                moveHeroGA = new(_laneViews[removeIndex - 1], heroView);
            }

            if(heroView!=null)
                ActionSystem.Instance.AddReaction(moveHeroGA);
            ActionSystem.Instance.AddReaction(removeLaneGA);
            yield break;
        }

        laneView.gameObject.SetActive(false);

        float lerpAmount = (_laneViews.Count - 2.0f) / (_originalLaneCount - 1.0f);

        _wrapper.DOScale(Vector3.Lerp(_singleLaneTransform.localScale,_originalScale,(lerpAmount)),duration);
        _wrapper.DOLocalMove(Vector3.Lerp(_singleLaneTransform.localPosition,_originalPosition,(lerpAmount)),duration);

        Tween tween = null;

        for (int i = _laneViews.Count - 1; i > removeIndex; i--)
        {
            LaneView moveLaneView = _laneViews[i];
            LaneView positionLaneView = _laneViews[i-1];
            tween = moveLaneView.transform.DOLocalMove(positionLaneView.transform.localPosition, duration);
        }

        if (tween != null)
            yield return tween.WaitForCompletion();

        Destroy(laneView.gameObject);
        _laneViews.Remove(laneView);

        yield return null;
    }
}
