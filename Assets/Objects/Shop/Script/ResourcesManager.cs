using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    [SerializeField] private int eggNumber;
    public int EggNumber => eggNumber;

    [SerializeField] private int moneyCount;
    public int MoneyCount => moneyCount;
    
    [SerializeField] private int swordCount;
    public int SwordCount => swordCount;
    
    [SerializeField] private int armorCount;
    public int ArmorCount => armorCount;

    public void AddEgg(int egg)
    {
        eggNumber += egg;
        UIManager.Instance.EggCount.text = eggNumber.ToString();
    }
    
    public void RemoveEgg(int egg)
    {
        eggNumber -= egg;
        UIManager.Instance.EggCount.text = eggNumber.ToString();
    }
    
    public void AddMoney(int money)
    {
        moneyCount += money;
        UIManager.Instance.CoinCount.text = moneyCount.ToString();
    }
    
    public void RemoveMoney(int money)
    {
        moneyCount -= money;
        UIManager.Instance.CoinCount.text = moneyCount.ToString();
    }
    
    public void AddSword(int sword)
    {
        swordCount += sword;
    }
    
    public void RemoveSword(int sword)
    {
        swordCount -= sword;
    }
    
    public void AddArmor(int armor)
    {
        armorCount += armor;
    }
    
    public void RemoveArmor(int armor)
    {
        armorCount -= armor;
    }
}
