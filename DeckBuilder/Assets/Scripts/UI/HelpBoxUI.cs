using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HelpBoxUI : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private ContentSizeFitter[] _contentSizeFitter;
    [SerializeField] private CanvasGroup _canvasGroup;


    [SerializeField, Min(0)] private float _appearTime;
    [SerializeField, Min(0)] private float _fadeInTime;

    private Coroutine _makeVisible = null;
    private Tween _fadeTween = null;

    private void OnEnable()
    {
        _canvasGroup.alpha = 0;

        foreach(ContentSizeFitter csf in _contentSizeFitter)
            csf.enabled = false;
        foreach(ContentSizeFitter csf in _contentSizeFitter)
            csf.enabled = true;

        Canvas.ForceUpdateCanvases();

        float time = Time.time;

        _makeVisible = StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        if(_makeVisible!= null)
        {
            StopCoroutine(_makeVisible);
            _makeVisible = null;
        }

        if(_fadeTween != null)
        {
            _fadeTween.Kill();
            _fadeTween = null;
        }
    }

    public void SetUpFromText(string title,string description)
    {
        _title.text = title;
        _description.text = description;

        gameObject.SetActive(true);
    }

    public void SetUpFromKeyWord(string keyWord)
    {
        _title.text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(keyWord.ToLower());
        _description.text = CardTipSystem.Instance.CardTipData.Map[keyWord];

        gameObject.SetActive(true);

    }

    public void SetUpFromStatusEffect(StatusEffectType statusEffectType, int stacks)
    {
        StatusEffectInfo statusEffectInfo = StatusEffectSystem.Instance.GetStatusEffectInfo(statusEffectType);

        _title.text = statusEffectInfo.Name;
        _description.text = statusEffectInfo.Description.Replace(" X"," "+stacks.ToString());

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        if(_appearTime > 0)
            yield return new WaitForSeconds(_appearTime);

        _fadeTween = _canvasGroup.DOFade(1, _fadeInTime);

        yield return _fadeTween.WaitForCompletion();

        _makeVisible = null;
        _fadeTween = null;
    }
}
