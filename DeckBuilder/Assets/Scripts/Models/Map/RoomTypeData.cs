using UnityEngine;

[System.Serializable]
public class RoomTypeData
{
    [field: SerializeField, Min(0)] public float Weight { get; private set; }
    [field: SerializeField, Min(0)] public int LowestLevel { get; private set; }
    [field: SerializeField] public bool RepeatsAllowed { get; private set; } = false;
}
