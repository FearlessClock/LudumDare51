using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    private float speed = 0;
    private Vector3 direction;
    private Vector3 previousDirection;
    [SerializeField] private BoolVariable isStunned;
    private Animator animator;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;
        animator = GetComponent<Animator>();
        isStunned = GetComponent<PlayerHandler>().isStunned;

    }
    void Update()
    {

        if (player != null && !isStunned.value)
        {
            direction = new Vector2(player.GetAxis("Horizontal"), player.GetAxis("Vertical"));

            if (direction.sqrMagnitude > .2f)
            {
                previousDirection = direction;
                if (speed < maxSpeed)
                {
                    speed += accelerationRate * TimeManager.fixedDeltaTime;
                    if (speed > maxSpeed)
                    {
                        speed = maxSpeed;
                    }
                }
            }
            else if (direction.sqrMagnitude < .2f && speed > 0)
            {
                speed -= decelerationRate * TimeManager.fixedDeltaTime;
                if (speed < 0)
                {
                    speed = 0;
                }
            }

            transform.position += previousDirection.normalized * speed * TimeManager.deltaTime;

            animator.SetFloat("Direction", direction.x);
        }
        if (isStunned.value)
        {
            speed = 0;
        }
    }
}
