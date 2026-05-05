using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Data/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    [field: SerializeField] public DefinitionSEInfo Info { get; private set; }

    public void SetUp(DefinitionSEInfo info)
    {
        Info = info;
    }
}
