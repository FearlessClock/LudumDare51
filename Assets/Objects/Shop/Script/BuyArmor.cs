using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyArmor : MonoBehaviour, IInteractable
{
    [Header("Boolean")]
    private bool isInteracting = false;
    private bool isEmptying = false;
    
    [Header("Time")]
    [SerializeField] private float interationTime = 0;
    private float actualIntercationTime;
    [SerializeField] private float emptyingTime = 0;
    
    [Header("Resources")] 
    [SerializeField] private int moneyNeeded;
    [SerializeField] private int armorGet;
    
    [Header("Ref")]
    [SerializeField] private Image circleIsReady;
    [SerializeField] private GameObject circle;

    [SerializeField] private EventScriptable armorBought;
    private SoundTransmitter st;
    
    private void Awake()
    {
        actualIntercationTime = interationTime;
        circle.SetActive(false);
        st = GetComponent<SoundTransmitter>();
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(1)) Interation(); 
        else if (Input.GetMouseButtonUp(1)) StopInteration(); */
       
        if(isInteracting) Action();
        else if(isEmptying) EmptyGauge();
    }

    public void Interation()
    {
        if (ResourcesManager.Instance.MoneyCount <= moneyNeeded) return;
        circle.SetActive(true);
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
            
            resources.RemoveMoney(moneyNeeded);
            resources.AddArmor(armorGet);
            armorBought?.Call(); 
            circleIsReady.fillAmount = 0;
            actualIntercationTime = interationTime;
            st.Play("Coin");

            if (resources.MoneyCount < moneyNeeded)  StopInteration();
            
        }
    }

    void EmptyGauge()
    {
        actualIntercationTime += Time.deltaTime * 2;
        actualIntercationTime = Mathf.Clamp(actualIntercationTime, 0, interationTime);
        circleIsReady.fillAmount = 1 - (actualIntercationTime / interationTime);


        if (circleIsReady.fillAmount <= 0)
        {
            isEmptying = false;
            circle.SetActive(false);
        }
    }
}
