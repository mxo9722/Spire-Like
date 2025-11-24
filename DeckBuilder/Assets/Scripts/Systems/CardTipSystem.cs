using UnityEngine;

public class CardTipSystem : Singleton<CardTipSystem>
{
    [field: SerializeField] public CardTipData CardTipData { get; private set; }
    [field: SerializeField] public Color KeyWordColor { get; private set; }

}
