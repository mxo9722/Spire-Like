using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TopBarUI : Singleton<TopBarUI>
{
    [SerializeField] private TMPro.TMP_Text _karmaText;
    [SerializeField] private TMPro.TMP_Text _healthText;
    [SerializeField] private CardPileUI _deckUI;

    public Vector3 DeckUIPos { get => (_deckUI.transform.position); }

    private void Start()
    {
        _deckUI.SetUp(RunSystem.Instance.RunData.Deck);
        _karmaText.text = KarmaSystem.Instance.Karma.ToString();
        
        UpdateHealth();
    }

    public void UpdateCredits(int credits)
    {
        int prevAmount = int.Parse(_karmaText.text);
        StartCoroutine(LerpCredits(credits-prevAmount, 0.5f));
    }

    public void UpdateHealth()
    {
        _healthText.text = RunSystem.Instance.CurrentHealth + "/" + RunSystem.Instance.MaxHealth;
    }

    public void UpdateHealth(HeroView hero)
    {
        _healthText.text = hero.CurrentHealth + "/" + hero.MaxHealth;
    }

    private IEnumerator LerpCredits(int amount, float duration)
    {
        int uAmount = Mathf.Abs(amount);

        int increment = amount / uAmount;

        for (int i = 0; i < uAmount; i++)
        {
            if (_karmaText == null)
                yield break;

            int prevAmount = int.Parse(_karmaText.text);
            _karmaText.text = (prevAmount + increment).ToString();
            yield return new WaitForSeconds(duration / (float)uAmount);
        }
    }
}
