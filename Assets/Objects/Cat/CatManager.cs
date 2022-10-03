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
        public CatSpriteHandler catSpriteHandler;
    }

    public List<cat> cats;
    [SerializeField] private CatAge catPrefab;
    [SerializeField] private EventObjectScriptable catDied;
    [SerializeField] private EventScriptable catsUpdated;
    [SerializeField] private EventScriptable armorBought;
    [SerializeField] private EventScriptable swordBought;
    [SerializeField] private int numberOfStartingCats = 3;

    [SerializeField] private EventObjectScriptable deafeat;

    protected override void Awake()
    {
        base.Awake();
        cats = new List<cat>();
        AddNewCat();
        AddNewCat();
        AddNewCat();
        catDied.AddListener(CatDied);
        armorBought.AddListener(CatGiveArmor);
        swordBought.AddListener(CatGiveSword);
        for (int i = 0; i < cats.Count; i++)
        {
            cats[i].catObject.SetAge = eCatAge.ADULT;
        }
        catsUpdated?.Call();
    }


    public void AddNewCat()
    {
        cat newCat = new cat();
        newCat.catObject = Instantiate<CatAge>(catPrefab, transform);
        newCat.catEquipment = newCat.catObject.GetComponent<CatEquipment>();
        newCat.catSpriteHandler = newCat.catObject.GetComponent<CatSpriteHandler>();
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
            if (cats[i].catObject.gameObject == obj)
            {
                cats[i].catObject.GrowUp -= CatGrowUp; 
                cats.RemoveAt(i);
                Destroy(obj);
                break;
            }
        }
        catsUpdated?.Call();

        if (cats.Count <= 0)
        {
            deafeat.Call(obj);
        }
    }

    private void CatGiveSword()
    {
        if (ResourcesManager.Instance.SwordCount > 0)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                if (!cats[i].catEquipment.HasSword && cats[i].catObject.GetAge == eCatAge.ADULT)
                {
                    cats[i].catObject.smokeEffect.Play();
                    cats[i].catEquipment.Equip(true, false);
                    ResourcesManager.Instance.RemoveSword(1);
                    cats[i].catSpriteHandler.UpdateCatSprite();
                    return;
                }
            }
        }
    }

    private void CatGiveArmor()
    {

        if (ResourcesManager.Instance.ArmorCount > 0)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].catEquipment.HasSword && !cats[i].catEquipment.HasArmor)
                {
                    cats[i].catObject.smokeEffect.Play();
                    cats[i].catEquipment.Equip(false, true);
                    ResourcesManager.Instance.RemoveArmor(1);
                    cats[i].catSpriteHandler.UpdateCatSprite();
                    return;
                }
            }
        }
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
        armorBought.RemoveListener(CatGiveArmor);
        swordBought.RemoveListener(CatGiveSword);
    }
}
