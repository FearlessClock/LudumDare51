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
    [SerializeField] private Vector2 topLeftBound;
    [SerializeField] private Vector2 botRightBound;

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

            Vector3 nextPosition = transform.position + previousDirection.normalized * speed * TimeManager.deltaTime;

            nextPosition.x = Mathf.Clamp(nextPosition.x, topLeftBound.x, botRightBound.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, botRightBound.y, topLeftBound.y);

            transform.position = new Vector2(nextPosition.x, nextPosition.y);

            animator.SetFloat("Direction", direction.x);
        }
        if (isStunned.value)
        {
            speed = 0;
        }


        if (player.GetButtonDown("Pause"))
        {
            PauseManager.Instance.setPause();
        }
    }
}
