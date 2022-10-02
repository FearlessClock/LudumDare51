using Codice.Client.BaseCommands;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum CatState { WAITING, WANDERING, MOVING_TO_TARGET, WAITING_FOR_END_OF_EVENT, FOLLOWING_ENEMY}

public class CatMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float wanderSpeed = 1;
    [SerializeField] private float drag = 0.92f;
    [SerializeField] private float closeDistance = 0;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float maxWanderDistance = 10;

    private ScaryObject[] scaryObjects = new ScaryObject[0];
    [SerializeField] private float avoidDistance = 1;
    [SerializeField] private float avoidanceMultiplier = 0.5f;

    [SerializeField] private float wanderWaitTimeOffset = 3;

    private Vector3 currentTarget;

    private Rigidbody2D rb = null;

    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 lastAvoidance;

    private bool hasCurrentGoal = false;
    private float waitTimer = 0;
    private bool isWaitingForWaitActionCallback = false;
    private bool hasWanderTarget = false;

    private CatState currentState = CatState.WAITING;

    public Action OnReachTarget = null;
    public Action OnFinishedWait = null;

    private Transform followingEnemy = null;
    [SerializeField] private float closeToEnemyDistance = 1;
    [SerializeField] private float tooCloseToEnemyDistance = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        scaryObjects = FindObjectsOfType<ScaryObject>();
    }

    public void MoveToPointAndWaitForTime(Vector3 targetPosition, float stayTime)
    {
        currentTarget = targetPosition;
        hasCurrentGoal = true;
        hasWanderTarget = false;
        waitTimer = stayTime;
        currentState = CatState.MOVING_TO_TARGET;
    }

    public void MoveToPointWander()
    {
        GetNewWanderTarget();
        hasCurrentGoal = true;
        currentState = CatState.MOVING_TO_TARGET;
    }

    public void MoveToEnemy(Transform enemy)
    {
        followingEnemy = enemy;
        hasCurrentGoal = true;
        currentState = CatState.FOLLOWING_ENEMY;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case CatState.WANDERING:
                acceleration = WanderingState(acceleration);
                break;
            case CatState.MOVING_TO_TARGET:
                acceleration = MoveToTargetState(acceleration);
                break;
            case CatState.WAITING_FOR_END_OF_EVENT:
                acceleration = WaitingAtTarget();
                break;
            case CatState.WAITING:
                acceleration = Vector3.zero;
                break;
            case CatState.FOLLOWING_ENEMY:
                acceleration = FollowEnemey();
                break;
            default:
                break;
        }

        velocity = velocity * drag + acceleration;
        if(velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        rb.MovePosition(this.transform.position +  velocity) ;
    }

    private Vector3 FollowEnemey()
    {
        if (Vector3.Distance(followingEnemy.position, this.transform.position) > closeToEnemyDistance)
        {
            acceleration = movementSpeed * (followingEnemy.position - this.transform.position).normalized;
        }
        else if (Vector3.Distance(followingEnemy.position, this.transform.position) < tooCloseToEnemyDistance)
        {
            acceleration = -movementSpeed * (followingEnemy.position - this.transform.position).normalized;
        }
        acceleration += CalculateAvoidance();
        acceleration *= TimeManager.fixedDeltaTime;
        return acceleration;
    }

    private Vector3 WaitingAtTarget()
    {
        if (isWaitingForWaitActionCallback)
        {
            return Vector3.zero;
        }
        else
        {
            waitTimer -= TimeManager.fixedDeltaTime;

            if (waitTimer < 0)
            {
                currentState = CatState.WAITING;
                OnFinishedWait?.Invoke();
            }

            return Vector3.zero;
        }
    }

    private Vector3 MoveToTargetState(Vector3 acceleration)
    {
        if (Vector3.Distance(currentTarget, this.transform.position) > closeDistance)
        {
            acceleration = movementSpeed * (currentTarget - this.transform.position).normalized;
        }
        else if (hasCurrentGoal)
        {
            hasCurrentGoal = false;
            currentState = CatState.WAITING_FOR_END_OF_EVENT;
            ReachedGoal();
        }
        acceleration += CalculateAvoidance();
        acceleration *= TimeManager.fixedDeltaTime;
        return acceleration;
    } 

    private Vector3 WanderingState(Vector3 acceleration)
    {
        if (hasWanderTarget)
        {
            if (Vector3.Distance(currentTarget, this.transform.position) > closeDistance)
            {
                acceleration = wanderSpeed * (currentTarget - this.transform.position).normalized;
            }
            else if (hasWanderTarget)
            {
                currentState = CatState.WAITING_FOR_END_OF_EVENT;
                hasWanderTarget = false;
            }
            acceleration += CalculateAvoidance();
            acceleration *= TimeManager.fixedDeltaTime;
        }
        else
        {
            GetNewWanderTarget();
        }

        return acceleration;
        
    }

    private Vector3 CalculateAvoidance()
    {
        lastAvoidance = Vector3.zero;
        for (int i = 0; i < scaryObjects.Length; i++)
        {
            if (Vector3.SqrMagnitude(scaryObjects[i].transform.position - this.transform.position) < closeDistance )
            {
                lastAvoidance += -(scaryObjects[i].transform.position - this.transform.position).normalized;
            }
        }

        lastAvoidance *= avoidanceMultiplier;
        return lastAvoidance;
    }

    private void GetNewWanderTarget()
    {
        currentTarget = Random.insideUnitCircle * maxWanderDistance;
        waitTimer = 1 + Random.value * wanderWaitTimeOffset;
        hasWanderTarget = true;
    }

    private void ReachedGoal()
    {
        OnReachTarget?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(currentTarget, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position + velocity * 3);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + lastAvoidance * 3);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Vector3.zero, maxWanderDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, avoidDistance/2);
    }
}
