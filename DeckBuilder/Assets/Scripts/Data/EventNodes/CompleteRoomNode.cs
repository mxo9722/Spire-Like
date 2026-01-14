using UnityEngine;
using XNode;

public class CompleteRoomNode : Node, IHasNodeContent
{

	public static string OptionText = "Continue";

	[SerializeField, Input] private int _prev;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return this; // Replace this
	}

    public Node[] GetNodeContent()
    {
		return new Node[] { this };
    }
}