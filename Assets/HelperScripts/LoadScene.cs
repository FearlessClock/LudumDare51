using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    [SerializeField] private StringVariable loadSceneEffectName = null;
    [SerializeField] private StringVariable newSceneToLoad = null;

    [SerializeField] private EventObjectScriptable newSceneLoaded;

    public void LoadSceneCallback(StringVariable name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        newSceneLoaded?.Call(name.value);
    }

    public void LoadSceneWithFadeCallback(StringVariable name)
    {
        if (name != null)
        {
            newSceneToLoad.SetValue(name);
            Scene scene = SceneManager.GetSceneByName(loadSceneEffectName);
            if (scene.name != loadSceneEffectName.value)
            {
                SceneManager.LoadSceneAsync(loadSceneEffectName, LoadSceneMode.Additive);
            }
            newSceneLoaded.Call(name.value);
        }
        else
        {
            Debug.LogWarning("Please set a valid name string variable");
        }
    }

    public void SetSceneToLoadName(StringVariable name)
    {
        newSceneToLoad.SetValue(name.value);

    }

    public void LoadCurrentSavedScene()
    {
        LoadSceneWithFadeCallback(newSceneToLoad);
    }
}
