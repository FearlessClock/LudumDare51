using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxManager : PersistentSingleton<FoxManager>
{
    public List<GameObject> foxes;
    [SerializeField] private EventObjectScriptable foxDiedEvent;

    protected override void Awake()
    {
        base.Awake();
        foxDiedEvent?.AddListener(FoxDied);
    }
    
    public void DeleteAllFoxes()
    { 
        for (int i = foxes.Count; i >= 0; i--)
        {
            Destroy(foxes[i]);
        }
    }

    public void FoxDied(object fox)
    {
        GameObject obj = (GameObject)fox;
        foxes.Remove(obj);
        Destroy(obj);
    }
}
