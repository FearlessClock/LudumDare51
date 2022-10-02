using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpriteHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private List<Sprite> catImages;
    private CatAge catAge;
    private CatEquipment catEquipment;

    void Start() 
    {
        catAge = GetComponent<CatAge>();
        catEquipment = GetComponent<CatEquipment>();
        UpdateCatSprite();
    }

    public void UpdateCatSprite()
    {
        int i = 0;
        if(catAge.GetAge == eCatAge.ADULT)
        {
            if (catEquipment.HasArmor)
            {
                i = 4;
            }else if (catEquipment.HasSword)
            {
                i = 3;
            }
            else
            {
                i = 2;
            }
        }
        else
        {
            i = (int)catAge.GetAge;
        }
        sprite.sprite = catImages[i];
    }
}
