using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Data/DeckData")]
public class DeckData : ScriptableObject
{
    [field: SerializeField] public List<CardData> Deck { get; private set; } 
}
