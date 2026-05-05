using UnityEngine;

public class ScriptableObjectSEI : SEIdentifier
{

    [SerializeField] private StatusEffectData _statusEffectData;

    public override StatusEffectInfo GetSEInfo()
    {
        return _statusEffectData.Info;
    }
}
