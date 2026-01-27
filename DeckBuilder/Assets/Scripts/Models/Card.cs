using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Card
{
    public string Title => data.name;
    public CardData Upgrade => data.Upgrade;
    public bool Unplayable => data.Unplayable;
    public CardType Type => data.Type;
    public Sprite Image => data.Image;
    public string Description => data.Description;
    public List<Condition> RequiredConditions => data.RequiredConditions;
    public List<Condition> HighlightConditions => data.HighlightConditions;

    public ManualTargetType ManualTargetType => data.ManualTargetType;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    public List<AutoTargetEffect> TurnEndEffect => data.TurnEndEffects;
    public List<CombatantFilter> CombatantFilters => data.CombatantFilters;
    public List<LaneFilter> LaneFilters => data.LaneFilters;
    public bool ExhuastOnUse => data.ExhuastOnUse;
    public Rarity Rarity => data.Rarity;

    public int Mana { get; private set; }
    public HeroData Owner { get; private set; } = null;

    public readonly CardData data;

    private List<string> _allKeyWords = null;

    public Card(Card card)
    {
        data = card.data;
        Mana = card.Mana;
        Owner = card.Owner;
    }

    public Card(CardData cardData, HeroData owner)
    {
        data = cardData;
        Mana = cardData.Mana;
        Owner = owner;
    }

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;

        RunData runData = RunSystem.Instance.RunData;

        Hero possibleOwner1 = runData.Hero1;
        Hero possibleOwner2 = runData.Hero2;

        SetOwner(possibleOwner1, possibleOwner2);
    }

    private void SetOwner(Hero possibleOwner1, Hero possibleOwner2)
    {
        if (possibleOwner1.ClassCards.Contains(data) && possibleOwner2.ClassCards.Contains(data))
            Owner = null;
        else if (possibleOwner1.ClassCards.Contains(data))
            Owner = possibleOwner1.Data;
        else if (possibleOwner2.ClassCards.Contains(data))
            Owner = possibleOwner2.Data;
        else
            Owner = null;
    }

    public string GetStaticDescription()
    {
        string description = Description;

        description = RemoveParentheses(description);

        List<IDynamicEffectText> dynamicTextEffects = new();

        if (ManualTargetEffect != null)
        {
            dynamicTextEffects.AddRange(ManualTargetEffect.GetDynamicTextEffects());
        }

        foreach (AutoTargetEffect effect in OtherEffects)
        {
            dynamicTextEffects.AddRange(effect.GetDynamicTextEffects());
        }
        
        foreach (AutoTargetEffect effect in TurnEndEffect)
        {
            dynamicTextEffects.AddRange(effect.GetDynamicTextEffects());
        }

        for (int i = 0; i < dynamicTextEffects.Count; i++)
        {
            string value = dynamicTextEffects[i].GetStaticText();
            description = description.Replace("{v" + i.ToString() + "}", value);
        }

        description = HighlightKeyWords(description);

        return description;
    }

    public string GetDynamicDescription(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        string description = Description;

        int index = 0;


        if (ManualTargetEffect != null)
        {

            IDynamicEffectText[] dynamicTextEffects = ManualTargetEffect.GetDynamicTextEffects();

            foreach (IDynamicEffectText dte in dynamicTextEffects)
            {

                List<CombatantView> cTargets = null;
                if (context.TargetCombatant != null)
                    cTargets = new() { context.TargetCombatant };

                List<LaneView> lTargets = null;
                if (context.TargetLane != null)
                    lTargets = new() { context.TargetLane };

                string value = dte.GetDynamicText(context, cTargets, lTargets);
                description = description.Replace("{v" + index++.ToString() + "}", value);
            }
        }


        foreach (AutoTargetEffect effect in OtherEffects)
        {
            IDynamicEffectText[] dtes = effect.GetDynamicTextEffects();

            if(dtes.Length > 0)
            {
                description = effect.ApplyDynamicTextEffect(description, index, context, this);
                index += dtes.Length;
            }
        }
        
        foreach (AutoTargetEffect effect in TurnEndEffect)
        {
            IDynamicEffectText[] dtes = effect.GetDynamicTextEffects();

            if(dtes.Length > 0)
            {
                description = effect.ApplyDynamicTextEffect(description, index, context, this);
                index += dtes.Length;
            }
        }

        description = HighlightKeyWords(description);

        return description;
    }

    public List<string> GetAllKeyWords()
    {
        if (_allKeyWords != null)
            return _allKeyWords;

        _allKeyWords = new();
        List<int> foundPositions = new();
        List<string> allKeys = new(CardTipSystem.Instance.CardTipData.Map.Keys);

        foreach (string key in allKeys)
        {
            string pattern = string.Format(@"\b{0}\b", key);
            Match match = Regex.Match(Description, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
                continue;

            int index = match.Index;

            if(_allKeyWords.Count == 0)
            {
                _allKeyWords.Add(key);
                foundPositions.Add(index);
                continue;
            }

            bool inserted = false;

            for(int i = 0; i < foundPositions.Count; i++)
            {
                if (index < foundPositions[i])
                {
                    foundPositions.Insert(i, index);
                    _allKeyWords.Insert(i, key);
                    inserted = true;
                    break;
                }
            }

            if (!inserted) 
            {
                foundPositions.Add(index);
                _allKeyWords.Add(key);
            }
        }

        return _allKeyWords;
    }

    public HeroView GetOwnerView()
    {
        if (HeroSystem.Instance == null || Owner == null)
            return null;
        HeroView heroView = HeroSystem.Instance.HeroViews.First(h => h.Hero.Data == Owner);
        return heroView;
    }

    private string HighlightKeyWords(string description)
    {
        List<string> keyWords = GetAllKeyWords();

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach (string key in keyWords)
        {
            string pattern = string.Format(@"\b{0}\b", key);
            string replaceWith = string.Format("<b><color=#{0}>{1}</color></b>",ColorUtility.ToHtmlStringRGB(CardTipSystem.Instance.KeyWordColor), textInfo.ToTitleCase(key.ToLower()));

            description = Regex.Replace(description, pattern, replaceWith, RegexOptions.IgnoreCase);
        }

        return description;
    }

    private string RemoveParentheses(string description)
    {
        return Regex.Replace(description, @" \(.*?\)", "");
    }

    public bool IsPlayable(CombatantView caster)
    {
        if (Unplayable)
            return false;

        if (caster == null)
        {
            if (Owner == null)
            {
                HeroView[] heroViews = HeroSystem.Instance.HeroViews;

                foreach (HeroView heroView in heroViews)
                {
                    if (IsPlayable(heroView))
                        return true;
                }

                return false;
            }

            caster = GetOwnerView();
        }

        if (!ManaSystem.Instance.HasEnoughMana(Mana))
            return false;

        EffectContext context = new EffectContext(caster);

        if (ManualTargetType != ManualTargetType.NONE)
        {
            switch (ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    if (CombatantFilters.Count == 0)
                        break;

                    List<CombatantView> allCombatants = BoardSystem.Instance.GetAllCombatants();

                    if (!allCombatants.Any(c => c.IsValid(context, CombatantFilters)))
                        return false;
                    break;
                case ManualTargetType.LANE:
                    List<LaneView> allLanes = BoardSystem.Instance.GetAllLanes();

                    if (!allLanes.Any(c => c.IsValid(context, LaneFilters)))
                        return false;
                    break;
            }
        }

        foreach (Condition condition in RequiredConditions)
        {
            if (!condition.TestCondition(context))
            {
                return false;
            }
        }

        return true;
    }
    
    public bool IsHighlighted()
    {
        return IsHighlighted(new EffectContext(GetOwnerView()));
    }

    public bool IsHighlighted(EffectContext context)
    {
        CombatantView caster = context.Caster;

        if (caster == null)
        {
            HeroView[] heroViews = HeroSystem.Instance.HeroViews;

            foreach(HeroView heroView in heroViews)
            {
                EffectContext newContext = new(heroView);
                if (IsHighlighted(newContext))
                    return true;
            }

            return false;
        }

        IEnumerable<ConditionalAutoTargetEffect> conditionals = OtherEffects.Where(e => e is ConditionalAutoTargetEffect).Select(e => (ConditionalAutoTargetEffect)e);

        if (HighlightConditions.Count == 0 && !OtherEffects.Any(e => e is ConditionalAutoTargetEffect))
            return false;

        foreach (Condition condition in HighlightConditions)
        {
            if (!condition.TestCondition(context))
            {
                return false;
            }
        }

        foreach (ConditionalAutoTargetEffect conditional in conditionals)
        {
           if(conditional.ConditionIsMeetable(context, this))
           {
                return true;
           }

            if (conditionals.Last() == conditional)
                return false;
        }

        return true;
    }

    public CombatantView[] AllValidCombatants(EffectContext context)
    {
        if (ManualTargetType != ManualTargetType.COMBATANT)
            return null;

        List<CombatantView> combatants = BoardSystem.Instance.GetAllCombatants();

        return combatants.FindAll(c => c.IsValid(context, CombatantFilters)).ToArray();
    }
    
    public LaneView[] AllValidLanes(EffectContext context)
    {
        if (ManualTargetType != ManualTargetType.LANE)
            return null;

        List<LaneView> lanes = BoardSystem.Instance.GetAllLanes();

        return lanes.FindAll(c => c.IsValid(context, LaneFilters)).ToArray();
    }

    public bool IsChaotic()
    {
        return GetOwnerView().GetStatusEffectStacks(StatusEffect.CHAOS) > 0 && ManualTargetType != ManualTargetType.NONE && Type == CardType.ATTACK;
    }

    public TargetMode<T> GetChaosTargetMode<T>()
    {
        if (!IsChaotic())
            return null;

        switch (ManualTargetType)
        {
            case ManualTargetType.COMBATANT:
                RandomCTM ctm = new();
                ctm.Filters.AddRange(CombatantFilters);

                return ctm as TargetMode<T>;
            case ManualTargetType.LANE:
                RandomLTM ltm = new();
                ltm.Filters.AddRange(LaneFilters);

                return ltm as TargetMode<T>;
        }

        return null;
    }

    
}
