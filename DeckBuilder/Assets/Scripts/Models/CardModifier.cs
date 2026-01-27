using UnityEngine;

[System.Serializable]
public abstract class CardModifier
{

    [SerializeField] public virtual bool IsTemporary { get => false; }

    public void SetUp(CardView cardView)
    {

    }

    protected virtual void ApplyVisualEffects(CardView card)
    {

    }

}
