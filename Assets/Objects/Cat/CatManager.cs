using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Singleton<CatManager>
{
    [SerializeField]
    public class cat
    {
        public CatAge catObject;
        public bool hasSword;
        public bool hasArmor;
    }

    public List<cat> cats;
    [SerializeField] private CatAge catPrefab;
    [SerializeField] private EventObjectScriptable catDied;
    [SerializeField] private EventScriptable catsUpdated;

    private void Start()
    {
        AddNewCat();
        catDied.AddListener(CatDied);
    }


    public void AddNewCat()
    {
        cat newCat = new cat();
        newCat.hasArmor = false;
        newCat.hasSword = false;
        newCat.catObject = Instantiate<CatAge>(catPrefab, transform);
        newCat.catObject.GrowUp += CatGrowUp;
        cats.Add(newCat);
        catsUpdated?.Call();
    }

    public int NumberOfTotalCats()
    {
        return cats.Count;
    }

    public int NumberOfEggCats()
    {
        int num = 0;
        for (int i = 0; i < cats.Count; i++)
        {
            if (!cats[i].hasSword && cats[i].catObject.GetAge == eCatAge.ADULT)
            {
                num++;
            }
        }
        return num;
    }
    
    public int NumberOfFightingCats()
    {
        int num = 0;
        for (int i = 0; i < cats.Count; i++)
        {
            if (cats[i].hasSword)
            {
                num++;
            }
        }
        return num;
    }

    private void CatDied(object cat)
    {
        catsUpdated?.Call();
        GameObject obj = (GameObject)cat;
        for (int i = 0; i < cats.Count; i++)
        {
            if (cats[i].catObject == obj)
            {
                cats.RemoveAt(i);
                Destroy(obj);
                return;
            }
        }
    }

    public bool CatGetSword()
    {
        catsUpdated?.Call();
        for (int i = 0; i < cats.Count; i++)
        {
            if (!cats[i].hasSword && cats[i].catObject.GetAge == eCatAge.ADULT)
            {
                cats[i].hasSword = true;
                return true;
            }
        }
        return false;
    }
    
    public bool CatGetArmor()
    {
        for (int i = 0; i < cats.Count; i++)
        {
            if (cats[i].hasSword && !cats[i].hasArmor)
            {
                cats[i].hasArmor = true;
                return true;
            }
        }
        return false;
    }

    private void CatGrowUp(eCatAge catAge)
    {
        if(catAge == eCatAge.ADULT)
            catsUpdated?.Call();
    }
}
