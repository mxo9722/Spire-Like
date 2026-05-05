using System;
using UnityEngine;

[Serializable]
public abstract class Modifier
{
    public abstract int GetValue(int oValue, ModifierKey context);
}
