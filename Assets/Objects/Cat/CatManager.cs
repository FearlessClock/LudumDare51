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
        public CatEquipment catEquipment;
    }

    public List<cat> cats;
    [SerializeField] private CatAge catPrefab;
    [SerializeField] private EventObjectScriptable catDied;
    [SerializeField] private EventScriptable catsUpdated;

    private void Start()
    {
        cats = new List<cat>();
        AddNewCat();
        catDied.AddListener(CatDied); 
    }


    public void AddNewCat()
    {
        cat newCat = new cat();
        newCat.catObject = Instantiate<CatAge>(catPrefab, transform);
        newCat.catEquipment = newCat.catObject.GetComponent<CatEquipment>();
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
            if (!cats[i].catEquipment.HasEquipment && cats[i].catObject.GetAge == eCatAge.ADULT)
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
            if (cats[i].catEquipment.HasEquipment)
            {
                num++;
            }
        }
        return num;
    }

    private void CatDied(object cat)
    {
        GameObject obj = (GameObject)cat;
        for (int i = 0; i < cats.Count; i++)
        {
            if (cats[i].catObject == obj)
            {
                cats.RemoveAt(i);
                cats[i].catObject.GrowUp -= CatGrowUp;
                Destroy(obj);
                return;
            }
        }
        catsUpdated?.Call();
    }

    private bool CatGiveSword()
    {
        for (int i = 0; i < cats.Count; i++)
        {
            if (!cats[i].catEquipment.HasSword && cats[i].catObject.GetAge == eCatAge.ADULT)
            {
                cats[i].catEquipment.Equip(true, false);
                ResourcesManager.Instance.RemoveSword(1);
                return true;
            }
        }
        return false;
    }

    private bool CatGiveArmor()
    {
        for (int i = 0; i < cats.Count; i++)
        {
            if (cats[i].catEquipment.HasSword && !cats[i].catEquipment.HasArmor)
            {
                cats[i].catEquipment.Equip(false, true);
                ResourcesManager.Instance.RemoveArmor(1);
                return true;
            }
        }
        return false;
    }

    private void CatGrowUp(eCatAge catAge)
    {
        if(catAge == eCatAge.ADULT)
        {
            if(ResourcesManager.Instance.ArmorCount > 0)
            {
                CatGiveArmor();
            }
            if (ResourcesManager.Instance.SwordCount > 0)
            {
                CatGiveSword();
            }
            catsUpdated?.Call();
        }
    }

    private void OnDestroy()
    {
        catDied.RemoveListener(CatDied);
    }
}
