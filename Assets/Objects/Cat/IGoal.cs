using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGoalType { TIME, WANDER, ENEMY}
public interface IGoal
{
    eGoalType GoalType { get; }
    Vector3 GetGoalTarget();
    object GetGoalEnd();
}
