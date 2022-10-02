using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEquipment : MonoBehaviour
{
    private bool hasEquipment = true;
    public bool HasEquipment => hasEquipment;
    private CatAge catAge = null;
    public bool CanEquipEquipment => catAge.GetAge == eCatAge.ADULT;

    private void Awake()
    {
        catAge = GetComponent<CatAge>();
    }

    public void Equip(object equipment)
    {
        hasEquipment = true;
        //TODO: Change to proper equipment class, add equipment to character, add equip effects
    }
}
