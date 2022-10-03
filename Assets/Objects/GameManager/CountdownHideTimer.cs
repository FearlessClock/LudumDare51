using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownHideTimer : MonoBehaviour
{

    [SerializeField] private EventObjectScriptable lose;
    [SerializeField] private EventObjectScriptable win;

    private void Start()
    {
        lose.AddListener(StopTimer);
        win.AddListener(StopTimer);
    }

    public void StopTimer(object obj)
    {
        this.gameObject.SetActive(false);
    }
}
