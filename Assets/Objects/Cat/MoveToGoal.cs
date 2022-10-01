using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : IGoal
{
    private readonly Vector3 target;
    private readonly float time;

    public eGoalType GoalType => eGoalType.TIME;

    public MoveToGoal(Vector3 target, float time)
    {
        this.target = target;
        this.time = time;
    }

    public Vector3 GetGoalTarget()
    {
        return target;
    }

    public object GetGoalEnd()
    {
        return time;
    }
}
