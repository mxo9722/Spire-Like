using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformTrialEA : EventAction
{

    [SerializeReference, SR] private List<CardFilter> _winFilters;
    [SerializeField] private int _successesNeeded = 1;
    [SerializeField] private string _trialText = "";

    [SerializeReference, SR] private List<EventAction> _successActions;
    [SerializeReference, SR] private List<EventAction> _failureActions;


    public override IEnumerator Invoke(EffectContext context)
    {
        TrialView.Instance.StartTrial(_winFilters, _successesNeeded, _trialText);

        yield return new WaitWhile(TrialView.Instance.IsTrialOnGoing);

        List<EventAction> actions = _successActions;

        if (!TrialView.Instance.TrialSucceeded())
            actions = _failureActions;

        foreach (EventAction action in actions)
        {
            yield return action.Invoke(context);
        }
    }
}
