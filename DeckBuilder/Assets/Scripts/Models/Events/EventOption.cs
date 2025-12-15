using UnityEngine;
using XNode;

public class EventOption
{
    public Node NextNode { get; private set; }
    public string Text { get; private set; }
    public bool Available { get; private set; }

    public bool SetUp(Node node)
    {
        switch (node)
        {
            case DialogueNode dNode:
                NextNode = dNode;
                Text = dNode.OptionText;
                Available = dNode.IsAvailable();
                break;
            case CompleteRoomNode:
                NextNode = node;
                Text = CompleteRoomNode.OptionText;
                Available = true;
                break;
            default:
                Debug.LogError("Node not accounted for!", node);
                return false;
        }

        return true;
    }
}
