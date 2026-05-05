using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        T t = list[0];
        list.Remove(t);
        return t;
    }
    
    public static T DrawRandom<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        int r = RNG.Random.Next(list.Count);
        T t = list[r];
        list.Remove(t);
        return t;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        System.Random random = RNG.Random;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static IEnumerable<CombatantView> ApplyFilters(this List<CombatantView> list,List<CombatantFilter> filters, EffectContext context = null)
    {
        if (context == null)
            context = new();
        return list.Where(i => filters.TrueForAll(f => f.TestTarget(context, i)));
    }
    
    public static IEnumerable<LaneView> ApplyFilters(this List<LaneView> list,List<LaneFilter> filters, EffectContext context = null)
    {
        if (context == null)
            context = new();
        return list.Where(i => filters.TrueForAll(f => f.TestTarget(context, i)));
    }
    
    public static IEnumerable<Card> ApplyFilters(this List<Card> list,List<CardFilter> filters, EffectContext context = null)
    {
        if (context == null)
            context = new();
        return list.Where(i => filters.TrueForAll(f => f.TestTarget(context, i)));
    }

    public static bool TargetIsValid<F,T>(this IEnumerable<F> filters, T target, EffectContext context = null) where F : TargetFilter<T>
    {
        return filters.All(f => f.TestTarget(context, target));
    }
}