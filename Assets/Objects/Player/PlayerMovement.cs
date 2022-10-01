using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    [SerializeField] private float maxSpeed;
    private float speed = 0;
    private Vector3 direction;
    private Vector3 previousDirection;
    [SerializeField] private BoolVariable isStunned;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;

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
                    speed += .005f;
                    if (speed > maxSpeed)
                    {
                        speed = maxSpeed;
                    }
                }
            }
            else if (direction.sqrMagnitude < .2f && speed > 0)
            {
                speed -= .01f;
                if (speed < 0)
                {
                    speed = 0;
                }
            }

            transform.position += previousDirection.normalized * speed * Time.deltaTime;
        }
        if (isStunned.value)
        {
            speed = 0;
        }
    }
}
