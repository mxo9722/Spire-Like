using System;
using System.Collections;
using UnityEngine;

public interface INeedsUserInput
{
    public abstract IEnumerator WaitForUserInput(EffectContext context);
}
