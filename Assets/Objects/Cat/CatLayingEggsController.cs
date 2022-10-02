using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLayingEggsController : MonoBehaviour
{
    private CatNeeds catNeeds = null;
    [Range(0f, 1f)]
    [SerializeField] private float chanceToLayEgg = 0;
    [SerializeField] private float minimumNeedsToLayEgg = 0.9f;
    [SerializeField] private float needsUsedToLayEggPercentage = 0.5f;
    [SerializeField] private GameObject eggPrefab = null;
    [SerializeField] private float eggLayCooldownTime = 5;
    [SerializeField] private float waitBetweenChecks = 2;
    private float waitBetweenChecksTimer = 0;
    private float eggLayTimer = 0;
    private CatAge catAge;

    private void Awake()
    {
        catNeeds = GetComponent<CatNeeds>();
        catAge = GetComponent<CatAge>();
    }

    private void Update()
    {
        if (catAge.GetAge == eCatAge.ADULT)
        {
            waitBetweenChecksTimer -= TimeManager.deltaTime;
            if (waitBetweenChecksTimer < 0)
            {
                waitBetweenChecksTimer = waitBetweenChecks;
                eggLayTimer -= TimeManager.deltaTime;
                if (eggLayTimer < 0)
                {
                    if (catNeeds.FilledPercentage > minimumNeedsToLayEgg)
                    {
                        if (Random.value < chanceToLayEgg)
                        {
                            LayEgg();
                            catNeeds.UseNeeds(needsUsedToLayEggPercentage);
                        }
                    }
                }
            }
        }
    }

    private void LayEgg()
    {
        Instantiate<GameObject>(eggPrefab, this.transform.position, Quaternion.identity) ;
        eggLayTimer = eggLayCooldownTime;
    }
}
