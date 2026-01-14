using UnityEngine;

[System.Serializable]
public abstract class TargetFilter<T>
{
    [SerializeField] private bool _invert = false;

    public bool TestTarget(EffectContext context,T target)
    {
        if (target == null)
            return false;

        if (_invert)
            return !TargetIsValid(context, target);
        return TargetIsValid(context, target);
    }
    protected abstract bool TargetIsValid(EffectContext context,T target);
}

[System.Serializable] public abstract class CombatantFilter : TargetFilter<CombatantView> {}
[System.Serializable]public abstract class LaneFilter : TargetFilter<LaneView> {}
[System.Serializable]public abstract class CardFilter : TargetFilter<Card> {}