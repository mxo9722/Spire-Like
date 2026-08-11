using SerializeReferenceEditor;
using System;
using UnityEngine;

[Serializable]
public class AnyCardCondition : Condition
{
    [SerializeReference, SR] private CardTargetMode _targetMode;

    protected override bool IsConditionMet(EffectContext context)
    {
        return _targetMode.GetTargetsTrivial(context).Count > 0 || _targetMode.AllPossibleTargets(context).Count > 0;
    }
}
