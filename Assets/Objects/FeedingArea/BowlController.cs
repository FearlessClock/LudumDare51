using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlController : Singleton<BowlController>
{
    [SerializeField] private Transform interactionPoint = null;
    public float feedRate = 1;

    public Transform GetInteractionPoint => interactionPoint;
    
    [SerializeField] private int maxFoodCount;
    public int MAXFoodCount => maxFoodCount;
    
    
    private float actualFoodCount;
    public float FoodCount => actualFoodCount;

    private void Start()
    {
        actualFoodCount = maxFoodCount;
    }
    
    void Update()
    {
        if(Input.GetKeyDown("return")) RemoveFood(1);
    }

    public void RefillFood()
    {
        actualFoodCount = maxFoodCount;
    }
    
    public void RemoveFood(float food)
    {
        actualFoodCount -= food;
    }
}
