using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndWaitEventGoal : IGoal
{
    private Vector3 target;

    public MoveAndWaitEventGoal(Vector3 target)
    {
        this.target = target;
    }

    public eGoalType GoalType => eGoalType.EVENT;

    public Vector3 GetGoalTarget()
    {
        return target;
    }

    public object GetGoalEnd()
    {
        return null;
    }
}
