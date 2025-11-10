using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [field:SerializeField] public Sprite Image { get; private set; }
    [field:SerializeField] public int Health { get; private set; }
    [field: SerializeField] public List<EnemyAction> ActionPattern { get; private set; } = new();
}
