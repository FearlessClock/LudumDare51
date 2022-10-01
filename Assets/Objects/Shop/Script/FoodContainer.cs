using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : MonoBehaviour
{
    [SerializeField] private int maxFoodCount;
    private int actualFoodCount;
    public int FoodCount => actualFoodCount;

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
    
    public void RemoveFood(int food)
    {
        actualFoodCount -= food;
    }
}
