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

    public ParticleSystem smokeEffect;

    private float ageTimer = 0;

    public Action<eCatAge> GrowUp = null;

    public eCatAge GetAge => currentAge;
    public eCatAge SetAge{ set { currentAge = value; } }

    private void Awake()
    {
        ageTimer = timeTillChild;
        catNeeds = GetComponent<CatNeeds>();
        catSpriteHandler = GetComponent<CatSpriteHandler>();
    }

    private void Update()
    {
        ageTimer -= TimeManager.deltaTime;
        if(catNeeds.FilledPercentage > agingPercentage)
        {
            if(ageTimer <= 0)
            {
                switch (currentAge)
                {
                    case eCatAge.KITTEN:
                        currentAge = eCatAge.CHILD;
                        ageTimer = timeTillAdult;
                        smokeEffect.Play();
                        break;
                    case eCatAge.CHILD:
                        currentAge = eCatAge.ADULT;
                        smokeEffect.Play();
                        break;
                }
                catSpriteHandler.UpdateCatSprite();

                GrowUp?.Invoke(currentAge);
                //TODO: Change skin, do grow effect
            }
        }
    }
}
