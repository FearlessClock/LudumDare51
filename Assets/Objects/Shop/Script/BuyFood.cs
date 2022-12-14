using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFood : MonoBehaviour, IInteractable
{
    [Header("Boolean")]
    private bool isInteracting = false;
    private bool isEmptying = false;
    
    [Header("Time")]
    [SerializeField] private float interationTime = 0;
    private float actualIntercationTime;
    [SerializeField] private float emptyingTime = 0;
    
    [Header("Resources")] 
    //[SerializeField] private int moneyNeeded;
    //[SerializeField] private int foodGet;
    
    [Header("Ref")]
    [SerializeField] private Image circleIsReady;
    [SerializeField] private GameObject circle;
    [SerializeField] private SpriteRenderer outlineSprite;

    [SerializeField] private BowlController foodContainer;
    private int priority = 2;

    private void Awake()
    {
        actualIntercationTime = interationTime;
        circle.SetActive(false);
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0)) Interation(); 
        else if (Input.GetMouseButtonUp(0)) StopInteration(); */
       
        if(isInteracting) Action();
        else if(isEmptying) EmptyGauge();
    }

    public void Interation()
    {
        if (foodContainer.FoodCount >= foodContainer.MAXFoodCount) return;
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
            foodContainer.RefillFood();
                
            circleIsReady.fillAmount = 0;
            actualIntercationTime = interationTime;

            if (foodContainer.FoodCount >= foodContainer.MAXFoodCount)  StopInteration();
            
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

    public int GetPriority()
    {
        return priority;
    }

    public void ShowOutline(bool show)
    {
        outlineSprite.material.SetFloat("_showOutline", show ? 1 : 0);
    }
}
