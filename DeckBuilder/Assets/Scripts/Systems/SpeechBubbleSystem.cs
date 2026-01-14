using System.Collections;
using System.Linq;
using UnityEngine;

public class SpeechBubbleSystem : Singleton<SpeechBubbleSystem>
{
    [SerializeField] private SpeechBubbleUI _speechBubblePrefab;
    [SerializeField] private RectTransform _bubbleParent;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpeechBubbleGA>(SpeechBubblePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpeechBubbleGA>();
    }

    private IEnumerator SpeechBubblePerformer(SpeechBubbleGA speechBubbleGA)
    {
        yield return DisplaySpeechBubble(speechBubbleGA.Speakers.First(), speechBubbleGA.Text, speechBubbleGA.Duration);
    }

    public IEnumerator DisplaySpeechBubble(CombatantView combatantView, string text, float displayTime)
    {
        SpeechBubbleUI speechBubble = DisplaySpeechBubble(combatantView.transform.position + Vector3.up, text, Vector3.one);

        if (displayTime > 0)
            yield return speechBubble.PlayWordBubble(displayTime);
    }

    public SpeechBubbleUI DisplaySpeechBubble(Vector3 pos, string text, Vector3 scale)
    {
        SpeechBubbleUI speechBubble = Instantiate(_speechBubblePrefab, _bubbleParent);

        speechBubble.SetUp(pos, text, scale);

        return speechBubble;
    }

}
