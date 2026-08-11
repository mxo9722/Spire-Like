using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class SameNameCardFilter : CardFilter
{
    [SerializeReference, SR] private CardTargetMode _nameBasis;
    [SerializeField] private bool _ignoreUpgrades = true;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        List<Card> targets = _nameBasis.GetTargets(context);
        string name = target.Title;

        if (_ignoreUpgrades)
            name = name.Replace("+", "");

        foreach(Card card in targets)
        {
            string oName = card.Title;

            if (_ignoreUpgrades)
                oName = oName.Replace("+","");

            if (oName == name)
                return true;
        }

        return false;
    }
}
