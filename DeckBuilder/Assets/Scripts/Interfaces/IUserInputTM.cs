using System;
using System.Collections;
using UnityEngine;

public interface IUserInputTM
{
    public abstract IEnumerator WaitForUserInput(EffectContext context);
}
