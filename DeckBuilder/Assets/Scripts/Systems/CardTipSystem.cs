using UnityEngine;

public class CardTipSystem : PersistentSingleton<CardTipSystem>
{
    [field: SerializeField] public CardTipData CardTipData { get; private set; }
    //Original color was hex "DAA520"
    [field: SerializeField] public Color KeyWordColor { get; private set; }

}
