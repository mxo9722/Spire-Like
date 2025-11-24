using UnityEngine;

[System.Serializable]
public class EnemyIntent
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Text { get; private set; }
}
