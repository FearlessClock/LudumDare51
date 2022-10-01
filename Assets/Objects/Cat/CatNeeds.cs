using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatNeeds : MonoBehaviour
{
    [SerializeField] private float maxHunger = 1;
    private float currentHunger = 1;
    [SerializeField] private float lossOfHungerRate = 0.1f;
    [Range(0f, 1f)]
    [SerializeField] private float hungerCutoffPercentage = 0.5f;

    private bool hasSentWant = false;
    private CatGoalHandler goalHandler = null;

    public float FilledPercentage => currentHunger / maxHunger;

    private void Awake()
    {
        goalHandler = GetComponent<CatGoalHandler>();
        currentHunger = maxHunger;
    }

    private void Update()
    {
        currentHunger -= lossOfHungerRate * Time.deltaTime;
        if(!hasSentWant && FilledPercentage < hungerCutoffPercentage)
        {
            hasSentWant = true;
            goalHandler.AddGoal(new MoveToGoal(BowlController.Instance.GetInteractionPoint.position, 3));
        }
        else if(FilledPercentage > hungerCutoffPercentage && hasSentWant)
        {
            hasSentWant = false;
        }
        if(currentHunger< 0)
        {
            currentHunger = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FeedingZone"))
        {
            currentHunger += collision.gameObject.GetComponent<BowlController>().feedRate * Time.deltaTime;
            if (currentHunger >= maxHunger)
            {
                currentHunger = maxHunger;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position + Vector3.up*0.7f, this.transform.position + Vector3.up * 0.7f + Vector3.right * currentHunger);
    }

    public void UseNeeds(float needsUsedToLayEggPercentage)
    {
        currentHunger -= needsUsedToLayEggPercentage;
    }
}
