using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpeechBubbleEffect : CombatantTargetEffect
{
    [SerializeField] [TextArea(2, 4)] private List<string> _dialogueOptions;
    [SerializeField] private float _duration;


    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        int index = RNG.TrivialRandom.Next(0, _dialogueOptions.Count);

        string speech = _dialogueOptions[index];

        SpeechBubbleGA speechBubbleGA = new(speech, combatantTargets, _duration);

        return speechBubbleGA;
    }
}
