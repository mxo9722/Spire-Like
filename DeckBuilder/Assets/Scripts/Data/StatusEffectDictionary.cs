using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectsData", menuName = "Data/StatusEffectsData")]
public class StatusEffectsDictionary : ScriptableObject
{
    [SerializedDictionary("Status Effect Type", "Properties")]
    [SerializeField] public SerializedDictionary<StatusEffect, StatusEffectData> NewMap = new();
#if UNITY_EDITOR
    [ProButton]
    void ConvertAll()
    {
        foreach (StatusEffect se in NewMap.Keys)
        {
            StatusEffectData data = CreateInstance<StatusEffectData>();
            data.SetUp(NewMap[se].Info);
            AssetDatabase.CreateAsset(data, "Assets/TempSavePlace/SE_"+se.ToString()+".asset");
        }
    }
#endif
}
