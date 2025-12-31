using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        AfterPlayerTurnGA afterPlayerTurnGA = new();
        ActionSystem.Instance.Perform(afterPlayerTurnGA);
    }
}
