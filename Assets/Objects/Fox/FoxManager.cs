using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxManager : PersistentSingleton<FoxManager>
{
    public List<GameObject> foxes;

    protected override void Awake()
    {
        base.Awake();
    }
    
    public void DeleteAllFoxes()
    { 
        for (int i = foxes.Count; i >= 0; i--)
        {
            Destroy(foxes[i]);
        }
    }
        
}
