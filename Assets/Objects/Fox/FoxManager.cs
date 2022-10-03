using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxManager : Singleton<FoxManager>
{
    public List<GameObject> foxes;
    [SerializeField] private EventObjectScriptable foxDiedEvent;
    [SerializeField] private int moneyGainOnFoxDeath = 5;
    [SerializeField] private EventObjectScriptable lastWave;
    [SerializeField] private EventObjectScriptable lose;
    private bool isLastWave = false;

    protected override void Awake()
    {
        base.Awake();
        foxDiedEvent?.AddListener(FoxDied);
        lastWave?.AddListener(LastWave);
    }

    private void Update()
    {
        if (isLastWave)
        {
            if (foxes.Count <= 0)
                lose.Call(null);
        }
    }

    public void DeleteAllFoxes()
    { 
        for (int i = foxes.Count-1; i >= 0; i--)
        {
            GameObject obj = foxes[i];
            foxes.Remove(obj);
            Destroy(obj);
        }
    }

    public void FoxDied(object fox)
    {
        GameObject obj = (GameObject)fox;
        foxes.Remove(obj);
        Destroy(obj);
        ResourcesManager.Instance.AddMoney(moneyGainOnFoxDeath);
    }

    private void LastWave(object obj)
    {
        isLastWave = true;  
    }
}
