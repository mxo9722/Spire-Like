using System.Collections.Generic;
using UnityEngine;

public class EventView : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private EventOptionView[] _eventOptions;

    public void SetUp(BaseDialogueNode node, List<EventOption> options)
    {
        _title.text = node.graph.name;
        _text.text = node.Text;

        int index = _eventOptions.Length - options.Count;

        for(int i = 0; i < _eventOptions.Length; i++)
        {
            EventOption option = null;

            if (i >= index)
                option = options[i - index];

            EventOptionView optionView = _eventOptions[i];
            optionView.SetUp(option);
        }
    }

    public void DisableAllOptionViews()
    {
        foreach (EventOptionView option in _eventOptions)
            option.SetUnavailable();
    }
}
