using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class EventGraph : NodeGraph 
{

	[field: SerializeField, HideInInspector] public StartEventNode StartNode { get; private set; }

    public void SetUp()
    {
    }

    public void OnValidate()
    {
		foreach(Node node in nodes)
        {
            if(node is StartEventNode startNode)
            {
                StartNode = startNode;
                return;
            }
        }
    }
}