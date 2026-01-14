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

    public readonly int MAX_HERO_COUNT = 2;
    public readonly int MAX_ENEMY_COUNT = 3;

    public void OnEnable()
    {
        _originalPosition = _wrapper.localPosition;
        _originalScale = _wrapper.localScale;
        _originalLaneCount = _laneViews.Count;

        for (int i=0;i<_laneViews.Count;i++)
        {
            LaneView laneView = _laneViews[i];
            _lanePositions.Add(laneView.transform.localPosition);
            laneView.SetUp(this, i);
        }
    }

    public HeroView CreateHero(HeroData heroData, int laneIndex = 0)
    {
        LaneView laneView = _laneViews[laneIndex];
        SlotView slot = laneView.FirstAvailableHeroSlot();
        HeroView heroView = CombatantViewCreator.Instance.CreateHeroView(heroData, slot);

        laneView.SetHero(heroView);

        slot.AddCombatant(heroView);
        heroView.transform.localScale = Vector3.one;

        if(RunSystem.Instance.CurrentHealth > 0)
            heroView.SetHealth(RunSystem.Instance.CurrentHealth);

        return heroView;
    }

    public NPCView CreateEnemy(NPCData enemyData, int laneIndex)
    {
        LaneView laneView = _laneViews[laneIndex];

        SlotView slot = laneView.FirstAvailableEnemySlot();

        NPCView enemyView = CombatantViewCreator.Instance.CreateEnemyView(enemyData, slot);
        enemyView.transform.localScale = Vector3.one;

        EnemySystem.DetermineEnemyBehaviour(enemyView);

        return enemyView;
    }

    public List<NPCView> GetAllEnemies()
    {
        List<NPCView> allEnemies = new();

        foreach (LaneView laneView in _laneViews)
            allEnemies.AddRange(laneView.EnemyViews);

        return allEnemies;
    }

    public List<CombatantView> GetAllHeroes()
    {
        List<CombatantView> allHeroes = new();

        foreach (LaneView laneView in _laneViews)
            allHeroes.AddRange(laneView.HeroViews);

        return allHeroes;
    }
    
    public List<NPCView> GetAllSideKicks()
    {
        List<NPCView> allSideKicks = new();

        foreach (LaneView laneView in _laneViews)
            allSideKicks.AddRange(laneView.HeroViews.Where(h => h is NPCView).Cast<NPCView>());

        return allSideKicks;
    }

    public List<CombatantView> GetAllCombatants()
    {
        List<CombatantView> allCombatants = new();

        foreach (LaneView laneView in _laneViews)
        {
            allCombatants.AddRange(laneView.EnemyViews);
            allCombatants.AddRange(laneView.HeroViews);
        }

        return allCombatants;
    }

    public List<CombatantView> GetAllFoes(CombatantView caster)
    {
        if (caster is HeroView)
            return GetAllEnemies().Cast<CombatantView>().ToList();
        else if(caster is NPCView npcView)
        {
            if(!npcView.IsEvil)
                return GetAllEnemies().Cast<CombatantView>().ToList();
        }

        return GetAllHeroes().Cast<CombatantView>().ToList();
    }

    public List<LaneView> GetAllLanes() => _laneViews;

    public LaneView GetCurrentLaneView(CombatantView combatantView)
    {
        return _laneViews.Find(e => e.HeroViews.Contains(combatantView) || e.EnemyViews.Contains(combatantView));
    }

    public LaneView GetCurrentLaneView(NPCView npcView)
    {
        return _laneViews.Find(e => e.EnemyViews.Contains(npcView) || e.HeroViews.Contains(npcView));
    }

    public LaneView GetCurrentLaneView(HeroView heroView)
    {
        return _laneViews.Find(e => e.HeroViews.Contains(heroView));
    }

    public IEnumerator RemoveEnemy(NPCView enemyView)
    {
        LaneView laneView = GetCurrentLaneView(enemyView);

        yield return laneView.RemoveEnemy(enemyView);
    }

    public IEnumerator RedistributeEnemies()
    {
        List<NPCView> allEnemyViews = GetAllEnemies();

        int enemyCount = allEnemyViews.Count;
        int minCount = enemyCount / _laneViews.Count;

        if (minCount == 0)
        {
            CompressBoardGA compressBoardGA = new();

            ActionSystem.Instance.AddReaction(compressBoardGA);

            yield break;
        }

        int shortageIndex = _laneViews.FindIndex(e => e.EnemyViews.Length < minCount);

        if (shortageIndex != -1)
        {
            LaneView shortageLane = _laneViews[shortageIndex];

            LaneView excess = null;
            int bestDist = allEnemyViews.Count;
            int highestCount = 0;

            for (int excessIndex = 0; excessIndex < _laneViews.Count; excessIndex++)
            {
                LaneView laneView = _laneViews[excessIndex];

                int count = laneView.EnemyViews.Length;
                int dist = Mathf.Abs(excessIndex - shortageIndex);

                if (count > 0 && excessIndex != shortageIndex && (dist < bestDist || (count > highestCount && dist == bestDist)))
                {
                    bestDist = dist;
                    excess = laneView;
                    highestCount = count;
                }
            }

            MoveUnitsGA moveEnemyGA = new MoveUnitsGA(shortageLane, excess.EnemyViews.Last(), null);
            ActionSystem.Instance.AddReaction(moveEnemyGA);

            ActionSystem.Instance.AddReaction(new RedistributeEnemiesGA());
        }
    }

    public IEnumerator MoveCombatants(MoveUnitsGA moveEnemyGA, float duration)
    {
        if (moveEnemyGA == null)
            yield break;

        Dictionary<CombatantView, LaneView> moves = new(moveEnemyGA.Moves);

        foreach (KeyValuePair<CombatantView, LaneView> move in moves)
        {
            bool moved = MoveCombatant(move.Key, move.Value, moveEnemyGA.Caster);

            if (moved)
                move.Key.SetMoved(true);
            else
                moveEnemyGA.RemoveMove(move.Key);

        }

        Tween tween = null;

        foreach(CombatantView target in GetAllCombatants())
        {
            yield return target.WaitForTweensComplete();

            if (moveEnemyGA.Moves.Keys.Contains(target))
            {
                if (moveEnemyGA.JumpValue > 0)
                    tween = target.transform.DOLocalJump(Vector3.zero, moveEnemyGA.JumpValue, 1, moveEnemyGA.AnimationDuration);
                else
                    tween = target.transform.DOLocalMove(Vector3.zero, moveEnemyGA.AnimationDuration);
            }
            else if(target.transform.localPosition != Vector3.zero)
            {
                target.transform.DOLocalMove(Vector3.zero, duration);
            }
        }

        if (tween != null)
            yield return tween.WaitForCompletion();

        DynamicViewsSystem.Instance.UpdateDynamicValues();
    }

    private bool MoveCombatant(CombatantView target, LaneView lane, CombatantView caster)
    {
        bool unmoved = false;

        if (target.GetStatusEffectStacks(StatusEffectType.ANCHORED) > 0 && caster != target && caster != null)
            unmoved = true;
        else if (target.GetStatusEffectStacks(StatusEffectType.HAMSTRUNG) > 0 && caster == target)
            unmoved = true;

        if (unmoved)
        {
            target.transform.DOPunchPosition(Random.insideUnitCircle, 1);
            return false;
        }

        bool moved;
        if (target is NPCView enemy && enemy.IsEvil)
            moved = MoveEnemy(enemy, lane);
        else
            moved = MoveHero(target, lane);

        return moved;
    }

    private bool MoveEnemy(CombatantView enemy, LaneView destination)
    {
        if (enemy.CurrentHealth == 0)
            return false;

        if (destination.EnemyViews.Length >= MAX_ENEMY_COUNT)
            return false;

        LaneView originalLaneView = GetCurrentLaneView(enemy);

        SlotView slot = destination.FirstAvailableEnemySlot();
        slot.AddCombatant(enemy, false);

        originalLaneView.SlideEnemiesLeft();
        destination.SlideEnemiesLeft();

        return true;
    }

    private bool MoveHero(CombatantView hero, LaneView destination)
    {
        if (hero.CurrentHealth == 0)
            return false;

        if (destination.HeroViews.Length >= MAX_HERO_COUNT)
            return false;

        LaneView originalLaneView = GetCurrentLaneView(hero);

        SlotView slot = destination.FirstAvailableHeroSlot();
        slot.AddCombatant(hero, false);

        originalLaneView.SlideHeroesRight();
        destination.SlideHeroesRight();

        return true;
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

        int heroIndex = _laneViews.FindIndex(e => e.HeroViews.Any(h => h is HeroView));

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
                            foreach (NPCView enemyView in pushLaneView.EnemyViews)
                            {
                                MoveUnitsGA moveEnemyGA = new(pullLaneView, enemyView, null);
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
                            foreach (NPCView enemyView in pushLaneView.EnemyViews)
                            {
                                MoveUnitsGA moveEnemyGA = new(pullLaneView, enemyView, null);
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

        CombatantView[] heroViews = laneView.HeroViews;

        if (heroViews.Length > 0)
        {
            foreach (CombatantView heroView in heroViews)
            {
                int middleIndex = _laneViews.Count / 2;

                MoveUnitsGA moveHeroGA;

                RemoveLaneGA removeLaneGA = new(laneView);

                if (removeIndex >= middleIndex)
                {
                    moveHeroGA = new(_laneViews[removeIndex - 1], heroView, null);
                }
                else
                {
                    moveHeroGA = new(_laneViews[removeIndex + 1], heroView, null);
                }

                if (heroView != null)
                    ActionSystem.Instance.AddReaction(moveHeroGA);
                ActionSystem.Instance.AddReaction(removeLaneGA);
            }
            
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

    public HeroView GetMainHero()
    {
        foreach(LaneView lane in _laneViews)
        {
            CombatantView heroView = lane.HeroViews.First(h => h is HeroView);
            if (heroView != null)
                return (HeroView)heroView;
        }

        return null;
    }
}
