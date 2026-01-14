using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleGA : GameAction
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public List<CombatantView> Speakers { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    
    public SpeechBubbleGA(string text, List<CombatantView> speaker, float duration)
    {
        Text = text;
        Speakers = speaker;
        Duration = duration;
    }

}
