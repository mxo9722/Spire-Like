using AYellowpaper.SerializedCollections;
using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseDialogueNode 
{
	[field: SerializeField] public string OptionText;
	[field: SerializeField] public bool HideIfUnavailable = false;
	[field: SerializeReference, SR] public List<EventCondition> Conditions { get; private set; }
	[Input] public int Prev;

	[field: SerializeReference, SR] public List<EventAction> Actions { get; private set; }

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	public override void SetUp()
	{
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if(!HideIfUnavailable || Conditions.TrueForAll(c => c.IsMet()))
			return this;
		return null;
	}

	public IEnumerator PerformActions(SerializedDictionary<int, object> dict)
    {
		EffectContext context = new();
		foreach(EventAction action in Actions)
        {
			yield return action.Invoke(context);
        }
    }

	public bool IsAvailable()
    {
		return Conditions.Count == 0 || Conditions.TrueForAll(c => c.IsMet());
	}

    private void OnValidate()
    {
		name = OptionText;
    }
}