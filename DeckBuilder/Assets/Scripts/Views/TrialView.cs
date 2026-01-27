using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class TrialView : Singleton<TrialView>
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private TMPro.TMP_Text _trialTextView;
    [SerializeField] private SpriteRenderer _background;

    private List<CardFilter> _winConditions;
    private List<CardView> _cardViews = new();

    private int _cardsToFlip = -1;
    private int _successCards = -1;
    private int _successesNeeded = 0;

    private bool _trialOnGoing = false;

    public void StartTrial(List<CardFilter> winConditions, int successesNeeded, string trialText)
    {
        _winConditions = winConditions;

        _background.gameObject.SetActive(true);
        _trialTextView.gameObject.SetActive(true);
        _trialTextView.text = trialText;

        _trialOnGoing = true;

        _cardsToFlip = 3;
        _successCards = 0;
        _successesNeeded = successesNeeded;

        StartCoroutine(SpawnCards(_cardsToFlip));
    }

    private IEnumerator SpawnCards(int drawCount)
    {
        List<Card> deck = new(RunSystem.Instance.Deck);
        deck.Shuffle();

        for (int i = 0; i < drawCount; i++)
        {
            float p = (i + 1.00f) / (drawCount + 1.00f);

            Vector3 position = _splineContainer.Spline.EvaluatePosition(p);

            var cardView = CreateCardView(deck[i], position, 0.5f);

            _cardViews.Add(cardView);

            yield return new WaitForSeconds(0.25f);
        }
    }

    private CardView CreateCardView(Card card, Vector3 cardPos, float moveTime)
    {
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, TopBarUI.Instance.DeckUIPos, Quaternion.identity, true);

        cardView.SetSideUp(false);
        cardView.SetGlow(Color.clear);

        cardView.OnButtonPressed += ButtonPressed;

        cardView.transform.parent = transform;

        cardView.transform.DOMove(cardPos, moveTime);

        return cardView;
    }

    private void ButtonPressed(CardView cardView)
    {
        cardView.OnButtonPressed -= ButtonPressed;

        StartCoroutine(FlipCard(cardView));
    }

    private IEnumerator FlipCard(CardView cardView)
    {
        cardView.SetSideUp(true,0.5f);
        yield return new WaitForSeconds(0.5f);

        if (_winConditions.TrueForAll(wc => wc.TestTarget(new(), cardView.Card)))
        {
            cardView.SetGlow(Color.green);
            _successCards++;
        }

        yield return new WaitForSeconds(2);

        _cardsToFlip--;

        if (_cardsToFlip == 0)
        {
            StartCoroutine(HideTrial());
        }
    }

    public bool IsTrialOnGoing()
    {
        return _trialOnGoing;
    }

    public bool TrialSucceeded()
    {
        return _successCards >= _successesNeeded;
    }

    public IEnumerator HideTrial()
    {
        Tween tween = null;

        foreach(CardView cardView in _cardViews)
        {
            tween = cardView.transform.DOScale(Vector3.zero, 0.5f);
            yield return new WaitForSeconds(0.25f);
        }

        if (tween != null)
            yield return tween.WaitForCompletion();

        foreach (CardView cardView in _cardViews)
        {
            Destroy(cardView.gameObject);
        }

        _cardViews.Clear();
        _background.gameObject.SetActive(false);

        _trialTextView.gameObject.SetActive(false);

        _trialOnGoing = false;
    }
}
