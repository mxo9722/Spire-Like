using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ScenePicker
{
    public string scenePath = "";

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(scenePath))
            return;

        SceneManager.LoadScene(scenePath);
    }
}