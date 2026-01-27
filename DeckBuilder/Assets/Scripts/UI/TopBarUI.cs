using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : Singleton<TopBarUI>
{
    [SerializeField] private TMPro.TMP_Text _renownText;

    [SerializeField] private Image _heroSymbol1;
    [SerializeField] private TMPro.TMP_Text _healthText1;
    [SerializeField] private Image _heroSymbol2;
    [SerializeField] private TMPro.TMP_Text _healthText2;
    [SerializeField] private CardPileUI _deckUI;

    public Vector3 DeckUIPos { get => (_deckUI.transform.position); }

    private void Start()
    {
        _deckUI.SetUp(RunSystem.Instance.RunData.Deck);
        _renownText.text = RenownSystem.Instance.Renown.ToString();

        _heroSymbol1.sprite = RunSystem.Instance.Hero1.Image;
        _heroSymbol2.sprite = RunSystem.Instance.Hero2.Image;

        UpdateHealth();
    }

    public void UpdateRenown(int credits)
    {
        int prevAmount = int.Parse(_renownText.text);
        StartCoroutine(LerpCredits(credits-prevAmount, 0.5f));
    }

    public void UpdateHealth(int health = 0)
    {
        _healthText1.text = RunSystem.Instance.Hero1.CurrentHealth + "/" + RunSystem.Instance.Hero1.MaxHealth;
        _healthText2.text = RunSystem.Instance.Hero2.CurrentHealth + "/" + RunSystem.Instance.Hero2.MaxHealth;
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
