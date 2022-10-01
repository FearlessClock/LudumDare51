using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellEgg : MonoBehaviour, IInteractable
{
    [Header("Boolean")]
    private bool isInteracting = false;
    private bool isEmptying = false;
    
    [Header("Time")]
    [SerializeField] private float interationTime = 0;
    private float actualIntercationTime;
    [SerializeField] private float emptyingTime = 0;

    [SerializeField] private Image circleIsReady;
    
    private void Awake()
    {
        actualIntercationTime = interationTime;
    }

    void Update()
    {
       if(Input.GetMouseButtonDown(0)) Interation(); 
       else if (Input.GetMouseButtonUp(0)) StopInteration(); 
       
       if(isInteracting) Action();
       else if(isEmptying) EmptyGauge();
    }

    public void Interation()
    {
        if (ResourcesManager.Instance.EggNumber <= 0) return;
        isInteracting = true;
        isEmptying = false;
    }

    public void StopInteration()
    {
        isInteracting = false;
        isEmptying = true;
    }

    void Action()
    {
        actualIntercationTime -= Time.deltaTime;
        circleIsReady.fillAmount = 1 - (actualIntercationTime / interationTime);

        if (actualIntercationTime <= 0)
        {
            var resources = ResourcesManager.Instance;
            
            resources.RemoveEgg(1);
            resources.AddMoney(10);
                
            circleIsReady.fillAmount = 0;
            actualIntercationTime = interationTime;

            if (resources.EggNumber <= 0)  StopInteration();
            
        }
    }

    void EmptyGauge()
    {
        actualIntercationTime += Time.deltaTime * 2;
        actualIntercationTime = Mathf.Clamp(actualIntercationTime, 0, interationTime);
        circleIsReady.fillAmount = 1 - (actualIntercationTime / interationTime);
        

        if (circleIsReady.fillAmount <= 0) isEmptying = false;
    }
}
