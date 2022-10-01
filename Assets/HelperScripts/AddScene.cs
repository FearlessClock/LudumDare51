using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddScene : MonoBehaviour
{
    AsyncOperation loadProgress = null;
    public void AddSceneCallback(StringVariable name)
    {
        if(name != null)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            if (scene.name != name.value)
            {
                loadProgress = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            }
        }
        else
        {
            Debug.LogWarning("Please set a valid name string variable");
        }
    }

    public float SceneAddProgress()
    {
        return loadProgress.progress;
    }

    public bool IsSceneAddDone()
    {
        return loadProgress.isDone;
    }

    public void RemoveScene(StringVariable name)
    {
        SceneManager.UnloadSceneAsync(name.value);
    }
}
