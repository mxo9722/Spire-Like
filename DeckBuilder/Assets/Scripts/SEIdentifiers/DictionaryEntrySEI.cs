using UnityEngine;

public class DictionaryEntrySEI : SEIdentifier
{

    [SerializeField] private StatusEffect _key; 

    public DictionaryEntrySEI() { }

    public DictionaryEntrySEI(StatusEffect key)
    {
        _key = key;
    }

    public override StatusEffectInfo GetSEInfo()
    {
        return StatusEffectSystem.GetDictionaryEntry(_key);
    }
}
