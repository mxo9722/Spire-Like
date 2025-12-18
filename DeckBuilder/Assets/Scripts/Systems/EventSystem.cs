using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EventSystem : Singleton<EventSystem>
{
    [field: SerializeField] public EventView EventView { get; private set; }
    [field: SerializeField] public EventRoom DefaultRoom { get; private set; }
    [field: SerializeField] public EventGraph SafeHouseGraph { get; private set; }
    [SerializeField] private ScenePicker _mapScene;



    private void Start()
    {
        Room room = RunSystem.Instance.GetRoom();

        switch (room)
        {
            case EventRoom eventRoom:
                DefaultRoom = eventRoom;
                EnterNode(DefaultRoom.EventGraph.StartNode);
                break;
            case SafeHouseRoom:
                EnterNode(SafeHouseGraph.StartNode);
                break;
            default:
                EnterNode(DefaultRoom.EventGraph.StartNode);
                break;
        }

    }

    public void EnterNode(Node node)
    {
        DefaultRoom.SetNode(node);
        RunSystem.Instance.SaveRun();

        switch (node)
        {
            case StartEventNode startNode:
                {
                    List<EventOption> options = GetOptions(startNode);
                    EventView.SetUp(startNode, options);
                    break;
                }
            case DialogueNode dialogueNode:
                {
                    EventView.DisableAllOptionViews();

                    StartCoroutine(PerformActions(dialogueNode));
                    break;
                }
            case CompleteRoomNode:
                RunSystem.Instance.GetRoom()?.SetCompleted();
                _mapScene.LoadScene();
                break;
        }
    }

    public IEnumerator PerformActions(DialogueNode node)
    {
        foreach(EventAction action in node.Actions)
        {
            yield return action.Invoke();
        }

        List<EventOption> options = GetOptions(node);
        EventView.SetUp(node, options);
    }

    public static List<EventOption> GetOptions(BaseDialogueNode node)
    {
        List<EventOption> options = new();

        NodePort port = node.GetOutputPort(BaseDialogueNode.OutputKey);

        foreach (NodePort p in port.GetConnections())
        {
            if (p == null)
                continue;

            EventOption option = new();
            bool success = option.SetUp(p.node);
            if (success)
                options.Add(option);
        }

        return options;
    }
}
