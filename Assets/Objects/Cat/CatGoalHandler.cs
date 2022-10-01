using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGoalHandler : MonoBehaviour
{
    private Queue<IGoal> goals = new Queue<IGoal>();
    private CatMovement catMovement = null;
    private IGoal currentGoal = null;

    private void Awake()
    {
        catMovement = GetComponent<CatMovement>();
        catMovement.OnReachTarget += NextGoal;
    }

    private void NextGoal()
    {
        if(goals.Count > 0)
        {
            StartGoal();
        }
    }

    public void AddGoal(IGoal newGoal)
    {
        goals.Enqueue(newGoal);
        if (goals.Count == 1)
        {
            StartGoal();
        }
    }

    private void StartGoal()
    {
        currentGoal = goals.Dequeue();
        switch (currentGoal.GoalType)
        {
            case eGoalType.TIME:
                catMovement.MoveToPointAndWaitForTime(currentGoal.GetGoalTarget(), (float)currentGoal.GetGoalEnd());
                break;
            case eGoalType.EVENT:
                catMovement.MoveToPointAndWaitForEvent(currentGoal.GetGoalTarget());
                Action callback = currentGoal.GetGoalEnd() as Action;
                callback += OnWaitDone;
                break;
            default:
                break;
        }
    }

    private void OnWaitDone()
    {
        Action callback = currentGoal.GetGoalEnd() as Action;
        callback += OnWaitDone;
    }
}
