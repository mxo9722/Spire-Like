using System.Collections;
using UnityEngine;

public class GoToConnectedNodeEA : EventAction
{
    [SerializeField] private string _optionName = "";

    public override IEnumerator Invoke(EffectContext context)
    {
        XNode.Node baseNode = EventSystem.Instance.DefaultRoom.CurrentNode;

        XNode.NodePort outputs = baseNode.GetOutputPort(BaseDialogueNode.OutputKey);

        for(int i = 0; i < outputs.ConnectionCount; i++)
        {
            XNode.Node output = outputs.GetConnection(i).node;

            if(output.name == _optionName)
            {
                EventSystem.Instance.EnterNode(output);

                yield break;
            }
        }

        yield return null;
    }
}
