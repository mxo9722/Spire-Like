using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetUp(Vector3 position, string text, Vector3 scale)
    {
        transform.position = position;

        Vector3 oScale = transform.localScale;

        transform.localScale = new(oScale.x * scale.x, oScale.y * scale.y, oScale.z * scale.z);
        _text.text = text;
    }

    public IEnumerator PlayWordBubble(float duration)
    {
        _canvasGroup.alpha = 0;
        
        var tween = _canvasGroup.DOFade(1, 0.15f);
        yield return tween.WaitForCompletion();

        yield return new WaitForSeconds(duration);

        tween = _canvasGroup.DOFade(0f, 0.15f);
        yield return tween.WaitForCompletion();

        Destroy(gameObject);
    }
}
