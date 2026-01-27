using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpeechBubbleEffect : CombatantTargetEffect
{
    [SerializeField] [TextArea(2, 4)] private List<string> _dialogueOptions;
    [SerializeField] private float _duration = 3;
    [SerializeField] private float _waitDuration = 1;
    
    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        int index = RNG.TrivialRandom.Next(0, _dialogueOptions.Count);

        string speech = _dialogueOptions[index];

        SpeechBubbleGA speechBubbleGA = new(speech, combatantTargets, _duration, _waitDuration);

        return speechBubbleGA;
    }
}
