using System.Collections.Generic;
using UnityEngine;

public class LeaveShopButtonUI : MonoBehaviour
{

    [SerializeField] private ScenePicker _mapScene;

    public void OnClick()
    {
        RunSystem.Instance.GetRoom()?.SetCompleted();
        _mapScene.LoadScene();
    }
}
