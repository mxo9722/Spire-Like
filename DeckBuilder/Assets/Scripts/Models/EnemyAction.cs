using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAction
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public EnemyActionSymbolType Symbol { get; private set; } = default;
    [field: SerializeField] public List<EnemyActionSymbolType> SecondarySymbols { get; private set; } = new();
    [field: SerializeField, Min(0)] public float Weight { get; private set; } = 1.0f;
    [field: SerializeField, Min(1)] public int Priority { get; private set; } = 1;
    [field: SerializeField, Min(0)] public int ConsecutiveMax { get; private set; } = 0;
    [field: SerializeReference, SR] public List<Condition> Conditions { get; private set; } = null;
    [field: SerializeField] public List<AutoTargetEffect> Effects { get; private set; }
}
