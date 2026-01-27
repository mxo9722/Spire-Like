using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeechBubbleEffect : CombatantTargetEffect
{
    [SerializeField] [TextArea(2, 4)] private List<string> _dialogue;
    [SerializeField] private float _durationPerBubble = 3;
    [SerializeField] private float _finalBubbleWaitDuration = 1;

    public SpeechBubbleEffect()
    {

    }

    public SpeechBubbleEffect(string dialogue, float duration)
    {
        _dialogue = new() { dialogue };
        _durationPerBubble = duration;
    }

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        
        if (_dialogue.Count > 1)
        {
            List<GameAction> GAs = new();

            for (int i = 0; i < _dialogue.Count; i++)
            {
                string dialogue = _dialogue[i];

                float waitDuration = i == _dialogue.Count - 1 ? _finalBubbleWaitDuration : _durationPerBubble;

                GAs.Add(new SpeechBubbleGA(dialogue, combatantTargets, _durationPerBubble, waitDuration));
            }

            MultipleGameActionsGA multipleEffectsGA = new(GAs);

            return multipleEffectsGA;
        }


        SpeechBubbleGA speechBubbleGA = new(_dialogue[0], combatantTargets, _durationPerBubble,_finalBubbleWaitDuration);
        return speechBubbleGA;
    }
}
