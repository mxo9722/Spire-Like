using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;

    public ManualTargetType ManualTargetType => data.ManualTargetType;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
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

    public string GetDynamicDescription(TargetModeContext targetModeContext)
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

    private string ToHex(Color c) => $"#{c.r:X2}{c.g:X2}{c.b:X2}";
}
