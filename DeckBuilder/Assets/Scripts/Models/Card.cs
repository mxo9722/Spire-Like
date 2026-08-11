using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Card : IHoldData
{
    public string Title => CardData.name;
    public CardData Upgrade => CardData.Upgrade;
    public bool Unplayable => CardData.Unplayable;
    public CardType Type => CardData.Type;
    public Sprite Image => CardData.Image;
    public CardData CardReference => CardData.CardReference;
    public string Description => CardData.Description;
    public List<Condition> RequiredConditions => CardData.RequiredConditions;
    public List<Condition> HighlightConditions => CardData.HighlightConditions;
    public bool JustHighlightCard => CardData.JustHighlightCard;
    public bool ExhuastOnUse => CardData.ExhuastOnUse;
    public bool BaseHasRetain => CardData.Retain;

    public ManualTargetType ManualTargetType => CardData.ManualTargetType;
    public Effect ManualTargetEffect => CardData.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => CardData.OtherEffects;
    public List<CombatantFilter> CombatantFilters => CardData.CombatantFilters;
    public List<LaneFilter> LaneFilters => CardData.LaneFilters;
    public Rarity Rarity => CardData.Rarity;

    public int BaseMana { get; private set; }
    public HeroData Owner { get; private set; } = null;
    public List<CardModifier> Modifiers { get; private set; } = new();

    public readonly CardData CardData;

    private List<CardReaction> _anywhereReactions = new();
    private List<CardReaction> _inHandReactions = new();
    private List<string> _allKeyWords = null;

    private Dictionary<string, object> _data = null;

    public Card(Card card)
    {
        CardData = card.CardData;
        BaseMana = card.BaseMana;
        Owner = card.Owner;

        _data = new(card._data);

        foreach (CardModifier mod in card.Modifiers)
            Modifiers.Add((CardModifier)mod.Clone());

        SetUp();
    }

    public Card(CardData cardData, HeroData owner)
    {
        CardData = cardData;
        BaseMana = cardData.Mana;
        Owner = owner;

        SetUp();
    }

    public Card(CardData cardData)
    {
        CardData = cardData;
        BaseMana = cardData.Mana;

        RunData runData = RunSystem.Instance.RunData;

        Hero possibleOwner1 = runData.Hero1;
        Hero possibleOwner2 = runData.Hero2;

        SetOwner(possibleOwner1, possibleOwner2);
        SetUp();
    }

    private void SetUp()
    {
        if (_data == null)
            _data = new();

        foreach (CardReaction cardReaction in CardData.AnywhereReactions)
        {
            CardReaction reaction = cardReaction.Clone();
            _anywhereReactions.Add(reaction);
            reaction.SetUp(this);
        }
        
        foreach (CardReaction cardReaction in CardData.InHandReactions)
        {
            CardReaction reaction = cardReaction.Clone();
            _inHandReactions.Add(reaction);
            reaction.SetUp(this);
        }
    }

    private void SetOwner(Hero possibleOwner1, Hero possibleOwner2)
    {
        if (possibleOwner1.ClassCards.Contains(CardData) && possibleOwner2.ClassCards.Contains(CardData))
            Owner = null;
        else if (possibleOwner1.ClassCards.Contains(CardData))
            Owner = possibleOwner1.Data;
        else if (possibleOwner2.ClassCards.Contains(CardData))
            Owner = possibleOwner2.Data;
        else
            Owner = null;
    }

    public string GetStaticDescription()
    {
        string description = Description;

        description = RemoveParentheses(description);

        foreach (CardModifier modifier in Modifiers)
        {
            description = modifier.ModifyDescription(description);
        }

        List<IDynamicEffectText> dynamicTextEffects = new();

        if (ManualTargetEffect != null)
        {
            dynamicTextEffects.AddRange(ManualTargetEffect.GetDynamicTextEffects());
        }

        foreach (AutoTargetEffect effect in OtherEffects)
        {
            dynamicTextEffects.AddRange(effect.GetDynamicTextEffects());
        }

        foreach (CardReaction reaction in _anywhereReactions)
        {
            dynamicTextEffects.AddRange(reaction.GetDynamicTextEffects());
        }
        
        foreach (CardReaction reaction in _inHandReactions)
        {
            dynamicTextEffects.AddRange(reaction.GetDynamicTextEffects());
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

        foreach (CardModifier modifier in Modifiers)
        {
            description = modifier.ModifyDescription(description);
        }

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

            if (dtes.Length > 0)
            {
                description = effect.ApplyDynamicTextEffect(description, index, context, this);
                index += dtes.Length;
            }
        }

        foreach (CardReaction reaction in _anywhereReactions)
        {
            IDynamicEffectText[] dtes = reaction.GetDynamicTextEffects();

            if (dtes.Length > 0)
            {
                description = reaction.ApplyDynamicTextEffect(description, index, context, this);
                index += dtes.Length;
            }
        }
        
        foreach (CardReaction reaction in _inHandReactions)
        {
            IDynamicEffectText[] dtes = reaction.GetDynamicTextEffects();

            if (dtes.Length > 0)
            {
                description = reaction.ApplyDynamicTextEffect(description, index, context, this);
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

        string description = GetStaticDescription();

        foreach (string key in allKeys)
        {
            string newKey = key.Replace(" X", " ([0-9]+)");
            string pattern = string.Format(@"\b{0}\b", newKey);
            Match match = Regex.Match(description, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
                continue;

            int index = match.Index;

            if (_allKeyWords.Count == 0)
            {
                _allKeyWords.Add(match.Value);
                foundPositions.Add(index);
                continue;
            }

            bool inserted = false;

            for (int i = 0; i < foundPositions.Count; i++)
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

    public HeroView GetOwnerView(EffectContext context = null)
    {
        if (HeroSystem.Instance == null || (context == null && Owner == null))
            return null;

        if(Owner == null)
        {
            switch (ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    return (HeroView)context.TargetCombatant?.Slot.Lane.HeroView;
                case ManualTargetType.LANE:
                    return (HeroView)context.TargetLane?.HeroView;
                case ManualTargetType.NONE:
                    return null;
            }
        }

        HeroView heroView = HeroSystem.Instance.HeroViews.First(h => h.Hero.Data == Owner);
        return heroView;
    }

    private string HighlightKeyWords(string description)
    {
        List<string> keyWords = GetAllKeyWords();

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach (string key in keyWords)
        {
            key.Replace(" X", " ([0-9]+)");
            string pattern = string.Format(@"\b{0}\b", key);
            string replaceWith = string.Format("<b><color=#{0}>{1}</color></b>", ColorUtility.ToHtmlStringRGB(CardTipSystem.Instance.KeyWordColor), textInfo.ToTitleCase(key.ToLower()));

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

        EffectContext context = new EffectContext(caster);

        if (!ManaSystem.Instance.HasEnoughMana(GetDynamicManaValue(context)))
            return false;

        if (ManualTargetType != ManualTargetType.NONE)
        {
            switch (ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    if (CombatantFilters.Count == 0)
                        break;

                    List<CombatantView> allCombatants = BoardSystem.Instance.GetAllCombatants();

                    if (!allCombatants.Any(c => c.IsValid(context, CombatantFilters) && c.IsSelectable()))
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

    public bool IsHighlighted(EffectContext context)
    {
        CombatantView caster = context.Caster;

        if (caster == null)
        {
            caster = GetOwnerView(context);
        }

        IEnumerable<ConditionalAutoTargetEffect> conditionals = OtherEffects.Where(e => e is ConditionalAutoTargetEffect).Select(e => (ConditionalAutoTargetEffect)e);

        if (HighlightConditions.Count == 0 && !OtherEffects.Any(e => e is ConditionalAutoTargetEffect))
            return false;

        switch (CardData.ManualTargetType)
        {
            case ManualTargetType.COMBATANT:
                if(context.TargetCombatant == null)
                {
                    CombatantView[] pTargets = AllValidCombatants(context);

                    foreach(CombatantView target in pTargets)
                    {
                        EffectContext targetContext = new(caster, manualTargetCombatant: target, playedCard: this);

                        if (IsHighlighted(targetContext))
                            return true;
                    }
                }
                break;

            case ManualTargetType.LANE:
                if (context.TargetLane == null)
                {
                    LaneView[] pTargets = AllValidLanes(context);

                    foreach (LaneView target in pTargets)
                    {
                        EffectContext targetContext = new(caster, manualTargetLane: target, playedCard: this);

                        if (IsHighlighted(targetContext))
                            return true;
                    }
                }
                break;
        }

        foreach (Condition condition in HighlightConditions)
        {
            if (!condition.TestCondition(context))
            {
                return false;
            }
        }

        foreach (ConditionalAutoTargetEffect conditional in conditionals)
        {
            if (conditional.ConditionIsMeetable(context, this))
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

        if (context.Caster == null) 
        {

            List<EffectContext> contexts = new();

            foreach(HeroView hero in HeroSystem.Instance.HeroViews)
            {
                contexts.Add(new(hero, context.TargetLane, context.TargetCombatant, this));
            }

            return combatants.FindAll(c => contexts.Any(heroContext => c.IsValid(heroContext, CombatantFilters))).ToArray();
        }

        return combatants.FindAll(c => c.IsValid(context, CombatantFilters)).ToArray();
    }

    public LaneView[] AllValidLanes(EffectContext context)
    {
        if (ManualTargetType != ManualTargetType.LANE)
            return null;

        List<LaneView> lanes = BoardSystem.Instance.GetAllLanes();

        return lanes.FindAll(c => c.IsValid(context, LaneFilters)).ToArray();
    }

    public void UnsubscribeAllReactions()
    {
        UnsubscribeReactions(_anywhereReactions);
        UnsubscribeReactions(_inHandReactions);
    }

    private void UnsubscribeReactions(List<CardReaction> reactions)
    {
        foreach (CardReaction reaction in reactions)
        {
            reaction.Unsubscribe();
        }
    }

    public void SubscribeAnywhereReactions()
    {
        foreach (CardReaction reaction in _anywhereReactions)
        {
            reaction.Subscribe();
        }
    }

    public void SubscribeInHand()
    {
        foreach (CardReaction reaction in _inHandReactions)
        {
            reaction.Subscribe();
        }
    }

    public bool IsChaotic()
    {
        if (GetOwnerView() == null)
            return false;

        return GetOwnerView().GetStatusEffectStacks(StatusEffect.CHAOS) > 0 && ManualTargetType != ManualTargetType.NONE && Type == CardType.ATTACK;
    }

    public bool IsHeatWarning(EffectContext context)
    {
        CombatantView owner = GetOwnerView(context);

        if (owner == null)
            return false;

        int heatValue = owner.GetStatusEffectStacks(StatusEffect.HEAT);

        if (heatValue > 0)
            heatValue--;

        foreach(AutoTargetEffect effect in OtherEffects)
        {
            if(effect is AutoCombatantTargetEffect actEffect && actEffect.Effect is AddStatusEffectEffect addStatusEffectEffect)
            {
                if (addStatusEffectEffect.StatusEffectType != StatusEffect.HEAT)
                    continue;

                List<CombatantView> targets = actEffect.TargetMode.AllPossibleTargets(context);

                if (targets.Contains(owner))
                    heatValue += addStatusEffectEffect.StackCount.GetAmount(context);
            }
        }

        return heatValue >= 10;
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


    public override string ToString()
    {
        return CardData.name;
    }

    public void SetData(string key, object data)
    {
        if (!_data.ContainsKey(key))
            _data.Add(key, data);
        else
            _data[key] = data;
    }

    public T GetData<T>(string key)
    {
        if (_data.ContainsKey(key))
            return (T)_data[key];

        return default(T);
    }

    public bool ContainsKey(string key)
    {
        return _data.ContainsKey(key);
    }

    public void AddCardModifier(CardModifier cardModifier)
    {
        cardModifier = (CardModifier)cardModifier.Clone();

        if (!cardModifier.CanApply(this))
            return;

        foreach(CardModifier mod in Modifiers)
        {
            bool combined = mod.TryToCombine(cardModifier);
            if (combined)
                return;
        }

        Modifiers.Add(cardModifier);
    }

    public T GetCardModifier<T>() where T : CardModifier
    {
        foreach(CardModifier modifier in Modifiers)
        {
            if (modifier is T t)
                return t;
        }
        
        return null;
    }
    
    public bool GetCardModifier<T>(out T modifier) where T : CardModifier
    {
        modifier = GetCardModifier<T>();
        
        return modifier != null;
    }

    public int GetDynamicManaValue(EffectContext context)
    {
        int value = BaseMana;

        ReduceCostCM reduceCostCM = GetCardModifier<ReduceCostCM>();
        if(reduceCostCM != null)
            value = Math.Max(0, value - reduceCostCM.Reduction);

        value = ConditionalModifierSystem.Instance.ModifyValue(value, new ManaModKey(context));

        value = Math.Max(value, 0);

        return value;
    }

    public int GetStaticManaValue()
    {
        return BaseMana;
    }

    public List<StatusEffect> GetAllStatusEffects()
    {
        List<StatusEffect> statusEffects = new();

        if(ManualTargetEffect != null)
            statusEffects.AddRange(ManualTargetEffect.GetAllStatusEffects());
        
        statusEffects.AddRange(OtherEffects.SelectMany(e => e.GetAllStatusEffects()));
        statusEffects.AddRange(_anywhereReactions.SelectMany(r => r.GetAllStatusEffects()));
        statusEffects.AddRange(_inHandReactions.SelectMany(r => r.GetAllStatusEffects()));

        return statusEffects.Distinct().ToList();
    }

    public bool GetRetain()
    {
        if(GetCardModifier(out RetainCM retainCM))
        {
            return retainCM.AddRetain;
        }

        return BaseHasRetain;
    }
}
