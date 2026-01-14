using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TopBarUI : Singleton<TopBarUI>
{
    [SerializeField] private TMPro.TMP_Text _renownText;
    [SerializeField] private TMPro.TMP_Text _healthText;
    [SerializeField] private CardPileUI _deckUI;

    public Vector3 DeckUIPos { get => (_deckUI.transform.position); }

    private void Start()
    {
        _deckUI.SetUp(RunSystem.Instance.RunData.Deck);
        _renownText.text = RenownSystem.Instance.Renown.ToString();
        
        UpdateHealth();
    }

    public void UpdateRenown(int credits)
    {
        int prevAmount = int.Parse(_renownText.text);
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
        if (amount == 0)
            yield break;

        int uAmount = Mathf.Abs(amount);

        int increment = amount / uAmount;

        for (int i = 0; i < uAmount; i++)
        {
            if (_renownText == null)
                yield break;

            int prevAmount = int.Parse(_renownText.text);
            _renownText.text = (prevAmount + increment).ToString();
            yield return new WaitForSeconds(duration / (float)uAmount);
        }
    }
}
