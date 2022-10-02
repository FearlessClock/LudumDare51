using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderGoal : IGoal
{
    public eGoalType GoalType => eGoalType.WANDER;

    public object GetGoalEnd()
    {
        return null;
    }

    public Vector3 GetGoalTarget()
    {
        return Vector3.zero;
    }
}
