using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCatAge { KITTEN, CHILD, ADULT}
public class CatAge : MonoBehaviour
{
    [SerializeField] private float timeTillAdult = 10;
    [SerializeField] private float timeTillChild = 10;
    [SerializeField] private float agingPercentage = 0.8f;
    private CatNeeds catNeeds = null;
    private CatSpriteHandler catSpriteHandler = null;
    private eCatAge currentAge = eCatAge.KITTEN;

    private float ageTimer = 0;

    public Action<eCatAge> GrowUp = null;

    public eCatAge GetAge => currentAge;

    private void Awake()
    {
        ageTimer = timeTillChild;
        catNeeds = GetComponent<CatNeeds>();
        catSpriteHandler = GetComponent<CatSpriteHandler>();
    }

    private void Update()
    {
        if(catNeeds.FilledPercentage > agingPercentage)
        {
            ageTimer -= TimeManager.deltaTime;
            if(ageTimer <= 0)
            {
                switch (currentAge)
                {
                    case eCatAge.KITTEN:
                        currentAge = eCatAge.CHILD;
                        ageTimer = timeTillAdult;
                        break;
                    case eCatAge.CHILD:
                        currentAge = eCatAge.ADULT;
                        break;
                }
                catSpriteHandler.UpdateCatSprite();

                GrowUp?.Invoke(currentAge);
                //TODO: Change skin, do grow effect
            }
        }
    }
}
