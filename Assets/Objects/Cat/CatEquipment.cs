using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEquipment : MonoBehaviour
{
    private bool hasEquipment = false;
    public bool HasEquipment => hasEquipment;
    private CatAge catAge = null;
    public bool CanEquipEquipment => catAge.GetAge == eCatAge.ADULT;

    public bool HasSword => hasSword;
    public bool HasArmor => hasArmor;

    private bool hasSword = false;
    private bool hasArmor = false;

    private void Awake()
    {
        catAge = GetComponent<CatAge>();
    }

    public void Equip(bool sword, bool armor)
    {
        if(sword || armor)
        {
            hasEquipment = true;
        }
        hasSword = hasSword | sword;
        hasArmor = hasArmor | armor;
        //TODO: Change to proper equipment class, add equipment to character, add equip effects
    }
}
