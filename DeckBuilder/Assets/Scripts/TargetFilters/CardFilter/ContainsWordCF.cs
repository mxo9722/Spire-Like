using System.Text.RegularExpressions;
using UnityEngine;

public class ContainsWordCF : CardFilter
{

    [SerializeField] private string _word;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        string pattern = @"\b" + _word.ToLower() + @"\b";

        if (Regex.IsMatch(target.Title.ToLower(), pattern))
            return true;
        
        if (Regex.IsMatch(target.Description.ToLower(), pattern))
            return true;

        return false;   
    }
}
