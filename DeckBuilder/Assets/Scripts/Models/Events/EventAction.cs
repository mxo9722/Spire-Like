using AYellowpaper.SerializedCollections;
using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public abstract class EventAction
{
    [SerializeField] public int ID { get; private set; } = 0;

    public abstract IEnumerator Invoke();

    public void SetID(int id)
    {
        ID = id;
    }
}   