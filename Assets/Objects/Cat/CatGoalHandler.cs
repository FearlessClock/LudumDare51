using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGoalHandler : MonoBehaviour
{
    private Queue<IGoal> goals = new Queue<IGoal>();
    private CatMovement catMovement = null;
    private IGoal currentGoal = null;
    private bool isGoalActive = false;

    private void Awake()
    {
        catMovement = GetComponent<CatMovement>();
    }

    private void Start()
    {
        if(goals.Count == 0)
        {
            AddGoal(new WanderGoal());
        }
    }

    private void NextGoal()
    {
        if(goals.Count > 0)
        {
            StartGoal();
        }
        else
        {
            AddGoal(new WanderGoal());
        }
    }

    public void AddGoal(IGoal newGoal)
    {
        goals.Enqueue(newGoal);
        if (!isGoalActive)
        {
            StartGoal();    
        }
    }

    private void StartGoal()
    {
        isGoalActive = true;
        currentGoal = goals.Dequeue();
        switch (currentGoal.GoalType)
        {
            case eGoalType.TIME:
                catMovement.MoveToPointAndWaitForTime(currentGoal.GetGoalTarget(), (float)currentGoal.GetGoalEnd());
                catMovement.OnFinishedWait += OnGoalDone;
                break;
            case eGoalType.WANDER:
                catMovement.MoveToPointWander();
                catMovement.OnFinishedWait += OnGoalDone;
                break;
            case eGoalType.ENEMY:
                catMovement.MoveToEnemy((Transform)currentGoal.GetGoalEnd());
                break;
            default:
                break;
        }
    }

    public void OnGoalDone()
    {
        isGoalActive = false;

        catMovement.OnFinishedWait -= OnGoalDone;
        NextGoal();
    }
}
