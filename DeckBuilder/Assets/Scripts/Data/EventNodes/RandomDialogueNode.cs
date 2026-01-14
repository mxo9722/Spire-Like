using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using XNode;

public class RandomDialogueNode : BaseDialogueNode
{
    [SerializeField, Min(1)] private int _optionCount = 1;

    private int[] _selectedOptions;

    [Input] public int Prev;


    public override void SetUp()
    {
        
    }

    public void RandomlySelectOptions()
    {
        List<NodePort> outputs = GetOutputPort(OutputKey).GetConnections();

        int outputCount = outputs.Count;

        if (outputCount > _optionCount)
        {
            _selectedOptions = Enumerable.Repeat(-1, _optionCount).ToArray();

            for (int i = 0; i < _optionCount; i++)
            {
                int index = -1;

                while (index == -1 || _selectedOptions.Contains(index))
                    index = RNG.Random.Next(0,outputCount);

                _selectedOptions[i] = index;
            }
        }
        else
        {
            _selectedOptions = new int[outputCount];

            for(int i=0;i< outputCount; i++)
            {
                _selectedOptions[i] = i;
            }
        }
    }

    public override Node[] GetNodeContent()
    {
        RandomlySelectOptions();

        List<NodePort> outputs = GetOutputPort(OutputKey).GetConnections();

        Node[] nodes = new Node[0];

        foreach (int index in _selectedOptions)
        {
            IHasNodeContent iHasNode = (IHasNodeContent)outputs[index].node;
            nodes = nodes.Concat(iHasNode.GetNodeContent()).ToArray();
        }

        return nodes;
    }
}
