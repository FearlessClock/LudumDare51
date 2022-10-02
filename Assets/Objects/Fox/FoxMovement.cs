using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum FoxState { WANDERING, MOVING_TO_TARGET, ATTACKING, WAITING_FOR_END_OF_EVENT }


public class FoxMovement : MonoBehaviour
{
    public Vector2 initialTarget;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float wanderSpeed = 1;
    [SerializeField] private float drag = 0.92f;
    [SerializeField] private float closeDistance = 0;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float maxWanderDistance = 2.5f;

    private ScaryObject[] scaryObjects = new ScaryObject[0];
    [SerializeField] private float avoidDistance = 1;
    [SerializeField] private float avoidanceMultiplier = 0.5f;

    private Vector3 currentTarget;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float timeBeforeAttack = 5.0f;
    private float timer = 0.0f;

    private Rigidbody2D rb = null;

    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 lastAvoidance;

    private bool hasCurrentGoal = false;
    private float waitTimer = 0;
    private bool hasWanderTarget = false;

    private FoxState currentState = FoxState.WANDERING;

    public GameObject catTarget = null;
    private bool canAttack = true;

    private float timeAttack = 0.0f;
    [SerializeField]private float attackBuffer = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        scaryObjects = FindObjectsOfType<ScaryObject>();
    }

    private void Start()
    {
        initialTarget = initialTarget + Random.insideUnitCircle * maxWanderDistance;
        MoveToPoint(initialTarget, 3); 
    }

    public void MoveToPoint(Vector3 targetPosition, float stayTime)
    {
        currentTarget = targetPosition;
        hasCurrentGoal = true;
        hasWanderTarget = false;
        waitTimer = stayTime;
        currentState = FoxState.MOVING_TO_TARGET;
    }

    private void FixedUpdate()
    {
        timer += TimeManager.fixedDeltaTime;
        timeAttack += TimeManager.fixedDeltaTime;   
        switch (currentState)
        {
            case FoxState.WANDERING:
                acceleration = WanderingState(acceleration);
                break;
            case FoxState.MOVING_TO_TARGET:
                acceleration = MoveToTargetState(acceleration);
                break;
            case FoxState.ATTACKING:
                acceleration = AttackingState(acceleration);
                break;
            case FoxState.WAITING_FOR_END_OF_EVENT:
                acceleration = WaitingAtTarget();
                break;
            default:
                break;
        }

        velocity = velocity * drag + acceleration;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        if (timer >= timeBeforeAttack && canAttack)
            currentState = FoxState.ATTACKING;

        rb.MovePosition(this.transform.position + velocity);
    }

    private Vector3 AttackingState(Vector3 acceleration)
    {

        FindCatTarget();
        if (catTarget)
            currentTarget = catTarget.transform.position;
        else
        {
            currentState = FoxState.WANDERING;
            canAttack = false;
            return acceleration;
        }

        if (Vector3.Distance(currentTarget, this.transform.position) > attackRange)
        {
            acceleration = movementSpeed * (currentTarget - this.transform.position).normalized;
            timeAttack = 0.0f;
        }
        else
        {
            if(timeAttack > attackBuffer)
            {
                timeAttack -= attackBuffer;
                catTarget.GetComponent<HealthController>().Hit(1.0f);
            }

            if(!catTarget)
            {
                currentState = FoxState.WANDERING;
                canAttack = false;
                return acceleration;
            }
        }
        acceleration += CalculateAvoidance();
        acceleration *= TimeManager.fixedDeltaTime;
        return acceleration;
    }

    private void FindCatTarget()
    {
        if (!catTarget)
            catTarget = CatManager.Instance.cats[Random.Range(0, CatManager.Instance.cats.Count)].catObject.gameObject;
    }

    private Vector3 WaitingAtTarget()
    {
        waitTimer -= TimeManager.fixedDeltaTime;

        if (waitTimer < 0)
        {
            currentState = FoxState.WANDERING;
        }

        return Vector3.zero;
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
            currentState = FoxState.WAITING_FOR_END_OF_EVENT;
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
                currentState = FoxState.WAITING_FOR_END_OF_EVENT;
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
            if (Vector3.SqrMagnitude(scaryObjects[i].transform.position - this.transform.position) < closeDistance)
            {
                lastAvoidance += -(scaryObjects[i].transform.position - this.transform.position).normalized;
            }
        }

        lastAvoidance *= avoidanceMultiplier;
        return lastAvoidance;
    }

    private void GetNewWanderTarget()
    {
        currentTarget = initialTarget + Random.insideUnitCircle * maxWanderDistance;
        waitTimer = 1;
        hasWanderTarget = true;
    }

    private void ReachedGoal()
    {

    }

    private void OnDrawGizmos()
    {
        //colider
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(currentTarget, 0.3f);

        //velocity
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position + velocity * 3);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + lastAvoidance * 3);
        
        //range of wandering
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(initialTarget, maxWanderDistance);

        //Avoid
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, avoidDistance / 2);
        
        //Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange / 2);
    }
}
