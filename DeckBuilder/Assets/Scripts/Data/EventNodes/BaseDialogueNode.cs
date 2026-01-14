using AYellowpaper.SerializedCollections;
using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[NodeWidth(450)]
public abstract class BaseDialogueNode : Node, IHasNodeContent
{
	[field: SerializeField, TextArea] public string Text { get; private set; }
	[field: SerializeField] public Sprite Sprite { get; private set; }

	public static string OutputKey = "Choices";
	[Output] public int Choices; 

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return this; // Replace this
	}

	public virtual Node[] GetNodeContent()
    {
		return new Node[] { this };
    }

	public abstract void SetUp();
}