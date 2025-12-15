using System;
using UnityEngine;

[Serializable]
public abstract class Reward
{
    public abstract SetReward GetSetReward();
    public abstract void SetUp();
}
