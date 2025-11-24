using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatRoom : Room
{
    [field: SerializeField] public List<EnemyData> TopRow { get; private set; }
    [field: SerializeField] public List<EnemyData> MiddleRow { get; private set; }
    [field: SerializeField] public List<EnemyData> BottomRow { get; private set; }

    [field: SerializeReference, SR] public List<Reward> Rewards { get; private set; }

    public CombatRoom()
    {
        TopRow = new();
        MiddleRow = new();
        BottomRow = new();
    }

    public CombatRoom(List<EnemyData> topRow, List<EnemyData> middleRow = null, List<EnemyData> bottomRow = null)
    {
        TopRow = topRow;
        MiddleRow = middleRow;
        BottomRow = bottomRow;
    }
}
