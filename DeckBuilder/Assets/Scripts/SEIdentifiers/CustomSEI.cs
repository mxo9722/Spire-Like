using SerializeReferenceEditor;
using UnityEngine;

public class CustomSEI : SEIdentifier
{
    [field:SerializeReference,SR] private StatusEffectInfo _info;

    public override StatusEffectInfo GetSEInfo()
    {
        return _info;
    }
}
