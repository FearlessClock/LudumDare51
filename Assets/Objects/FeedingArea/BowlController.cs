using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlController : Singleton<BowlController>, IInteractable
{
    [SerializeField] private Transform interactionPoint = null;
    public float feedRate = 1;

    public Transform GetInteractionPoint => interactionPoint;

    [SerializeField] private int maxFoodCount;
    public int MAXFoodCount => maxFoodCount;

    [SerializeField] private Transform minFoodHeight = null; 
    [SerializeField] private Transform maxFoodHeight = null; 
    [SerializeField] private Transform foodInBowl = null; 

    private float currentFoodAmount;
    public float FoodCount => currentFoodAmount;

    public bool BowlHasFood => currentFoodAmount > 0;

    [Header("Boolean")]
    private bool isInteracting = false;
    private bool isEmptying = false;

    [Header("Time")]
    [SerializeField] private float interationTime = 0;
    private float actualIntercationTime;
    [SerializeField] private float emptyingTime = 0;

    [Header("Ref")]
    [SerializeField] private Image circleIsReady;
    [SerializeField] private GameObject circle;
    [SerializeField] private int priority = 6;

    private void Start()
    {
        currentFoodAmount = maxFoodCount;
        circle.SetActive(false);
    }

    void Update()
    {
        //if(Input.GetKeyDown("return")) RemoveFood(1);

        if (isInteracting) Action();
        else if (isEmptying) EmptyGauge();
    }

    public void RefillFood()
    {
        currentFoodAmount = maxFoodCount;
        foodInBowl.DOMoveY(maxFoodHeight.position.y, 1);
    }

    public void RemoveFood(float food)
    {
        currentFoodAmount -= food;
        foodInBowl.DOKill();
        foodInBowl.DOMoveY(Mathf.Lerp(minFoodHeight.position.y, maxFoodHeight.position.y, currentFoodAmount/ maxFoodCount), 0.3f);
    }

    public void Interation()
    {
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
            circleIsReady.fillAmount = 0;
            actualIntercationTime = interationTime;
            RefillFood();
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
}
