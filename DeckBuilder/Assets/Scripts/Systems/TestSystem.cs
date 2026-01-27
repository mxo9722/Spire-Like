using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<Card> deckData;

    private void Start()
    {
        CardSystem.Instance.SetUp(deckData);
    }

}
