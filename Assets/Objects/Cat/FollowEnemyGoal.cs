using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyGoal : IGoal
{
    public Transform target = null;

    public FollowEnemyGoal(Transform transform)
    {
        this.target = transform;
    }

    public eGoalType GoalType => eGoalType.ENEMY;

    public object GetGoalEnd()
    {
        return target;
    }

    public Vector3 GetGoalTarget()
    {
        return target.position;
    }
}
