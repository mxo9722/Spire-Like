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

    public abstract IEnumerator Invoke();
}   