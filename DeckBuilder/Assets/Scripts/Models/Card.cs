using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public bool Unplayable => data.Unplayable;
    public List<Condition> RequiredConditions => data.RequiredConditions;
    public List<Condition> HighlightConditions => data.HighlightConditions;
    public Sprite Image => data.Image;

    public ManualTargetType ManualTargetType => data.ManualTargetType;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    public List<AutoTargetEffect> TurnEndEffect => data.TurnEndEffects;
    public List<CombatantFilter> CombatantFilters => data.CombatantFilters;
    public List<LaneFilter> LaneFilters => data.LaneFilters;
    public bool ExhuastOnUse => data.ExhuastOnUse;

    public int Mana { get; private set; }

    public readonly CardData data;

    private List<string> _allKeyWords = null;

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
    }

    public string GetStaticDescription()
    {
        string description = Description;

        if (ManualTargetEffect is IDynamicEffectText dynamicEffect)
        {
            string value = dynamicEffect.GetStaticText();
            description = description.Replace("{vt}", value);
        }

        List<IDynamicEffectText> dynamicTextEffects = new();

        foreach (AutoTargetEffect effect in OtherEffects)
        {
            IDynamicEffectText dynamicTextEffect = effect.GetDynamicTextEffect();
            if (dynamicTextEffect != null)
                dynamicTextEffects.Add(dynamicTextEffect);
        }

        for (int i = 0; i < dynamicTextEffects.Count; i++)
        {
            string value = dynamicTextEffects[i].GetStaticText();
            description = description.Replace("{v" + i.ToString() + "}", value);
        }

        description = HighlightKeyWords(description);

        return description;
    }

    public string GetDynamicDescription(EffectContext targetModeContext)
    {
        string description = Description;

        if (ManualTargetEffect is IDynamicEffectText dynamicEffect)
        {
            List<CombatantView> targetCombatants = targetModeContext.TargetCombatant ? new() { targetModeContext.TargetCombatant } : null;
            List<LaneView> laneViews = targetModeContext.TargetLane ? new() { targetModeContext.TargetLane } : null;

            string value = dynamicEffect.GetDynamicText(targetModeContext.Caster, targetCombatants, laneViews);
            description = description.Replace("{vt}", value);
        }

        List<AutoTargetEffect> dynamicTextEffects = new();

        foreach (AutoTargetEffect effect in OtherEffects)
        {
            IDynamicEffectText dynamicTextEffect = effect.GetDynamicTextEffect();
            if (dynamicTextEffect != null)
                dynamicTextEffects.Add(effect);
        }

        for (int i = 0; i < dynamicTextEffects.Count; i++)
        {
            string value = dynamicTextEffects[i].GetDynamicText(targetModeContext);
            description = description.Replace("{v" + i.ToString()+"}", value);
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

    public bool IsPlayable(CombatantView caster = null)
    {
        if (Unplayable)
            return false;

        if (caster == null)
            caster = HeroSystem.Instance.HeroView;

        if (!ManaSystem.Instance.HasEnoughMana(Mana))
            return false;

        EffectContext context = new EffectContext(caster);

        if (ManualTargetEffect != null)
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
        return IsHighlighted(EffectContext.CreateHeroEC());
    }

    public bool IsHighlighted(EffectContext context)
    {
        CombatantView caster = context.Caster;

        if (caster == null)
            caster = HeroSystem.Instance.HeroView;

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
        if (ManualTargetEffect == null || ManualTargetType != ManualTargetType.COMBATANT)
            return null;

        List<CombatantView> combatants = BoardSystem.Instance.GetAllCombatants();

        return combatants.FindAll(c => c.IsValid(context, CombatantFilters)).ToArray();
    }
    
    public LaneView[] AllValidLanes(EffectContext context)
    {
        if (ManualTargetEffect == null || ManualTargetType != ManualTargetType.LANE)
            return null;

        List<LaneView> lanes = BoardSystem.Instance.GetAllLanes();

        return lanes.FindAll(c => c.IsValid(context, LaneFilters)).ToArray();
    }
}
