using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Singleton<CatManager>
{
    public List<GameObject> cats;

    public int NumberOfTotalCats()
    {
        return cats.Count;
    }

    public int NumberOfEggCats()
    {
        return 1;
    }
    
    public int NumberOfFightingCats()
    {
        return 1;
    }
}
