using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    [SerializeField] private int eggNumber;
    public int EggNumber => eggNumber;

    [SerializeField] private int moneyCount;
    public int MoneyCount => moneyCount;

    public void AddEgg(int egg)
    {
        eggNumber += egg;
    }
    
    public void RemoveEgg(int egg)
    {
        eggNumber -= egg;
    }
    
    public void AddMoney(int money)
    {
        moneyCount += money;
    }
    
    public void RemoveMoney(int money)
    {
        moneyCount -= money;
    }
}
