using UnityEngine;

public class RetainCM : CardModifier
{
    public bool AddRetain { get; private set; } = true;

    public RetainCM(bool addRetain)
    {
        AddRetain = addRetain;
    }

    public override bool CanApply(Card card)
    {
        return card.BaseHasRetain != AddRetain;
    }

    public override object Clone()
    {
        return new RetainCM(AddRetain);
    }

    public override bool TryToCombine(CardModifier cardModifier)
    {
        if (cardModifier is RetainCM ret)
        {
            AddRetain = ret.AddRetain;
            return true;
        }
        return false;
    }

    public override string ModifyDescription(string baseDescription)
    {
        if (AddRetain)
            return "<b>Hoard.</b>\n"+baseDescription;

        return baseDescription.Replace("Hoard.", "");
    }
}
