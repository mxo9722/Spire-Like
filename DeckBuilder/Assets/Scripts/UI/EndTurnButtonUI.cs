using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        AfterPlayerTurnGA afterPlayerTurnGA = new();

        if (!ActionSystem.Instance.IsPerforming)
            ActionSystem.Instance.Perform(afterPlayerTurnGA);
    }
}
