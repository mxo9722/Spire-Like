using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Data/CombatData")]
public class CombatData : ScriptableObject
{
    [field: SerializeField] public List<FightLayout> Fights { get; private set; }
    [field: SerializeField] public List<FightLayout> BossFights { get; private set; }
    [field: SerializeField] public List<FightLayout> VillianFights { get; private set; }
}

[System.Serializable]
public class FightLayout
{
    [field: SerializeField] public EnemyData[] TopLane { get; private set; }
    [field: SerializeField] public EnemyData[] MiddleLane { get; private set; }
    [field: SerializeField] public EnemyData[] BottomLane { get; private set; }
    [field: SerializeField] public bool LaneOrderMatters { get; private set; } = false;

    public void SetCombatLayout(CombatRoom room)
    {

        List<EnemyData> topRow = room.TopRow;
        List<EnemyData> middleRow = room.MiddleRow;
        List<EnemyData> bottomRow = room.BottomRow;

        if (!LaneOrderMatters)
        {
            List<EnemyData>[] lanes = new[]{topRow,middleRow,bottomRow};

            RNG.Random.Shuffle(lanes);

            topRow = lanes[0];
            middleRow = lanes[1];
            bottomRow = lanes[2];
        }

        topRow.Clear();
        topRow.AddRange(TopLane);
        middleRow.Clear();
        middleRow.AddRange(MiddleLane);
        bottomRow.Clear();
        bottomRow.AddRange(BottomLane);
    }
}