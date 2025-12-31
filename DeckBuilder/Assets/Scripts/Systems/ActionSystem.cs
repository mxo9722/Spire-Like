using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;
    public bool IsPerforming { get; private set; } = false;
    private static Dictionary<Type, Dictionary<object, Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, Dictionary<object, Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    //private void OnDisable()
    //{
    //    preSubs = new();
    //    postSubs = new();
    //    performers = new();
    //}

    public void Perform(GameAction action, Action OnPerformFinished = null)
    {
        if (IsPerforming || MatchEndSystem.Instance.GameOver)
        {
            Debug.Log("action attempted while performing ");
            return;
        }
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    public void AddReaction(GameAction gameAction)
    {
        if (reactions == null || gameAction == null)
            return;

        if (reactions.Count() > 0)
        {
            GameAction last = reactions.Last();

            bool combined = last.TryCombine(gameAction);

            if (combined)
                return;
        }

        reactions.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        reactions = action.PreformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }

    private void PerformSubscribers(GameAction action, Dictionary<Type, Dictionary<object, Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            Dictionary<object, Action<GameAction>> dict = subs[type];

            foreach (Action<GameAction> reaction in dict.Values)
            {
                reaction(action);
            }
        }
    }

    private IEnumerator PerformPerformer(GameAction action)
    {
        if (MatchEndSystem.Instance.GameOver && !action.PerformAfterGameOver)
            yield break;

        Func<GameAction, IEnumerator> performer = performers[action.GetType()];
        yield return performer(action);
    }

    private IEnumerator PerformReactions()
    {
        foreach (GameAction reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type)) performers[type] = wrappedPerformer;
        else performers.Add(type, wrappedPerformer);
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type)) performers.Remove(type);
    }

    public static void SubscribeReaction<T>(object subscriber, Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, Dictionary<object, Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Debug.Log(typeof(T).ToString());
        //Debug.Log(typeof(action).ToString());
        void wrappedReaction(GameAction action) => reaction((T)action);
        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(subscriber, wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new());
            subs[typeof(T)].Add(subscriber, wrappedReaction);
        }
    }

    public static void UnsubscribeReaction<T>(object subscriber, Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, Dictionary<object, Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (subs.ContainsKey(typeof(T)))
        {
            bool succ = subs[typeof(T)].Remove(subscriber);
            if (succ)
                Debug.Log("successfully removed");
        }
    }
}
