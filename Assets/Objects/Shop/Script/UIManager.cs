using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI coinCount;
    public TextMeshProUGUI CoinCount
    {
        get => coinCount;
        set => coinCount = value;
    }
    [SerializeField] private TextMeshProUGUI eggCount;

    public TextMeshProUGUI EggCount
    {
        get => eggCount;
        set => eggCount = value;
    }
    
    [SerializeField] private TextMeshProUGUI catCount;
    public TextMeshProUGUI CatCount
    {
        get => catCount;
        set => catCount = value;
    }
    
    [SerializeField] private TextMeshProUGUI loveCatCount;
    public TextMeshProUGUI LoveCatCount
    {
        get => loveCatCount;
        set => loveCatCount = value;
    }
    
    [SerializeField] private TextMeshProUGUI swordCatCount;
    public TextMeshProUGUI SwordCatCount
    {
        get => swordCatCount;
        set => swordCatCount = value;
    }
    
    private void Start()
    {
        eggCount.text = ResourcesManager.Instance.EggNumber.ToString();
        coinCount.text = ResourcesManager.Instance.MoneyCount.ToString();
        catCount.text = CatManager.Instance.NumberOfTotalCats().ToString();
        loveCatCount.text = CatManager.Instance.NumberOfEggCats().ToString();
        swordCatCount.text = CatManager.Instance.NumberOfFightingCats().ToString();
    }
}
